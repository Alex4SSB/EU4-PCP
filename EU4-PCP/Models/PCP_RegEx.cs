using System.Text.RegularExpressions;

namespace EU4_PCP
{
    public static class PCP_RegEx
    {
        // Global read-only RegEx patterns
        public static readonly Regex DefinesDateRE = new(@"(?<=START_DATE *= *"")[\d.]+(?="")");
        public static readonly Regex DefinesFileRE = new(@"\w+\.lua$");
        public static readonly Regex ProvNamesRE = new(@"(?<index>\d+)\s*=\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex BookmarkCodeRE = new(@"(?<=^\t*name *= *"")\w+(?="")", RegexOptions.Multiline);
        public static readonly Regex BookmarkDateRE = new(@"(?<=^\t*date *= *)[\d.]+", RegexOptions.Multiline);
        public static readonly Regex BookmarkDefRE = new(@"\t*default *= *yes", RegexOptions.Multiline);
        //public static readonly Regex BookmarkParamsRE = new(@"name\s*=\s*""(?<name>.+?)""[\s\S]*date\s*=\s*(?<date>[\d.]+)(?:[\s\S]*default\s*=\s*(?<default>yes))*");
        public static readonly Regex GameVerRE = new(@"(?<=^Game Version: \w+ ).*(?=\.\w+)", RegexOptions.Multiline);
        public static readonly Regex ProvFileRE = new(@"^[0-9]+(?=.*?$)");
        public static readonly Regex ProvOwnerRE = new(@"^[^{]*?(?<!#[\t ]*)owner\s*=\s*(?<owner>[A-Z][A-Z0-9]{2})");
        public static readonly Regex DateOwnerRE = new(@"(?<!#[\t ]*)owner\s*=\s*(?<value>[A-Z][A-Z0-9]{2})");
        public static readonly Regex DateCulRE = new(@"(?<!#[\t ]*)primary_culture\s*=\s*(?<value>\w+)");
        public static readonly Regex ProvEventRE = new(@"^\s*[\d.]* *= *{[^{]*owner[^{]*}", RegexOptions.Multiline);
        public static readonly Regex CulEventRE = new(@"^\s*[\d.]* *= *{[^{]*primary_culture[^{]*}", RegexOptions.Multiline);
        public static readonly Regex PriCulRE = new(@"(?<=^\s*primary_culture *= *)\w+", RegexOptions.Multiline);
        public static readonly Regex LocProvsRE = new(@"^[^#\r\n]*PROV(?<index>\d+):\d*\s*""(?<name>.+?)""", RegexOptions.Multiline);
        public static readonly Regex LocFileRE = new(@"\w+(english)\.yml$");
        public static readonly Regex MaxProvRE = new(@"(?<=^max_provinces\s*=\s*)\d+", RegexOptions.Multiline);
        public static readonly Regex DefMapRE = new(@"max_provinces.*");
        public static readonly Regex ModReplaceRE = new(@"(?<=^replace_path *= *"")[\w /]+(?="")", RegexOptions.Multiline);
        public static readonly Regex ModNameRE = new(@"(?<=^name *= *"").+?(?="")", RegexOptions.Multiline);
        public static readonly Regex ModVerRE = new(@"(?<=^supported_version *= *"")\d+(\.\d+)*", RegexOptions.Multiline);
        public static readonly Regex ModPathRE = new(@"(?<=^path *= *"").+?(?="")", RegexOptions.Multiline);
        //public static readonly Regex ModParamsRE = new(@"name\s*=\s*""(?<name>.+?)""[\s\S]*supported_version\s*=\s*""(?<gameVer>[\d.]*?)[.*]*""[\s\S]*path\s*=\s*""(?<path>.+?)""", RegexOptions.Multiline);
        public static readonly Regex RnwRE = new(@"(Unused(Land){0,1}\d+|RNW)");
        public static readonly Regex CulClearRE = new(@"(\w+_names\s*=\s*{[\s\S]+?}|\w+\s*=\s*\w+|#.*)", RegexOptions.Multiline);
        public static readonly Regex CulGroupsRE = new(@"\w+[\s\S]*?({\s*})+\s+}", RegexOptions.Multiline);
        public static readonly Regex CulSingleRE = new(@"\w+", RegexOptions.Multiline);
    }

}
