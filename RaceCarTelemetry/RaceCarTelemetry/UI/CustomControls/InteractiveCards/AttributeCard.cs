using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.CustomControls
{
    public class AttributeCard : Button
    {
        static AttributeCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttributeCard), new FrameworkPropertyMetadata(typeof(AttributeCard)));
        }

        public static readonly DependencyProperty AttributeNameProperty = DependencyProperty.Register(nameof(AttributeName), typeof(string), typeof(AttributeCard), new PropertyMetadata(string.Empty));

        public string AttributeName
        {
            get => (string)GetValue(AttributeNameProperty);
            set => SetValue(AttributeNameProperty, value);
        }

        public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register(nameof(LineWidth), typeof(string), typeof(AttributeCard), new PropertyMetadata(string.Empty));

        public string LineWidth
        {
            get => (string)GetValue(LineWidthProperty);
            set => SetValue(LineWidthProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(AttributeCard), new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty AttributeColorProperty = DependencyProperty.Register(nameof(AttributeColor), typeof(Brush), typeof(AttributeCard), new PropertyMetadata(default(Brush)));

        public Brush AttributeColor
        {
            get => (Brush)GetValue(AttributeColorProperty);
            set => SetValue(AttributeColorProperty, value);
        }

        public static readonly DependencyProperty LabelColorProperty = DependencyProperty.Register(nameof(LabelColor), typeof(Brush), typeof(AttributeCard), new PropertyMetadata(default(Brush)));

        public Brush LabelColor
        {
            get => (Brush)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }
    }
}
