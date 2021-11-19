using System.Text.RegularExpressions;

namespace EU4_PCP
{
    public static class PCP_RegEx
    {
        // Global read-only RegEx patterns
        public static readonly Regex DefinesDateRE = new(@"START_DATE\s*=\s*""(?<startDate>[\d.]+)""");
        public static readonly Regex ProvNamesRE = new(@"(?<index>\d+)\s*=\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex BookmarkCodeRE = new(@"^[^#\r\n]*name\s*=\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex BookmarkDateRE = new(@"^[^#\r\n]*date\s*=\s*(?<date>[\d.]+)", RegexOptions.Multiline);
        public static readonly Regex BookmarkDefRE = new(@"^[^#\r\n]*default\s*=\s*(?<default>yes)", RegexOptions.Multiline);
        public static readonly Regex GameVerRE = new(@"Game Version:\s*\w+\s*(?<version>\w*\d+(?:\.\d+){0,2})[\.\d\s]*(?<name>\w+)", RegexOptions.Multiline);
        public static readonly Regex ProvFileRE = new(@"^(?<index>\d+)[\s-]*");
        public static readonly Regex ProvEventRE = new(@"^[\t ]*(?<eventDate>[\d.]+)[\t ]*=\s*{[^{]*(?<!#[\t ]*)owner[\t ]*=[\t ]*(?<result>[A-Z][A-Z0-9]{2})[^{]*}", RegexOptions.Multiline);
        public static readonly Regex CulEventRE = new(@"^[\t ]*(?<eventDate>[\d.]+)[\t ]*=\s*{[^{]*(?<!#[\t ]*)primary_culture[\t ]*=[\t ]*(?<result>\w+)[^{]*}", RegexOptions.Multiline);
        public static readonly Regex ProvOwnerRE = new(@"^[^{]*?(?<!#[\t ]*)owner[\t ]*=[\t ]*(?<result>[A-Z][A-Z0-9]{2})");
        public static readonly Regex PriCulRE = new(@"^[^{]*?(?<!#[\t ]*)primary_culture[\t ]*=[\t ]*(?<result>\w+)");
        public static readonly Regex LocProvsRE = new(@"^[^#\r\n]*PROV(?<index>\d+):\d*\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex LocFileRE = new(@"\w+english\.yml$");
        public static readonly Regex MaxProvRE = new(@"^[^#\r\n]*max_provinces[\t ]*=[\t ]*(?<value>\d+)", RegexOptions.Multiline);
        public static readonly Regex DefMapRE = new(@"max_provinces.*");
        public static readonly Regex ModReplaceRE = new(@"^[^#\r\n]*replace_path\s*=\s*""(?<replace>.+?)""", RegexOptions.Multiline);
        public static readonly Regex ModNameRE = new(@"^[^#\r\n]*name\s*=\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex ModVerRE = new(@"^[^#\r\n]*supported_version\s*=\s*""(?<gameVer>\d+(?:\.\d+)*?)(?:(?:\.\*)|\.|\*)*""", RegexOptions.Multiline);
        public static readonly Regex ModPathRE = new(@"^[^#\r\n_]*path\s*=\s*""(?<path>.+?)""", RegexOptions.Multiline);
        public static readonly Regex RnwRE = new(@"Unused(?:Land)?\d+|RNW");
        public static readonly Regex CulClearRE = new(@"\w+_names\s*=\s*{[\s\S]+?}|\w+\s*=\s*\w+|#.*", RegexOptions.Multiline);
        public static readonly Regex CulGroupsRE = new(@"(?<group>\w+)(?<cultures>[\s\S]*?(?:{\s*})+\s+})");
        public static readonly Regex CulSingleRE = new(@"^[^#\r\n\w]*?\s*(?<name>\w+)\s*=", RegexOptions.Multiline);
        public static readonly Regex RemoveFileExtRE = new(@"(?<name>.*)\..+$");
    }

}
