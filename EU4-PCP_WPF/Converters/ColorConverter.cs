using System.Windows.Media;

namespace EU4_PCP_WPF.Converters
{
    public static class ColorConverter
    {
        public static Color Convert(this System.Drawing.Color sdc) => Color.FromRgb(sdc.R, sdc.G, sdc.B);

        public static System.Drawing.Color Convert(this Color swmc) => System.Drawing.Color.FromArgb(swmc.R, swmc.G, swmc.B);
    }
}
