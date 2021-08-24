using EU4_PCP.Models;
using EU4_PCP.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                ModSetup();
                modNames.AddRange(Mods.Select(m => m.Name));
            }
            ModList = modNames;

            var lastSelMod = Storage.RetrieveValue(General.LastSelMod);
            LoadValue = (AutoLoad)Storage.RetrieveEnumGroup(typeof(AutoLoad));

            if (LoadValue == AutoLoad.Fully
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
            OverrideBooks = Storage.RetrieveBool(General.OverrideBooks);
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

            StartDateStr = CurrentDateFormat(true, StartDate);
            ModVersion = "Mod";
            if (SelectedMod)
            {
                ModVersion += $" - {SelectedMod.GameVer}";
            }

            CountProv(SelectedMod);
            PopulateBooks();

            // Unless a bookmark is selected, perform relevant max_provinces check
            if ((BookStatus(true) &&
                !SelectedMod && !MaxProvinces(Scope.Game)) ||
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
            BookmarkSequence();

            if (!LocalisationSetup(Naming == ProvinceNames.Dynamic))
                return ErrorMsg(ErrorType.LocRead);

            return DynamicSequence();
        }

        private static void BookmarkSequence()
        {
            if (Naming != ProvinceNames.Dynamic || SelectedBookmarkIndex > 0)
                return;

            AreBooksOverridden = false;
            BookFiles.Clear();

            FetchFiles(FileType.Bookmark);
            BookSetup();
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
            else if (!Cultures.Any(cul => cul && cul.Group))
                return ErrorMsg(ErrorType.NoCulGroups);

            Parallel.Invoke(
                () => DefinesPrep(),
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
            if (Storage.RetrieveEnumGroup(typeof(AutoLoad)) is AutoLoad value
                && LoadValue == AutoLoad.Disable && value != AutoLoad.Disable)
                LaunchSequence();
            else if (Naming != (ProvinceNames)Storage.RetrieveEnumGroup(typeof(ProvinceNames)))
                MainSequence();
            else if (Storage.RetrieveBool(General.ShowIllegalProv) != ShowIllegal)
                MainSequence();
            else if (Storage.RetrieveBool(General.ShowAllProvinces) is bool showRnw && showRnw != ShowRnw)
            {
                ShowRnw = showRnw;

                foreach (var prov in Provinces.Values.Where(prov => prov && prov.Name))
                {
                    prov.Show = !prov.IsRNW() || (ShowRnw && !string.IsNullOrEmpty(prov.Name.ToString()));
                }
                ProvincesShown = Provinces.Values.Count(prov => prov && prov.Show).ToString();

                DupliPrep();
            }
            else if (Storage.RetrieveBool(General.CheckDupli) is bool checkDupli && checkDupli != CheckDupli)
            {
                CheckDupli = checkDupli;

                DupliPrep();
            }
            else if (Storage.RetrieveBool(General.OverrideBooks) is bool overBooks && overBooks != OverrideBooks)
            {
                OverrideBooks = overBooks;
                if (Naming != ProvinceNames.Dynamic)
                    return;

                if (overBooks)
                {
                    BookmarkSequence();
                    LocalisationSetup(true, false);
                    PopulateBooks();
                }
                else if (AreBooksOverridden)
                {
                    BookmarkList.Clear();
                    AreBooksOverridden = false;
                }
            }
        }

    }
}
