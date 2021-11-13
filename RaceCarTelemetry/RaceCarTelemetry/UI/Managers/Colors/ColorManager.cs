using System.Windows;
using System.Windows.Media;

namespace UI.Managers
{
    public static partial class ColorManager
    {
        public static Color Secondary => (Color)Application.Current.Resources["Secondary"];
        public static Color Primary => (Color)Application.Current.Resources["Primary"];

        public static Brush PrimaryButton => (Brush)Application.Current.Resources["PrimaryButton"];

        public static Brush SplitterColor => (Brush)Application.Current.Resources["GridSplitter"];

        public static Color SelectedForeground => (Color)Application.Current.Resources["Primary"];
        public static Color NotSelectedForeground => Secondary;

        public static Brush Error => (Brush)Application.Current.Resources["MaterialDesignSnackbarBackground"];

        public static Color FontColor => Secondary;
        public static Color White => (Color)Application.Current.Resources["White"];
        public static Color Gray => (Color)Application.Current.Resources["Gray"];
    }
}
