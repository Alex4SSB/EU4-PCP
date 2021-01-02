using System.Text.RegularExpressions;

namespace EU4_PCP_WPF
{
    public static class PCP_RegEx
	{
		// Global read-only RegEx patterns
		public static readonly Regex DefinesDateRE = new Regex(@"(?<=START_DATE *= *"")[\d.]+(?="")");
		public static readonly Regex DefinesFileRE = new Regex(@"\w+\.lua$");
		public static readonly Regex LocNameRE = new Regex(@"(?<="").+(?="")");
		public static readonly Regex BookLocCodeRE = new Regex(@"\w+(?=:)");
		public static readonly Regex BookmarkCodeRE = new Regex(@"(?<=^\t*name *= *"")\w+(?="")", RegexOptions.Multiline);
		public static readonly Regex BookmarkDateRE = new Regex(@"(?<=^\t*date *= *)[\d.]+", RegexOptions.Multiline);
		public static readonly Regex BookmarkDefRE = new Regex(@"\t*default *= *yes", RegexOptions.Multiline);
		public static readonly Regex GameVerRE = new Regex(@"(?<=^Game Version: \w+ ).*(?=\.\w+)", RegexOptions.Multiline);
		public static readonly Regex ProvFileRE = new Regex(@"^[0-9]+(?=.*?$)");
		public static readonly Regex ProvOwnerRE = new Regex(@"(?<=^owner *= *)[A-Z]+", RegexOptions.Multiline);
		public static readonly Regex DateOwnerRE = new Regex(@"(?<=owner *= *)[A-Z][A-Z0-9]{2}");
		public static readonly Regex DateCulRE = new Regex(@"(?<=primary_culture *= *)\w+");
		public static readonly Regex ProvEventRE = new Regex(@"^\s*[\d.]* *= *{[^{]*owner[^{]*}", RegexOptions.Multiline);
		public static readonly Regex CulEventRE = new Regex(@"^\s*[\d.]* *= *{[^{]*primary_culture[^{]*}", RegexOptions.Multiline);
		public static readonly Regex PriCulRE = new Regex(@"(?<=^\s*primary_culture *= *)\w+", RegexOptions.Multiline);
		public static readonly Regex LocProvRE = new Regex(@"(?<=^[ \t]*PROV)([0-9])+(:.*)", RegexOptions.Multiline);
		public static readonly Regex LocFileRE = new Regex(@"\w+(english)\.yml$");
		public static readonly Regex MaxProvRE = new Regex(@"(?<=^max_provinces *= *)\d+", RegexOptions.Multiline);
		public static readonly Regex DefMapRE = new Regex(@"max_provinces.*");
		public static readonly Regex ModFileRE = new Regex(@"\w+\.mod$");
		public static readonly Regex ModNameRE = new Regex(@"(?<=^name *= *"").+?(?="")", RegexOptions.Multiline);
		public static readonly Regex ModReplaceRE = new Regex(@"(?<=^replace_path *= *"")[\w /]+(?="")", RegexOptions.Multiline);
		public static readonly Regex ModVerRE = new Regex(@"(?<=^supported_version *= *"")\d+(\.\d+)*", RegexOptions.Multiline);
		public static readonly Regex ModPathRE = new Regex(@"(?<=^path *= *"")[\w ()/:]+(?="")", RegexOptions.Multiline);
		public static readonly Regex RnwRE = new Regex(@"(Unused(Land){0,1}\d+|RNW)");
		public static readonly Regex NewLineRE = new Regex("[\r\n]+");
		public static readonly Regex AsciiRE = new Regex("[\x00-\x7F]");
	}

}
