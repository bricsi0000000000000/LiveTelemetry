namespace UI.Extensions
{
    public static class Extension
{
        public static System.Windows.Media.Color ConvertColor(this string colorText)
        {
            return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(colorText);
        }

        public static System.Drawing.Color ConvertToChartColor(this string colorText)
        {
            System.Windows.Media.Color color = colorText.ConvertColor();
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Color ConvertToDrawingColor(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ConvertToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.SolidColorBrush ConvertBrush(this string colorText)
        {
            return new System.Windows.Media.SolidColorBrush(colorText.ConvertColor());
        }

        public static System.Windows.Media.SolidColorBrush ConvertBrush(this System.Windows.Media.Color color)
        {
            return new System.Windows.Media.SolidColorBrush(color);
        }
    }
}
