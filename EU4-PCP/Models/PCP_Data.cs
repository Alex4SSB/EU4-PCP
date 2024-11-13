using EU4_PCP.Models;

namespace EU4_PCP;

public static class PCP_Data
{
    // BOOLEANS
    public static bool ShowRnw { get; set; }
    public static bool UpdateCountries { get; set; }
    public static bool Lockdown { get; set; }
    public static bool CheckDupli { get; set; }
    public static bool ShowIllegal { get; set; }
    public static AutoLoad LoadValue { get; set; }

    // GLOBAL OBJECTS
    public static ModObj SelectedMod { get; set; }
    public static DateTime BeginTiming { get; set; }
    public static DateTime FinishTiming { get; set; }
    public static DateTime StartDate { get; set; } = DateTime.MinValue;

    // STRING LISTS
    public static List<string> DefinesFiles { get; set; } = [];
    public static List<string> CultureFiles { get; set; } = [];

    // CLASS DICT AND LISTS
    public static Dictionary<int, Province> Provinces { get; set; } = [];
    public static List<Country> Countries { get; set; } = [];
    public static List<Culture> Cultures { get; set; } = [];
    public static List<Bookmark> Bookmarks { get; set; } = [];
    public static List<ModObj> Mods { get; set; } = [];
    public static List<FileObj> CountryFiles { get; set; } = [];
    public static List<FileObj> BookFiles { get; set; } = [];
    public static List<FileObj> ProvFiles { get; set; } = [];
    public static List<FileObj> ProvNameFiles { get; set; } = [];

    // Control public data sources
    public static List<string> ModList { get; set; } = [];
    public static List<ListBookmark> BookmarkList { get; set; } = [];

    public static Color ColorPickerPickedColor { get; set; }
    public static TableProvince ChosenProv { get; set; }

    public static int SelectedModIndex { get; set; } = -1;
    public static string StartDateStr => StartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
    public static string GameVersion { get; set; }
    public static string ModVersion { get; set; }
    public static int SelectedBookmarkIndex { get; set; } = -1;
    public static string GameProvinceCount { get; set; }
    public static string GameMaxProvinces { get; set; }
    public static string ModProvinceCount { get; set; }
    public static string ModDupliProvinceCount { get; set; }
    public static string ModIllegalProvinceCount { get; set; }
    public static string ModMaxProvinces { get; set; }
    public static double ProvTableIndex { get; set; } = 0;
    
    public static bool NavigateToColorPicker { get; set; }
    public static bool NavigateToSettings { get; set; }
    public static int SelectedGridRow { get; set; } = -1;
    public static bool AreBooksOverridden { get; set; }
    public static bool OverrideBooks { get; set; }


    public static Notifiable Notifiable { get; set; } = new();
}
