using EU4_PCP.Models;
using EU4_PCP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Implementations;
using static EU4_PCP.PCP_Paths;

namespace EU4_PCP
{
	public static class PCP_Logic
	{
		private static ProvinceNames Naming;
		/// <summary>
		/// Main entry point from launch
		/// </summary>
		public static void PCP_Main()
		{
			if (!LaunchSequence())
				NavigateToSettings = true;
		}

		/// <summary>
		/// A sequence that runs on app start and when selecting/changing game/mod path.
		/// </summary>
		/// <returns>MainSequence result, or <see langword="false"/> ValGame result.</returns>
		private static bool LaunchSequence()
		{
			if (!ValGame()) return false;

			var modNames = new List<string> { "[Vanilla - no mod]" };
			if (PathHandler(Scope.Mod))
			{
				ModPrep();
				modNames.AddRange(Mods.Select(m => m.Name));
			}
			ModList = modNames;

			var lastSelMod = Storage.RetrieveValue(General.LastSelMod);

			if (Storage.RetrieveBoolEnum(AutoLoad.Fully)
				&& modNames.IndexOf(lastSelMod) is int index
				&& index > -1)
			{
				if (!MainSequence()) return false;

				SelectedModIndex = index;
				ChangeMod();

				return true;
			}
			else SelectedModIndex = 0;

			return MainSequence();
		}

		/// <summary>
		/// Includes all essential sequences that run every time, except when changing bookmarks.
		/// </summary>
		/// <returns><see langword="false"/> if any of the sub-sequences fails.</returns>
		private static bool MainSequence()
		{
			Naming = (ProvinceNames)Storage.RetrieveEnumGroup(typeof(ProvinceNames));
			CheckDupli = Storage.RetrieveBool(General.CheckDupli);
			ShowRnw = Storage.RetrieveBool(General.ShowAllProvinces);
			UpdateCountries = false;

			ClearArrays();
			StartDate = DateTime.MinValue;

			if (BookStatus(true) && !DefinSetup(DecidePath()))
				return ErrorMsg(ErrorType.DefinRead);

			if (Naming != ProvinceNames.Dynamic)
			{
				FetchDefines();
				DefinesPrep();
				if (!ValDate()) return false;
			}

			if (!LocalisationSequence()) return false;
			if (Naming != ProvinceNames.Dynamic) DynamicSetup();
			GameVer();

			StartDateStr = StartDate.ToString(DATE_FORMAT);
			ModVersion = "Mod";
			if (SelectedMod)
			{
				ModVersion += $" - {SelectedMod.Ver}";
			}

			CountProv(SelectedMod);
			PopulateBooks();

			// Unless a bookmark is selected, perform relevant max_provinces check
			if (BookStatus(true) &&
				(!SelectedMod && !MaxProvinces(Scope.Game)) ||
				(SelectedMod && !MaxProvinces(Scope.Mod)))
				return false;

			DupliPrep();

			return true;
		}

		/// <summary>
		/// A mini function for the localisation sequence and the calling of the dynamic sequence.
		/// </summary>
		/// <returns>DynamicSequence result.</returns>
		private static bool LocalisationSequence()
		{
			if (Naming == ProvinceNames.Definition) return true;

			if (Naming == ProvinceNames.Dynamic) // BookStatus(false)
			{
				FetchFiles(FileType.Bookmark);
				BookPrep();
			}

            PathIndexer($@"{GamePath}\#localisation#", Scope.Game, Naming == ProvinceNames.Dynamic);

   //         if (!FetchFiles(FileType.Localisation))
			//	return ErrorMsg(ErrorType.LocFolder);

			//if (!LocPrep(LocScope.ProvLoc))
			//	return ErrorMsg(ErrorType.LocRead);

			return DynamicSequence();
		}

		/// <summary>
		/// The full sequence of calculating the dynamic province names.
		/// </summary>
		/// <returns><see langword="false"/> on failure.</returns>
		private static bool DynamicSequence()
		{
			if (Naming != ProvinceNames.Dynamic) return true;
			bool enBooks = BookStatus(false);
			bool success = true;

			Parallel.Invoke(
				() => CulturePrep(),
				() => FetchDefines());

			if (!Cultures.Any())
				return ErrorMsg(ErrorType.NoCultures);
			else if (!Cultures.Where(cul => cul && cul.Group).Any())
				return ErrorMsg(ErrorType.NoCulGroups);

			//Parallel.Invoke(
			//	() => DefinesPrep(),
			//	() => { if (enBooks) FetchFiles(FileType.Bookmark); });

			Parallel.Invoke(
				() => DefinesPrep(),
				() => FetchFiles(FileType.Country));
			
			//Parallel.Invoke(
			//	() => { if (enBooks) BookPrep(); },
			//	() => FetchFiles(FileType.Country));

			if (!ValDate()) return false;

			Parallel.Invoke(
				() => CountryCulSetup(),
				() => success = FetchFiles(FileType.Province));

			if (!success)
				return ErrorMsg(ErrorType.HistoryProvFolder);

			if (!Countries.Any())
				return ErrorMsg(ErrorType.NoCountries);

			Parallel.Invoke(
				() => OwnerSetup(),
				() => FetchFiles(FileType.ProvName));

			ProvNameSetup();
			DynamicSetup();

			return true;
		}

		/// <summary>
		/// Handles mod changing.
		/// </summary>
		public static void ChangeMod()
		{
			if (SelectedModIndex < 1)
			{
				SelectedMod = null;
				SteamModPath = "";
			}
			else
			{
				SelectedMod = Mods[SelectedModIndex - 1];
				SteamModPath = SelectedMod.Path;
			}
			SelectedBookmarkIndex = BookmarkList != null && BookmarkList.Any() ? 0 : -1;

			if (MainSequence())
				Storage.StoreValue(SelectedMod ? SelectedMod.Name : "-1", General.LastSelMod.ToString());
			else if (SelectedMod)
			{
				SelectedModIndex = 0;
				ChangeMod();
			}
		}

		/// <summary>
		/// Invokes the relevant procedures if the corresponding properties were changed.
		/// </summary>
		public static void UpdateProperties()
		{
			if (Storage.RetrieveBool(General.ShowAllProvinces) is bool showRnw && showRnw != ShowRnw)
			{
				ShowRnw = showRnw;

				foreach (var prov in Provinces.Where(prov => prov && prov.Name))
				{
					prov.Show = !prov.IsRNW() || (ShowRnw && !string.IsNullOrEmpty(prov.Name.ToString()));
				}
				ProvincesShown = Provinces.Count(prov => prov && prov.Show).ToString();

				DupliPrep();
			}
			
			if (Storage.RetrieveBool(General.CheckDupli) is bool checkDupli && checkDupli != CheckDupli)
			{
				CheckDupli = checkDupli;

				DupliPrep();
			}

			if (Naming != (ProvinceNames)Storage.RetrieveEnumGroup(typeof(ProvinceNames)))
				MainSequence();

			if (Storage.RetrieveBool(General.ShowIllegalProv) != ShowIllegal)
				MainSequence();
		}

	}
}
