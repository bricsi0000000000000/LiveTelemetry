using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class PopUpDeleteCardButton : Button
    {
        static PopUpDeleteCardButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopUpDeleteCardButton), new FrameworkPropertyMetadata(typeof(PopUpDeleteCardButton)));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(PopUpDeleteCardButton), new PropertyMetadata(string.Empty));

        public string Kind
        {
            get { return (string)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
    }
}
