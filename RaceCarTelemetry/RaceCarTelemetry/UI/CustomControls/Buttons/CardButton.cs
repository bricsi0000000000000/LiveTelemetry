using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class CardButton : Button
    {
        static CardButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardButton), new FrameworkPropertyMetadata(typeof(CardButton)));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(CardButton), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
