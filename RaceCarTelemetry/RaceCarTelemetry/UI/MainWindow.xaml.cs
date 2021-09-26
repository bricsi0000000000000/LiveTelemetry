using System.Windows;
using UI.Managers;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = "Race car telemetry 4.0";

            MenuManager.InitMainMenuTabs(MainMenuTabControl);
        }
    }
}
