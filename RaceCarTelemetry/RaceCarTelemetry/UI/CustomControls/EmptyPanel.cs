using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class EmptyPanel : Control
    {
        static EmptyPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyPanel), new FrameworkPropertyMetadata(typeof(EmptyPanel)));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(EmptyPanel), new PropertyMetadata(string.Empty));

        public string Kind
        {
            get { return (string)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
    }
}
