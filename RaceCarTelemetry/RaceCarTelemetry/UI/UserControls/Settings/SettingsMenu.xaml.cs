using DataModel.Constants;
using System.Collections.Generic;
using System.Windows.Controls;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Settings
{
    public partial class SettingsMenu : UserControl
    {
        public delegate void UpdateGroupsAfterChangeInSettings();

        public LiveSettingsMenu LiveSettingsMenu { get; set; }
        public GroupSettingsMenu GroupSettingsMenu { get; set; }

        private PageTemplateSettingsMenu pageTemplateSettingsMenu;
        private ConfigurationMenu configurationMenu;

        private static readonly List<TabItem> menuTabs = new List<TabItem>();

        private readonly UpdateLiveMenu updateLiveMenu;
        private readonly UpdateLiveMenuCharts updateLiveMenuCharts;
        private readonly FinishedReadingGroups finishedReadingGroups;
        private readonly FinishedReadingConfiguration finishedReadingConfiguration;
        private readonly FinishedReadingPageTemplates finishedReadingPageTemplates;

        public SettingsMenu(UpdateLiveMenu updateLiveMenu,
                            UpdateLiveMenuCharts updateLiveMenuCharts,
                            FinishedReadingGroups finishedReadingGroups,
                            FinishedReadingConfiguration finishedReadingConfiguration,
                            FinishedReadingPageTemplates finishedReadingPageTemplates)
        {
            InitializeComponent();

            this.updateLiveMenu = updateLiveMenu;
            this.updateLiveMenuCharts = updateLiveMenuCharts;
            this.finishedReadingGroups = finishedReadingGroups;
            this.finishedReadingConfiguration = finishedReadingConfiguration;
            this.finishedReadingPageTemplates = finishedReadingPageTemplates;

            InitSettingsTabs();
        }

        public void InitSettingsTabs()
        {
            settingsTabControl.Items.Clear();

            LiveSettingsMenu = new LiveSettingsMenu(updateLiveMenu);
            pageTemplateSettingsMenu = new PageTemplateSettingsMenu(finishedReadingPageTemplates, updateLiveMenuCharts);
            GroupSettingsMenu = new GroupSettingsMenu(finishedReadingGroups, updateLiveMenuCharts, new UpdateGroupsAfterChangeInSettings(pageTemplateSettingsMenu.InitializeGroups));
            configurationMenu = new ConfigurationMenu(finishedReadingConfiguration,
                                                      LiveSettingsMenu.AfterConfigurationIsLoaded,
                                                      new ConfigurationMenu.AfterConfigurationIsUpdated(LiveSettingsMenu.AfterConfigurationIsUpdated),
                                                      finishedReadingConfiguration);

            AddSettingsTab(TextManager.LIVE_SETTINGS_MENU, LiveSettingsMenu, selected: true);
            AddSettingsTab(TextManager.GROUP_SETTINGS_MENU, GroupSettingsMenu);
            AddSettingsTab(TextManager.PAGE_TEMPLATES_SETTINGS_MENU, pageTemplateSettingsMenu);
            AddSettingsTab(TextManager.CONFIGURATION_MENU, configurationMenu);
        }

        private void AddSettingsTab(string header, UserControl content, bool selected = false)
        {
            TabItem tab = new TabItem
            {
                Header = header,
                Content = content,
                Name = $"{header.Replace(" ", "")}_Tab",
                IsSelected = selected
            };

            menuTabs.Add(tab);
            settingsTabControl.Items.Add(tab);
        }
    }
}
