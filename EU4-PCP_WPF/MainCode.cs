using EU4_PCP_WPF.Models;
using EU4_PCP_WPF.Services;
using EU4_PCP_WPF.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static EU4_PCP_WPF.PCP_Const;
using static EU4_PCP_WPF.PCP_Data;
using static EU4_PCP_WPF.PCP_Implementations;
using static EU4_PCP_WPF.PCP_Paths;
using static EU4_PCP_WPF.PCP_RegEx;

namespace EU4_PCP_WPF
{
    public static class MainCode
	{
		public static List<TableProvince> ProvTableList = new List<TableProvince>();

		public static void PCP_Main()
		{
			LaunchSequence();
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

				//lockdown = false;
				SelectedModIndex = index;
				// TODO: Activate selected index changed
				//lockdown = true;
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

			ShowRnw = Security.RetrieveBool(General.ShowAllProvinces);
			UpdateCountries = false;

            //// Repaint duplicate rows before clearing the list
            //PaintDupli(true);
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
			PopulateTable();
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

			//ClearCP(); // Clear the color picker, and call the randomizer

			//DupliPrep();

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
			DocsPrep();

			return true;
		}

		/// <summary>
		/// Handles path validation for game and mod.
		/// </summary>
		/// <param name="scope">Game / Mod.</param>
		/// <returns><see langword="true"/> if the validation was successful.</returns>
		private static bool PathHandler(Scope scope) => PathHandler(scope, PathRead(scope));

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
					//SettingsWrite(scope);
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
					else ParadoxModPath = setting;
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
			return Security.RetrieveValue(Enum.GetName(typeof(Scope), scope) + "Path").ToString();
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

			if (!LocPrep(LocScope.Province))
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
		/// A smart count of overall provinces and shown provinces. <br />
		/// </summary>
		/// <param name="scope"></param>
		private static void CountProv(Scope scope)
		{
			var provCount = Provinces.Count(p => p && p.ToString().Length > 0).ToString();

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
            //ProvincesShown = ProvTable.Rows.Count.ToString();
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
		/// Writes all provinces to the ProvTable and paints it accordingly.
		/// </summary>
		private static void PopulateTable()
		{
			//ProvTableList = Provinces.Where(prov => prov && prov.Show).Select(prov => (TableProvince)prov).ToList();

			//ProvTableList = (from prov in Provinces
			//				where prov && prov.Show
			//				select (TableProvince)prov).ToList();

			//Province[] selProv = Provinces.Where(prov => prov && prov.Show).ToArray();
			//var oldCount = ProvTable.RowCount;
			//ProvTable.RowCount = selProv.Length;

			//PaintTable(selProv.Length, oldCount);

			//for (int prov = 0; prov < selProv.Length; prov++)
			//{
			//    ProvTable[0, prov].Style.BackColor = selProv[prov].Color;
			//    ProvTable.Rows[prov].SetValues(selProv[prov].ToRow());
			//    provinces[selProv[prov].Index].TableIndex = prov;
			//}
			//ProvTableSB.Maximum = ProvTable.RowCount - ProvTable.DisplayedRowCount(false) + 1;
			//ProvTable.ClearSelection();
			//ProvTableSB.Visible = ProvTableSB.Maximum >= 1;
		}

		/// <summary>
		/// Handles mod changing.
		/// </summary>
		public static void ChangeMod()
		{
			//if (SelectedModIndex < 1)
			//	Critical(CriticalType.Begin);
			//else
			//	Critical(CriticalType.Begin, CriticalScope.Mod);

			//ModStartDateTB.Text = "";
			if (SelectedModIndex < 1)
			{
				SelectedMod = null;
				SteamModPath = "";
				//ColorPickerGB.Enabled =
				//ModInfoGB.Visible = false;
				//SelectedBookmarkIndex = -1;
			}
			else
			{
				//if (!ModInfoGB.Visible)
				//{
				//	if (GameBookmarkCB.Items.Count > 0)
				//	{
				//		GameBookmarkCB.SelectedIndex = 0;
				//		if (!selectedMod)
				//		{
				//			//GameBookmarkCB.Enabled = false;
				//			StartDate = Bookmarks[GameBookmarkCB.SelectedIndex].StartDate;
				//			GameStartDateTB.Text = StartDate.ToString(DATE_FORMAT);
				//		}
				//	}
				//	//ColorPickerGB.Enabled =
				//	//ModInfoGB.Visible = true;
				//}
				SelectedMod = Mods[SelectedModIndex - 1];
				SteamModPath = SelectedMod.Path;
			}

			SelectedBookmarkIndex = BookmarkList != null && BookmarkList.Any() ? 0 : -1;

			//var success = MainSequence();
			if (!MainSequence())
			{
				if (SelectedMod)
				{
					SelectedModIndex = 0;
					ChangeMod();
				}
				//else ClearScreen();
			}
			else if (SelectedModIndex > 0)
				Security.StoreValue(SelectedMod.Name, General.LastSelMod.ToString());

			//Critical(CriticalType.Finish, success);
		}

		/// <summary>
		/// Combines the game and mod bookmark CB index changed handlers.
		/// </summary>
		public static void EnactBook()
		{
			if (Lockdown) return;
			//Critical(CriticalType.Begin, CriticalScope.Bookmark);

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
			PopulateTable();

			//Critical(CriticalType.Finish, true);
		}
	}
}