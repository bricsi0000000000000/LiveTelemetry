using DataModel.Constants;
using System.Collections.Generic;
using System.Windows.Controls;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Settings
{
    public partial class SettingsMenu : UserControl
    {
        private static readonly List<TabItem> menuTabs = new List<TabItem>();

        private readonly UpdateLiveMenu updateLiveMenu;
        private readonly UpdateLiveMenuCharts updateLiveMenuCharts;
        private readonly FinishedReadingGroups finishedReadingGroups;
        private readonly FinishedReadingConfiguration finishedReadingConfiguration;
        private readonly FinishedReadingPageTemplates finishedReadingPageTemplates;

        public LiveSettingsMenu LiveSettingsMenu;
        public GroupSettingsMenu GroupSettingsMenu;

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

            LiveSettingsMenu = new LiveSettingsMenu(updateLiveMenu, finishedReadingConfiguration);
            GroupSettingsMenu = new GroupSettingsMenu(finishedReadingGroups, updateLiveMenuCharts);

            AddSettingsTab(TextManager.LIVE_SETTINGS_MENU, LiveSettingsMenu, selected: true);
            AddSettingsTab(TextManager.GROUP_SETTINGS_MENU, GroupSettingsMenu);
            AddSettingsTab(TextManager.PAGE_TEMPLATES_SETTINGS_MENU, new PageTemplateSettingsMenu(finishedReadingPageTemplates, updateLiveMenuCharts));
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

        public TabItem GetTab(string name)
        {
            return menuTabs.Find(x => x.Header.Equals(name));
        }
    }
}
