using BusinessLogic;
using System;
using System.Windows.Controls;
using UI.Errors;
using UI.Managers;
using UI.ValidationRules;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Settings
{
    public partial class ConfigurationMenu : UserControl
    {
        public delegate void AfterConfigurationIsUpdated();

        private readonly FieldsViewModel fieldsViewModel;
        private readonly ConfigurationBusinessLogic configurationBusinessLogic;
        private readonly AfterConfigurationIsUpdated afterConfigurationIsUpdated;
        private readonly FinishedReadingConfiguration finishedReadingConfiguration;

        public ConfigurationMenu(FinishedReadingConfiguration AfterConfigurationIsLoaded_LiveMenu,
                                 Action AfterConfigurationIsLoaded_LiveSettings,
                                 AfterConfigurationIsUpdated afterConfigurationIsUpdated_LiveSettings,
                                 FinishedReadingConfiguration afterConfigurationIsUpdated_LiveMenu)
        {
            InitializeComponent();

            configurationBusinessLogic = new ConfigurationBusinessLogic();
            fieldsViewModel = new FieldsViewModel();

            afterConfigurationIsUpdated = afterConfigurationIsUpdated_LiveSettings;
            finishedReadingConfiguration = afterConfigurationIsUpdated_LiveMenu;

            DataContext = fieldsViewModel;

            LoadLiveConfiguration();

            if (ConfigurationManager.Configuration != null)
            {
                AfterConfigurationIsLoaded_LiveMenu();
                AfterConfigurationIsLoaded_LiveSettings();

                fieldsViewModel.IpAddress = ConfigurationManager.Configuration.Url;
                fieldsViewModel.Port = ConfigurationManager.Configuration.Port;
                IsHttpsToggleButton.IsChecked = ConfigurationManager.Configuration.IsHttps;
                fieldsViewModel.TimeOut = ConfigurationManager.Configuration.TimeOut;
                fieldsViewModel.WaitBetweenCollectData = ConfigurationManager.Configuration.WaitBetweenCollectData;
            }
        }

        private void LoadLiveConfiguration()
        {
            ConfigurationManager.Configuration = configurationBusinessLogic.LoadLiveConfiguration(out string errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error, className: nameof(LiveSettingsMenu), exceptionMessage: errorMessage);
            }
        }

        private void SaveImageCardButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ConfigurationManager.Configuration.Url = fieldsViewModel.IpAddress;
            ConfigurationManager.Configuration.Port = (int)fieldsViewModel.Port;
            ConfigurationManager.Configuration.IsHttps = (bool)IsHttpsToggleButton.IsChecked;
            ConfigurationManager.Configuration.TimeOut = (int)fieldsViewModel.TimeOut;
            ConfigurationManager.Configuration.WaitBetweenCollectData = (int)fieldsViewModel.WaitBetweenCollectData;

            ConfigurationManager.Save(configurationBusinessLogic, out string errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error, className: nameof(ConfigurationMenu));
            }
            else
            {
                ErrorManager.ShowMessage("Saved successfully", MessageSnackbar, MessageType.Info);

                afterConfigurationIsUpdated();
                finishedReadingConfiguration();
            }
        }
    }
}
