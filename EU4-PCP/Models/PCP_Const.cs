#pragma warning disable SYSLIB0001 // Type or member is obsolete

using System.Text;
using System.Windows;
using System.Windows.Media;

namespace EU4_PCP
{
    public static class PCP_Const
    {
        // MISC
        public static readonly string[] NOT_CUL = {
            "graphical_culture", "second_graphical_culture", "male_names", "female_names", "dynasty_names", "primary"};
        public static readonly Style GreenStyle = Application.Current?.FindResource("GreenBackground") as Style;
        public static readonly Style RedStyle = Application.Current?.FindResource("RedBackground") as Style;
        public static readonly Color RedBackground = Color.FromRgb(0xDE, 0x25, 0x25);
        public static readonly Color GreenBackground = Color.FromRgb(0x7C, 0xB6, 0x1A);
        public static readonly Color PurpleBackground = Color.FromRgb(0x6A, 0x33, 0x9E);
        public static readonly string LocIndexer = " Loc Indexer";
        public static readonly int MarkerHeight = 4;

        // SYSTEM CONST
        public static readonly Encoding UTF7 = Encoding.UTF7;
        public static readonly Encoding UTF8 = new UTF8Encoding(false);
        public static readonly string[] SEPARATORS = new string[] { "\n", "\r" };

        // CACHE TO REMOVE
        public static readonly string[] LEGACY_CACHE = new string[] { "BookLoc", "ProvLoc", "NoLocProvCount", "LocFiles" };

        // DATE FORMATS
        public static readonly string[] EUDF = {
            "yyyy.M.dd", "yyyy.MM.dd", "yyyy.M.d", "yyyy.MM.d"
        }; // EU Date Formats. The years 2 - 999 are interpreted falsely, and thus processed in the date parser
        public static readonly string[] DATE_FORMATS = { "yyyy-MMM-dd", "yyyy.MM.dd", "yyyy/MM/dd", "dd/MM/yyyy", "dd.MM.yyyy", "MM/dd/yyyy", "MM.dd.yyyy" };
    }
}
