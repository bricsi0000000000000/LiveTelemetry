using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class ImageCardButton : Button
    {
        static ImageCardButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageCardButton), new FrameworkPropertyMetadata(typeof(ImageCardButton)));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(ImageCardButton), new PropertyMetadata(string.Empty));

        public string Kind
        {
            get { return (string)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
    }
}
