using System.Windows;

namespace EU4_PCP.Converters
{
    public static class AddConst
    {
        public static int GetDefault(this FrameworkElement control) => (int)GetDefault(control.Tag.ToString());

        public static long GetDefault(this string key)
        {
            return long.Parse(Names.GlobalNames[key + "Default"]);
        }

        public static string GetPlaceholder(this FrameworkElement control) => GetPlaceholder(control.Tag.ToString());

        public static string GetPlaceholder(this string key)
        {
            return Names.GlobalNames[key + "Placeholder"];
        }
    }
}
