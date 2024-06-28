namespace EU4_PCP;

public static partial class PCP_RegEx
{
    [GeneratedRegex("START_DATE\\s*=\\s*\"(?<startDate>[\\d.]+)\"")]
    public static partial Regex RE_DEFINES_DATE();

    [GeneratedRegex("(?<index>\\d+)\\s*=\\s*\"(?<name>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_PROV_NAMES();

    [GeneratedRegex("^[^#\\r\\n]*name\\s*=\\s*\"(?<name>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_BOOKMARK_CODE();

    [GeneratedRegex("^[^#\\r\\n]*date\\s*=\\s*(?<date>[\\d.]+)", RegexOptions.Multiline)]
    public static partial Regex RE_BOOKMARK_DATE();

    [GeneratedRegex("^[^#\\r\\n]*default\\s*=\\s*(?<default>yes)", RegexOptions.Multiline)]
    public static partial Regex RE_BOOKMARK_DEFINITION();

    [GeneratedRegex("Game Version:\\s*\\w+\\s*(?<version>\\w*\\d+(?:\\.\\d+){0,2})[\\.\\d\\s]*(?<name>\\w+)", RegexOptions.Multiline)]
    public static partial Regex RE_GAME_VERSION();

    [GeneratedRegex("^(?<index>\\d+)[\\s-]*")]
    public static partial Regex RE_PROVINCE_FILE();

    [GeneratedRegex("^[\\t ]*(?<eventDate>[\\d.]+)[\\t ]*=\\s*{[^{]*(?<!#[\\t ]*)owner[\\t ]*=[\\t ]*(?<result>[A-Z][A-Z0-9]{2})[^{]*}", RegexOptions.Multiline)]
    public static partial Regex RE_PROVINCE_EVENT();

    [GeneratedRegex("^[\\t ]*(?<eventDate>[\\d.]+)[\\t ]*=\\s*{[^{]*(?<!#[\\t ]*)primary_culture[\\t ]*=[\\t ]*(?<result>\\w+)[^{]*}", RegexOptions.Multiline)]
    public static partial Regex RE_CULTURE_EVENT();

    [GeneratedRegex("^[^{]*?(?<!#[\\t ]*)owner[\\t ]*=[\\t ]*(?<result>[A-Z][A-Z0-9]{2})")]
    public static partial Regex RE_PROVINCE_OWNER();

    [GeneratedRegex("^[^{]*?(?<!#[\\t ]*)primary_culture[\\t ]*=[\\t ]*(?<result>\\w+)")]
    public static partial Regex RE_PRIMARY_CULTURE();

    [GeneratedRegex("^[^#\\r\\n]*PROV(?<index>\\d+):\\d*\\s*\"(?<name>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_LOCALISATION_PROVINCES();

    [GeneratedRegex("\\w+english\\.yml$")]
    public static partial Regex RE_LOCALISATION_FILE();

    [GeneratedRegex("^[^#\\r\\n]*max_provinces[\\t ]*=[\\t ]*(?<value>\\d+)", RegexOptions.Multiline)]
    public static partial Regex RE_MAX_PROVINCES();

    [GeneratedRegex("max_provinces.*")]
    public static partial Regex RE_DEF_MAP();

    [GeneratedRegex("^[^#\\r\\n]*replace_path\\s*=\\s*\"(?<replace>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_MOD_REPLACE();

    [GeneratedRegex("^[^#\\r\\n]*name\\s*=\\s*\"(?<name>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_MOD_NAME();

    [GeneratedRegex("^[^#\\r\\n]*supported_version\\s*=\\s*\"v*(?<gameVer>\\d+(?:\\.\\d+)*?)(?:(?:\\.\\*)|\\.|\\*)*\"", RegexOptions.Multiline)]
    public static partial Regex RE_MOD_VERSION();

    [GeneratedRegex("^[^#\\r\\n_]*path\\s*=\\s*\"(?<path>.+?)\"", RegexOptions.Multiline)]
    public static partial Regex RE_MOD_PATH();

    [GeneratedRegex("Unused(?:Land)?\\d+|RNW")]
    public static partial Regex RE_RNW();

    [GeneratedRegex("\\w+_names\\s*=\\s*{[\\s\\S]+?}|\\w+\\s*=\\s*\\w+|#.*", RegexOptions.Multiline)]
    public static partial Regex RE_CULTURE_CLEAR();

    [GeneratedRegex("(?<group>\\w+)(?<cultures>[\\s\\S]*?(?:{\\s*})+\\s+})")]
    public static partial Regex RE_CULTURE_GROUPS();

    [GeneratedRegex("^[^#\\r\\n\\w]*?\\s*(?<name>\\w+)\\s*=", RegexOptions.Multiline)]
    public static partial Regex RE_CULTURE_SINGLE();

    [GeneratedRegex("(?<name>.*)\\..+$")]
    public static partial Regex RE_REMOVE_FILE_EXTENSION();

    [GeneratedRegex("^(?<tag>[A-Z]{3}).*")]
    public static partial Regex RE_COUNTRY_TAG();
}
