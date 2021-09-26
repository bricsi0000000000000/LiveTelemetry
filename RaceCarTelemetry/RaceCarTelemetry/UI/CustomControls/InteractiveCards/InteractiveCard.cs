using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class InteractiveCard : Button
    {
        static InteractiveCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InteractiveCard), new FrameworkPropertyMetadata(typeof(InteractiveCard)));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(InteractiveCard), new PropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(InteractiveCard), new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
    }
}
