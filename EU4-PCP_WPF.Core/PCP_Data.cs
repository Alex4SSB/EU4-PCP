using System;
using System.Collections.Generic;
using System.Drawing;

namespace EU4_PCP_WPF
{
	public static class PCP_Data
	{
		// BOOLEANS
		public static bool ShowRnw;
		public static bool EnLoc;
		public static bool EnDyn;
		public static bool UpdateCountries;
		public static bool Lockdown = false;

		// GLOBAL OBJECTS
		public static ModObj SelectedMod;
		public static DateTime BeginTiming;
		public static DateTime FinishTiming;
		public static DateTime StartDate = DateTime.MinValue;

		// STRING LISTS
		public static List<string> DefinesFiles = new List<string>();
		public static List<string> CultureFiles = new List<string>();

		// CLASS ARRAYS AND LISTS
		public static List<Province> Provinces = new List<Province>();
		public static List<Country> Countries = new List<Country>();
		public static List<Culture> Cultures = new List<Culture>();
		public static List<Bookmark> Bookmarks = new List<Bookmark>();
		public static List<MembersCount> Members = new List<MembersCount>();
		public static List<ModObj> Mods = new List<ModObj>();
		public static List<FileObj> LocFiles = new List<FileObj>();
		public static List<FileObj> CountryFiles = new List<FileObj>();
		public static List<FileObj> BookFiles = new List<FileObj>();
		public static List<FileObj> ProvFiles = new List<FileObj>();
		public static List<FileObj> ProvNameFiles = new List<FileObj>();
		public static List<Dupli> Duplicates = new List<Dupli>();

		// Control public data sources
		public static List<string> ModList = new List<string>();
		public static int SelectedModIndex = -1;
		public static string StartDateStr;
		public static string GameVersion;
		public static string ModVersion;
		public static List<string> BookmarkList = new List<string>();
		public static int SelectedBookmarkIndex = -1;
		public static string GameProvinceCount;
		public static string GameMaxProvinces;
		public static string ModProvinceCount;
		public static string ModMaxProvinces;
		public static string ProvincesShown;
		public static double ProvTableIndex = 0;
		public static Color ColorPickerPickedColor;
		public static TableProvince ChosenProv;
	}
}
