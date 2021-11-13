using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Extensions;
using UI.Managers;

namespace UI.CustomControls
{
    public class LiveSessionCard : Button
    {
        static LiveSessionCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LiveSessionCard), new FrameworkPropertyMetadata(typeof(LiveSessionCard)));
        }

        public static readonly DependencyProperty SessionNameProperty = DependencyProperty.Register(nameof(SessionName), typeof(string), typeof(LiveSessionCard), new PropertyMetadata(string.Empty));

        public string SessionName
        {
            get => (string)GetValue(SessionNameProperty);
            set => SetValue(SessionNameProperty, value);
        }

        public static readonly DependencyProperty SessionDateProperty = DependencyProperty.Register(nameof(SessionDate), typeof(string), typeof(LiveSessionCard), new PropertyMetadata(string.Empty));

        public string SessionDate
        {
            get => (string)GetValue(SessionDateProperty);
            set => SetValue(SessionDateProperty, value);
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(string), typeof(LiveSessionCard), new PropertyMetadata(string.Empty));

        public string Kind
        {
            get { return (string)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }

        public void ChangeIconStatus(bool isLive)
        {
            Kind = isLive ? PackIconKind.AccessPoint.ToString() : PackIconKind.AccessPointOff.ToString();
            StatusColor = isLive ? ColorManager.Primary.ConvertBrush() : ColorManager.Secondary.ConvertBrush();
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(LiveSessionCard), new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(LiveSessionCard), new PropertyMetadata(default(bool)));

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public static readonly DependencyProperty StatusColorProperty = DependencyProperty.Register(nameof(StatusColor), typeof(Brush), typeof(LiveSessionCard), new PropertyMetadata(default(Brush)));

        public Brush StatusColor
        {
            get => (Brush)GetValue(StatusColorProperty);
            set => SetValue(StatusColorProperty, value);
        }
    }
}
