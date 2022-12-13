using EU4_PCP.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace EU4_PCP
{
    public static class PCP_Data
    {
        // BOOLEANS
        public static bool ShowRnw;
        public static bool UpdateCountries;
        public static bool Lockdown;
        public static bool CheckDupli;
        public static bool ShowIllegal;
        public static AutoLoad LoadValue;

        // GLOBAL OBJECTS
        public static ModObj SelectedMod;
        public static DateTime BeginTiming;
        public static DateTime FinishTiming;
        public static DateTime StartDate = DateTime.MinValue;

        // STRING LISTS
        public static List<string> DefinesFiles = new();
        public static List<string> CultureFiles = new();

        // CLASS DICT AND LISTS
        public static Dictionary<int, Province> Provinces = new();
        public static List<Country> Countries = new();
        public static List<Culture> Cultures = new();
        public static List<Bookmark> Bookmarks = new();
        public static List<ModObj> Mods = new();
        public static List<FileObj> CountryFiles = new();
        public static List<FileObj> BookFiles = new();
        public static List<FileObj> ProvFiles = new();
        public static List<FileObj> ProvNameFiles = new();

        // Control public data sources
        public static List<string> ModList = new();
        public static int SelectedModIndex = -1;
        public static string StartDateStr;
        public static string GameVersion;
        public static string ModVersion;
        public static List<ListBookmark> BookmarkList = new();
        public static int SelectedBookmarkIndex = -1;
        public static string GameProvinceCount;
        public static string GameMaxProvinces;
        public static string ModProvinceCount;
        public static string ModDupliProvinceCount;
        public static string ModIllegalProvinceCount;
        public static string ModMaxProvinces;
        public static double ProvTableIndex = 0;
        public static Color ColorPickerPickedColor;
        public static TableProvince ChosenProv;
        public static bool NavigateToColorPicker;
        public static bool NavigateToSettings;
        public static int SelectedGridRow = -1;
        public static bool AreBooksOverridden;
        public static bool OverrideBooks;
    }
}
