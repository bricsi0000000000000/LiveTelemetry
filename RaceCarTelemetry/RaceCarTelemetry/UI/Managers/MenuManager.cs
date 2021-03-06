using DataModel.Constants;
using DataModel.Live;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using UI.UserControls.Live;
using UI.UserControls.Settings;

namespace UI.Managers
{
    public static class MenuManager
    {
        private static readonly List<TabItem> menuTabs = new List<TabItem>();

        public delegate void UpdateLiveMenu(LiveSession session);
        public delegate void UpdateLiveMenuCharts();
        public delegate void UpdateLiveMenuRangeSlider();
        public delegate void FinishedReadingGroups();
        public delegate void FinishedReadingConfiguration();
        public delegate void FinishedReadingPageTemplates();
        public delegate void UpdateLiveSettingsCarStatus(TimeSpan? sentTime = null, long? arrivedTime = null);
        public delegate void InitializeGroups();

        public static UpdateLiveMenuRangeSlider updateLiveMenuRangeSlider;

        public static void InitMainMenuTabs(TabControl tabControl)
        {
            LiveMenu liveMenu = new LiveMenu();

            updateLiveMenuRangeSlider = new UpdateLiveMenuRangeSlider(liveMenu.BuildCharts);

            SettingsMenu settingsMenu = new SettingsMenu(new UpdateLiveMenu(liveMenu.Update),
                                                         new UpdateLiveMenuCharts(liveMenu.Update),
                                                         new FinishedReadingGroups(liveMenu.InitializeGroupItems),
                                                         new FinishedReadingConfiguration(liveMenu.InitilaizeHttpClient),
                                                         new FinishedReadingPageTemplates(liveMenu.InitializePageTemplates));

            liveMenu.InitGroups = new InitializeGroups(settingsMenu.GroupSettingsMenu.InitializeGroups);

            AddMenuTab(TextManager.SETTINGS_MENU, settingsMenu, tabControl, selected: true);
            AddMenuTab(TextManager.LIVE_MENU, liveMenu, tabControl);
        }

        private static void AddMenuTab(string header, UserControl content, TabControl tabControl, bool selected = false)
        {
            TabItem tab = new TabItem
            {
                Header = header,
                Content = content,
                Name = $"{content.GetType().ToString().Replace(".", "")}_Tab",
                IsSelected = selected
            };

            menuTabs.Add(tab);
            tabControl.Items.Add(tab);
        }
    }
}
