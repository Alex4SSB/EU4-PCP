namespace EU4_PCP;

public static class PCP_Paths
{
    // DOCS FOLDERS
    public static string ParadoxModPath { get; set; } = ""; // The folder that contains the .mod files
    public static string SteamModPath { get; set; } = ""; // Current Mod - parsed from the .mod file

    public static readonly string SelectedDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DOCS_FOLDER; // OneDrive / Offline

    public const string DOCS_FOLDER = @"\Paradox Interactive\Europa Universalis IV";
    public const string MOD_PATH = @"\mod";
    public const string GAME_LOG_PATH = @"\logs\game.log";

    // MAIN FOLDERS
    public static string GamePath { get; set; } = "";

    public const string GAME_FILE = @"\eu4.exe";
    public const string DEFIN_PATH = @"\map\definition.csv";
    public const string DEF_MAP_PATH = @"\map\default.map";
    public const string LOC_PATH = @"\localisation";
    public const string REP_LOC_PATH = @"\localisation\replace";
    public const string HIST_PROV_PATH = @"\history\provinces";
    public const string HIST_COUNTRY_PATH = @"\history\countries";
    public const string CULTURE_PATH = @"\common\cultures";
    public const string CULTURE_FILE = @"\00_cultures.txt";
    public const string PROV_NAMES_PATH = @"\common\province_names";
    public const string BOOKMARKS_PATH = @"\common\bookmarks";
    public const string DEFINES_FOLDER_PATH = @"\common\defines";
    public const string DEFINES_FILE_PATH = @"\common\defines.lua";

    // REPLACE FOLDERS
    public const string CULTURES_REP = "common/cultures";
    public const string BOOKMARKS_REP = "common/bookmarks";
    public const string PROV_NAMES_REP = "common/province_names";
    public const string COUNTRIES_REP = "history/countries";
    public const string PROVINCES_REP = "history/provinces";
    public const string LOCALISATION_REP = "localisation";
}
