using DataModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UI.Errors;
using UI.Managers;
using UI.UserControls.Charts;
using UI.UserControls.PageTemplates;
using UI.ValidationRules;
using UI.Extensions;

namespace UI.UserControls.Settings
{
    public partial class PageTemplateSettingsMenu : UserControl
    {
        private readonly FieldsViewModel fieldsViewModel = new FieldsViewModel();

        public delegate void ChangeActivePageTemplate(int selectedTemplateId);

        private int activeTemplateId;

        private int chartIndex = 0;
        private List<PageTemplateChartSettingsItem> charts = new List<PageTemplateChartSettingsItem>();

        private bool canUpdateCharts = false;

        public PageTemplateSettingsMenu()
        {
            InitializeComponent();

            DataContext = fieldsViewModel;

            PageTemplateManager.LoadPageTemplates(out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }

            if (PageTemplateManager.PageTemplates.Any())
            {
                activeTemplateId = PageTemplateManager.PageTemplates.First().Id;
                InitializePageTemplates();
                InitializeSensorNames();
            }
            else
            {
                activeTemplateId = -1;
            }


            if (GroupManager.Groups.Any())
            {
                foreach (Group group in GroupManager.Groups)
                {
                    CheckBox checkBox = new CheckBox()
                    {
                        Content = group.Name
                    };

                    checkBox.Checked += GroupCheckBox_CheckedClick;
                    checkBox.Unchecked += GroupCheckBox_CheckedClick;

                    GroupsStackPanel.Children.Add(checkBox);
                }

                UpdateGroups();
            }

            canUpdateCharts = true;

            InitializeCharts();
        }

        private void InitializeCharts()
        {
            if (activeTemplateId != -1)
            {
                foreach (PageTemplateChart chart in PageTemplateManager.GetPageTemplate(activeTemplateId).Charts)
                {
                    charts.Add(new PageTemplateChartSettingsItem(chart.Name, chart.Index, MoveUp, MoveDown));
                }

                if (charts.Any())
                {
                    chartIndex = charts.Last().Index;
                }
            }

            RearrangeCharts();
        }

        private void GroupCheckBox_CheckedClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string groupName = checkBox.Content.ToString();
            Group group = GroupManager.GetGroup(groupName);

            if ((bool)checkBox.IsChecked)
            {
                PageTemplateManager.GetPageTemplate(activeTemplateId).AddGroup(group.Name);
            }
            else
            {
                PageTemplateManager.GetPageTemplate(activeTemplateId).RemoveGroup(group.Name);
            }

            Save();

            UpdateGroups();
        }

        private void UpdateGroups()
        {
            PageTemplate activeTemplate = PageTemplateManager.GetPageTemplate(activeTemplateId);
            foreach (object item in GroupsStackPanel.Children)
            {
                if (item is CheckBox checkBox)
                {
                    checkBox.IsChecked = activeTemplate.IsGroupExists(checkBox.Content.ToString());
                }
            }

            UpdateCharts();
        }

        private void UpdateCharts()
        {
            if (canUpdateCharts)
            {
                charts.Clear();

                foreach (string name in PageTemplateManager.GetPageTemplate(activeTemplateId).GroupNames)
                {
                    PageTemplateChartSettingsItem chart = new PageTemplateChartSettingsItem(name, chartIndex++, MoveUp, MoveDown);
                    charts.Add(chart);
                }

                foreach (string name in PageTemplateManager.GetPageTemplate(activeTemplateId).SensorNames)
                {
                    PageTemplateChartSettingsItem chart = new PageTemplateChartSettingsItem(name, chartIndex++, MoveUp, MoveDown);
                    charts.Add(chart);
                }

                RearrangeCharts();
            }
        }

        private void MoveUp(string name, int oldIndex)
        {
            if (oldIndex > charts.First().Index)
            {
                int indexA = charts.FindIndex(x => x.Index == oldIndex);
                int indexB = indexA - 1;

                charts.Swap(indexA, indexB);
            }

            RearrangeCharts();
        }

        private void MoveDown(string name, int oldIndex)
        {
            if (oldIndex < charts.Last().Index)
            {
                int indexA = charts.FindIndex(x => x.Index == oldIndex);
                int indexB = indexA + 1;

                charts.Swap(indexA, indexB);
            }

            RearrangeCharts();
        }

        private void RearrangeCharts()
        {
            ChartsStackPanel.Children.Clear();
            PageTemplate activeTemplate = PageTemplateManager.GetPageTemplate(activeTemplateId);
            activeTemplate.Charts.Clear();

            chartIndex = 0;
            foreach (PageTemplateChartSettingsItem chart in charts)
            {
                chart.Index = chartIndex++;
                ChartsStackPanel.Children.Add(chart);
                activeTemplate.Charts.Add(new PageTemplateChart { Name = chart.ChartName, Index = chart.Index });
            }

            Save();
        }

        private void InitializePageTemplates()
        {
            PageTemplatesStackPanel.Children.Clear();

            foreach (PageTemplate template in PageTemplateManager.PageTemplates)
            {
                AddPageTemplate(template, withUpdate: false);
            }

            PageTemplate activeTemplate = PageTemplateManager.GetPageTemplate(activeTemplateId);
            if (activeTemplate != null)
            {
                SelectedPageTemplateNameTextBox.Text = activeTemplate.Name;

                fieldsViewModel.PageTemplateName = activeTemplate.Name;
            }

            UpdatePageTemplates(activeTemplateId);
        }

        private void AddPageTemplate(PageTemplate template, bool withUpdate = true)
        {
            PageTemplateSettingsItem templateSettingsItem = new PageTemplateSettingsItem(template, new ChangeActivePageTemplate(UpdatePageTemplates));
            PageTemplatesStackPanel.Children.Add(templateSettingsItem);

            if (withUpdate)
            {
                activeTemplateId = template.Id;
                UpdatePageTemplates(activeTemplateId);
            }

            // updateLiveMenuCharts();
        }

        private void UpdatePageTemplates(int selectedTemplateId)
        {
            activeTemplateId = selectedTemplateId;
            foreach (PageTemplateSettingsItem item in PageTemplatesStackPanel.Children)
            {
                item.ChangeColorMode(item.Id == activeTemplateId);
            }

            PageTemplate activePageTemplate = PageTemplateManager.GetPageTemplate(activeTemplateId);
            if (activePageTemplate != null)
            {
                SelectedPageTemplateNameTextBox.Text = activePageTemplate.Name;
            }

            UpdateGroups();
            InitializeSensorNames();
        }


        private void ChangeAddPageTemplatePopUpState(bool open)
        {
            ChangePopUpBackgroundState(open);
            AddPageTemplateGrid.Visibility = open ? Visibility.Visible : Visibility.Hidden;

            if (open)
            {
                AddPageTemplateNameTextBox.Focus();
            }

            AddPageTemplateNameTextBox.Text = string.Empty;
        }

        private void ChangeAddSensorPopUpState(bool open)
        {
            ChangePopUpBackgroundState(open);
            AddSensorGrid.Visibility = open ? Visibility.Visible : Visibility.Hidden;

            if (open)
            {
                AddSensorNameTextBox.Focus();
            }

            AddSensorNameTextBox.Text = string.Empty;
        }

        private void ChangePopUpBackgroundState(bool open)
        {
            PopUpBackground.Visibility = open ? Visibility.Visible : Visibility.Hidden;
        }

        private void ChangeSelectedPageNameCardButton_Click(object sender, RoutedEventArgs e)
        {
            PageTemplate activeTemplate = PageTemplateManager.GetPageTemplate(activeTemplateId);

            string name = SelectedPageTemplateNameTextBox.Text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                SelectedPageTemplateNameTextBox.Text = activeTemplate.Name;
                ErrorManager.ShowMessage("Name can not be empty", MessageSnackbar, MessageType.Error);
            }
            else
            {
                activeTemplate.Name = name;
                Save();
                InitializePageTemplates();
            }
        }

        private void AddSensorNameCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddSensorPopUpState(open: true);
        }

        private void AddPageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddPageTemplatePopUpState(open: true);
        }

        private void DeletePageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {
            PageTemplateManager.RemovePageTemplate(activeTemplateId, out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }

            if (PageTemplateManager.PageTemplates.Any())
            {
                activeTemplateId = PageTemplateManager.PageTemplates.First().Id;
            }
            else
            {
                activeTemplateId = -1;
            }

            InitializePageTemplates();
        }

        private void AddPageTemplatePopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
            string name = AddPageTemplateNameTextBox.Text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                ErrorManager.ShowMessage("Name can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            PageTemplate template = new PageTemplate(PageTemplateManager.LastTemplateId++, name);
            ChangeAddPageTemplatePopUpState(open: false);
            PageTemplateManager.AddPageTemplate(template, out string errorMessage);
            if (string.IsNullOrEmpty(errorMessage))
            {
                AddPageTemplate(template);
            }
            else
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void CancelAddPageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddPageTemplateNameTextBox.Text = "";
            ChangeAddPageTemplatePopUpState(open: false);
        }

        private void Save()
        {
            PageTemplateManager.SavePageTemplates(out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void AddSensorPopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
            string name = AddSensorNameTextBox.Text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                ErrorManager.ShowMessage("Name can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            ChangeAddSensorPopUpState(open: false);

            PageTemplateManager.GetPageTemplate(activeTemplateId).AddSensorName(name, out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }

            Save();
            InitializeSensorNames();
        }

        private void CancelAddSensorCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddSensorNameTextBox.Text = "";
            ChangeAddSensorPopUpState(open: false);
        }

        private void InitializeSensorNames()
        {
            SensorsStackPanel.Children.Clear();

            if (activeTemplateId != -1)
            {
                foreach (string sensorName in PageTemplateManager.GetPageTemplate(activeTemplateId).SensorNames)
                {
                    SensorsStackPanel.Children.Add(new PageTemplateSensorNameSettingsItem(sensorName, RemoveSensorName));
                }
            }

            UpdateCharts();
        }

        private void RemoveSensorName(string name)
        {
            if (activeTemplateId != -1)
            {
                PageTemplateManager.GetPageTemplate(activeTemplateId).RemoveSensorName(name, out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
                }

                Save();
                InitializeSensorNames();
            }
        }
    }
}
