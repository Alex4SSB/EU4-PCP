using EU4_PCP.Models;
using EU4_PCP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Implementations;
using static EU4_PCP.PCP_Paths;
using static EU4_PCP.PCP_RegEx;

namespace EU4_PCP
{
    public static class MainCode
	{
		public static readonly Color RedBackground = Color.FromRgb(0xDE, 25, 25);
		public static readonly Color GreenBackground = Color.FromRgb(0x7C, 0xB6, 0x1A);

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

			var lastSelMod = Security.RetrieveValue(General.LastSelMod);

			if (Security.RetrieveBoolEnum(AutoLoad.Fully)
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
			if (!Security.RetrieveBoolEnum(ProvinceNames.Definition))
			{
				EnLoc = true;
				EnDyn = Security.RetrieveBoolEnum(ProvinceNames.Dynamic);
			}
			else
			{
				EnLoc =
				EnDyn = false;
			}

			CheckDupli = Security.RetrieveBool(General.CheckDupli);
            ShowRnw = Security.RetrieveBool(General.ShowAllProvinces);
			UpdateCountries = false;

            ClearArrays();
			StartDate = DateTime.MinValue;

            if (BookStatus(true) && !DefinSetup(DecidePath()))
				return ErrorMsg(ErrorType.DefinRead);

			if (!EnDyn)
			{
				FetchDefines();
				DefinesPrep();
				if (!ValDate()) return false;
			}

			if (!LocalisationSequence()) return false;
			if (!EnDyn) DynamicSetup();
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
		/// Checks game validity.
		/// </summary>
		/// <returns><see langword="false"/> upon failure or if auto-loading is disabled.</returns>
		private static bool ValGame()
		{
			string tempGamePath = Security.RetrieveValue(General.GamePath);

			if (Security.RetrieveBoolEnum(AutoLoad.Disable) ||
				string.IsNullOrEmpty(tempGamePath) ||
				!PathHandler(Scope.Game)) return false;
			GamePath = tempGamePath;

			return true;
		}

		/// <summary>
		/// Handles path validation for game and mod.
		/// </summary>
		/// <param name="scope">Game / Mod.</param>
		/// <returns><see langword="true"/> if the validation was successful.</returns>
		public static bool PathHandler(Scope scope) => PathHandler(scope, PathRead(scope));

		/// <summary>
		/// Handles path validation for game and mod.
		/// </summary>
		/// <param name="scope">Game / Mod.</param>
		/// <param name="setting">The setting from which to read.</param>
		/// <returns><see langword="true"/> if the validation was successful.</returns>
		private static bool PathHandler(Scope scope, string setting)
		{
			if (setting.Contains('|')) // Legacy support
				setting = setting.Split('|')[0];

			switch (scope)
			{
				case Scope.Game when !File.Exists(setting + GameFile):
					return ErrorMsg(ErrorType.GameExe);
				case Scope.Mod:
					if (string.IsNullOrEmpty(setting))
					{
						var path = SelectedDocsPath + ModPath;
						if (Directory.Exists(path))
						{
							ParadoxModPath =
							setting = path;
						}
						else return false;
					}
					else 
						ParadoxModPath = setting;
					break;
				default:
					break;
			}
			PathWrite(scope, setting);

			return true;
		}

		/// <summary>
		/// Reads path from game / mod settings.
		/// </summary>
		/// <param name="scope">Game / Mod.</param>
		/// <returns>The path as string.</returns>
		private static string PathRead(Scope scope)
		{
			if (Security.RetrieveValue(Enum.GetName(typeof(Scope), scope) + "Path") is string str)
				return str;
			else
				return "";
		}

		private static void PathWrite(Scope scope, string path)
		{
			Security.StoreValue(path, Enum.GetName(typeof(Scope), scope) + "Path");
		}

		/// <summary>
		/// Decide whether to update the bookmarks - if a bookmark is selected.
		/// <br /> <br />
		/// The function is used to prevent updating the bookmarks CBs when a bookmark is selected,
		/// thus preventing the start date from changing, which in turn prevents useless
		/// activation of some sequences again.
		/// <br />
		/// Optionally combined with enabled bookmarks check, to disable some sequences
		/// when the bookmarks are disabled.
		/// </summary>
		/// <param name="enabled"><see langword="true"/> to ignore bookmarks being enabled or disabled 
		/// (tied to dynamic enabled).</param>
		/// <returns><see langword="true"/> if Bookmarks should be updated.</returns>
		private static bool BookStatus(bool enabled)
		{
			return (enabled || Security.RetrieveBoolEnum(ProvinceNames.Dynamic)) &&
				SelectedBookmarkIndex < 1;
		}

		/// <summary>
		/// A mini function for the localisation sequence and the calling of the dynamic sequence.
		/// </summary>
		/// <returns>DynamicSequence result.</returns>
		private static bool LocalisationSequence()
		{
			if (!EnLoc) return true;

			if (!FetchFiles(FileType.Localisation))
				return ErrorMsg(ErrorType.LocFolder);

			if (!LocPrep(LocScope.ProvLoc))
				return ErrorMsg(ErrorType.LocRead);

			return DynamicSequence();
		}

		/// <summary>
		/// The full sequence of calculating the dynamic province names.
		/// </summary>
		/// <returns><see langword="false"/> on failure.</returns>
		private static bool DynamicSequence()
		{
			if (!EnDyn) return true;
			bool enBooks = BookStatus(false);
			bool success = true;

			Parallel.Invoke(
				() => CulturePrep(),
				() => FetchDefines());

			if (!Cultures.Any())
				return ErrorMsg(ErrorType.NoCultures);
			else if (!Cultures.Where(cul => cul && cul.Group).Any())
				return ErrorMsg(ErrorType.NoCulGroups);

            Parallel.Invoke(
				() => DefinesPrep(),
				() => { if (enBooks) FetchFiles(FileType.Bookmark); });

			Parallel.Invoke(
				() => { if (enBooks) BookPrep(); },
				() => FetchFiles(FileType.Country));

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
		/// Reads game version from the game logs.
		/// </summary>
		private static void GameVer()
		{
			var gameVer = "Game";
			var logText = "";

			try
			{
				logText = File.ReadAllText(SelectedDocsPath + GameLogPath, UTF8);
			}
			catch (Exception) { }

			if (GameVerRE.Match(logText) is Match match && match.Success)
				gameVer += $" - {match.Value}";

			GameVersion = gameVer;
		}

		/// <summary>
		/// A smart count of overall Provinces.
		/// </summary>
		/// <param name="scope"></param>
		private static void CountProv(Scope scope)
		{
			var provCount = Provinces.Count(p => p && !string.IsNullOrWhiteSpace(p.ToString())).ToString();

			switch (scope)
			{
				case Scope.Game:
					GameProvinceCount = provCount;
					ModProvinceCount = "";
					break;
				case Scope.Mod:
					ModProvinceCount = provCount;
					break;
				default:
					break;
			}
        }

		/// <summary>
		/// Update the bookmark ComboBox with all relevant bookmarks.
		/// </summary>
		private static void PopulateBooks()
		{
			if (!BookStatus(false)) return;

			if (!Bookmarks.Any(b => b.Code != null))
			{
				BookmarkList = null;
				return;
			}

			BookmarkList = Bookmarks.Select(b => b.Name).ToList();

			SelectedBookmarkIndex = 0;
		}

		/// <summary>
		/// Default file max provinces.
		/// </summary>
		/// <param name="scope">Game / Mod.</param>
		/// <returns><see langword="false"/> upon failure.</returns>
		private static bool MaxProvinces(Scope scope)
		{
			var filePath = (scope == Scope.Mod) ? SteamModPath : GamePath;

			string d_file;
			try
			{
				d_file = File.ReadAllText(filePath + DefMapPath, UTF8);
			}
			catch (Exception)
			{
				return ErrorMsg(ErrorType.DefMapRead);
			}
			var match = MaxProvRE.Match(d_file);

			if (!match.Success)
			{
				switch (scope)
				{
					case Scope.Game:
						GameMaxProvinces = "";
						break;
					case Scope.Mod:
						ModMaxProvinces = "";
						SelectedModIndex = -1;
						break;
					default:
						break;
				}
				return ErrorMsg(ErrorType.DefMapMaxProv);
			}

			switch (scope)
			{
				case Scope.Game:
					GameMaxProvinces = match.Value;
					ModMaxProvinces = "";
					break;
				case Scope.Mod:
					ModMaxProvinces = match.Value;
					break;
				default:
					break;
			}

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
                Security.StoreValue(SelectedMod ? SelectedMod.Name : "-1", General.LastSelMod.ToString());
            else if (SelectedMod)
			{
				SelectedModIndex = 0;
				ChangeMod();
			}
		}

		/// <summary>
		/// Combines the game and mod bookmark CB index changed handlers.
		/// </summary>
		public static void EnactBook()
		{
			if (Lockdown) return;
			if (SelectedBookmarkIndex < 0)
            {
				if (BookmarkList.Any())
					SelectedBookmarkIndex = 0;
				else return;
            }

			StartDate = Bookmarks[SelectedBookmarkIndex].BookDate;
			StartDateStr = StartDate.ToString(DATE_FORMAT);

			ShowRnw = Security.RetrieveBool(General.ShowAllProvinces);
			UpdateCountries = true;
			CountryCulSetup();
			OwnerSetup(true);
			ProvNameSetup();
			DynamicSetup();
		}

		/// <summary>
		/// Invokes the relevant procedures if the corresponding properties were changed.
		/// </summary>
		public static void UpdateProperties()
        {
			if (Security.RetrieveBool(General.ShowAllProvinces) is bool showRnw && showRnw != ShowRnw)
            {
				ShowRnw = showRnw;

				foreach (var prov in Provinces.Where(prov => prov && prov.Name))
				{
					prov.Show = !prov.IsRNW() || (ShowRnw && !string.IsNullOrEmpty(prov.Name.ToString()));
				}
				ProvincesShown = Provinces.Count(prov => prov && prov.Show).ToString();

				DupliPrep();
			}
			if (Security.RetrieveBool(General.CheckDupli) is bool checkDupli && checkDupli != CheckDupli)
            {
				CheckDupli = checkDupli;

				DupliPrep();
            }
			if (Security.RetrieveBoolEnum(ProvinceNames.Dynamic) != EnDyn || Security.RetrieveBoolEnum(ProvinceNames.Localisation) != EnLoc)
				MainSequence();
		}

		/// <summary>
		/// Updates the definition.csv file by writing all current provinces.
		/// </summary>
		/// <returns><see langword="false"/> if the operation was not successful.</returns>
		public static bool WriteProvinces()
        {
			string stream = "province;red;green;blue;x;x\r\n";

            foreach (var prov in Provinces.Where(p => p.Index >= 0))
            {
				stream += prov.ToCsv() + "\r\n";
            }

            try
            {
				File.WriteAllBytes(SteamModPath + DefinPath, stream.Select(c => (byte)c).ToArray());
			}
            catch (Exception)
            {
				return ErrorMsg(ErrorType.DefinWrite);
            }

			return true;
        }

		/// <summary>
		/// Updates the default.map file.
		/// </summary>
		/// <param name="newMax">The new max_provinces value</param>
		/// <returns></returns>
		public static bool WriteDefines(string newMax)
        {
			string defMap;
			try
			{
				defMap = File.ReadAllText(SteamModPath + DefMapPath);
			}
			catch (Exception)
			{
				return ErrorMsg(ErrorType.DefMapRead);
			}
			defMap = DefMapRE.Replace(defMap, $"max_provinces = {newMax}");
			try
			{
				File.WriteAllText(SteamModPath + DefMapPath, defMap, UTF8);
			}
			catch (Exception)
			{
				return ErrorMsg(ErrorType.DefMapWrite);
			}

			return true;
        }

		/// <summary>
		/// Handles duplicate provinces - sets for each province its next duplicate.
		/// </summary>
		public static void DupliPrep()
        {
            Provinces.ForEach(prov => prov.NextDupli = null);
            if (!CheckDupli || !SelectedMod) return;

			var dupliGroups = (from prov in Provinces
							  where prov && prov.Show
							  group prov by prov.Color)
							  .Where(g => g.Count() > 1);
							  
            foreach (var group in dupliGroups)
            {
                for (int i = 0; i < group.Count(); i++)
                {
					var nextIndex = (i == group.Count() - 1) ? 0 : i + 1;

					group.ElementAt(i).NextDupli = group.ElementAt(nextIndex);
                }
            }
        }

		/// <summary>
		/// Adds the given <see cref="Province"/> to the list if it doesn't exist. Otherwise, updates its color and definition name.
		/// </summary>
		/// <param name="newProv">The province to add / update</param>
		/// <returns></returns>
		public static bool AddProv(Province newProv)
		{
			if (ChosenProv)
			{
				var prov = Provinces.Find(prov => prov.Index == ChosenProv.ID);
				prov.Color = newProv.Color;
				prov.Name.Definition = newProv.Name.Definition;
			}
			else
			{
				newProv.IsRNW();
				Provinces.Add(newProv);

				ModMaxProvinces = Security.RetrieveBool(General.IterateMaxProv)
					? Inc(ModMaxProvinces, 1)
					: Inc(ModProvinceCount, 2);

				if (Security.RetrieveBool(General.UpdateMaxProv) && !WriteDefines(ModMaxProvinces))
					return false;

				ModProvinceCount = Inc(ModProvinceCount, 1);
				ProvincesShown = Provinces.Count(prov => prov && prov.Show).ToString();
			}

			if (!WriteProvinces())
				return false;

			ChosenProv = null;
			
			return true;
		}
	}
}
