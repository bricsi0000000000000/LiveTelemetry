using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Extensions;
using UI.Managers;

namespace UI.CustomControls
{
    public class ImageCardButtonWithoutShadow : Button
    {
        static ImageCardButtonWithoutShadow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageCardButtonWithoutShadow), new FrameworkPropertyMetadata(typeof(ImageCardButtonWithoutShadow)));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(ImageCardButtonWithoutShadow), new PropertyMetadata(string.Empty));

        public string Kind
        {
            get => (string)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public void Toggle(bool isToggled)
        {
            Foreground = isToggled ? ColorManager.Primary.ConvertBrush() : ColorManager.Secondary.ConvertBrush();
        }
    }
}
