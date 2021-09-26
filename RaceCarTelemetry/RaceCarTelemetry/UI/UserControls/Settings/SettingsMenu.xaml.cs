﻿using DataModel.Constants;
using System.Collections.Generic;
using System.Windows.Controls;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Settings
{
    public partial class SettingsMenu : UserControl
    {
        private static readonly List<TabItem> menuTabs = new List<TabItem>();

        private readonly UpdateLiveMenu updateLiveMenu;
        private readonly FinishedReadingGroups finishedReadingGroups;
        private readonly FinishedReadingConfiguration finishedReadingConfiguration;

        public LiveSettingsMenu LiveSettingsMenu;

        public SettingsMenu(UpdateLiveMenu updateLiveMenu, FinishedReadingGroups finishedReadingGroups, FinishedReadingConfiguration finishedReadingConfiguration)
        {
            InitializeComponent();

            this.updateLiveMenu = updateLiveMenu;
            this.finishedReadingGroups = finishedReadingGroups;
            this.finishedReadingConfiguration = finishedReadingConfiguration;

            InitSettingsTabs();
        }

        public void InitSettingsTabs()
        {
            settingsTabControl.Items.Clear();

            LiveSettingsMenu = new LiveSettingsMenu(updateLiveMenu, finishedReadingConfiguration);

            AddSettingsTab(TextManager.GROUP_SETTINGS_MENU, new GroupSettingsMenu(finishedReadingGroups));
            AddSettingsTab(TextManager.LIVE_SETTINGS_MENU, LiveSettingsMenu, selected: true);
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
