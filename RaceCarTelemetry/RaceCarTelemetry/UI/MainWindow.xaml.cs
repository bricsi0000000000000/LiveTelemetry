using System.Windows;
using UI.Errors;
using UI.Managers;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = "Race Car Telemetry 5.1";

            ErrorManager.Initialize();

            MenuManager.InitMainMenuTabs(MainMenuTabControl);
        }
    }
}
