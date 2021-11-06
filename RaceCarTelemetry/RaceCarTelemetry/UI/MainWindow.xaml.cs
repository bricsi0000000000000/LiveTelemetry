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

            Title = "Race car telemetry 5.0";

            ErrorManager.Initialize();
            MenuManager.InitMainMenuTabs(MainMenuTabControl);
        }
    }
}
