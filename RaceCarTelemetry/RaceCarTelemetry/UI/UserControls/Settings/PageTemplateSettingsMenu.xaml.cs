using DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UI.Errors;
using UI.Managers;
using UI.ValidationRules;

namespace UI.UserControls.Settings
{
    public partial class PageTemplateSettingsMenu : UserControl
    {
        private readonly FieldsViewModel fieldsViewModel = new FieldsViewModel();

        public delegate void ChangeActivePageTemplate(int selectedTemplateId);

        private int activeTemplateId;

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
            }
        }

        private void GroupCheckBox_CheckedClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string content = checkBox.Content.ToString();

            if ((bool)checkBox.IsChecked)
            {
                PageTemplateManager.GetPageTemplate(activeTemplateId).AddGroupName(content);
            }
            else
            {
                PageTemplateManager.GetPageTemplate(activeTemplateId).remove(content);

                selectedGroups.Remove(content);
            }
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

            //UpdateGroups(activeGroupId);
        }

        private void AddPageTemplate(PageTemplate template, bool withUpdate = true)
        {
            //GroupSettingsItem groupSettingsItem = new GroupSettingsItem(template, new ChangeActiveGroup(UpdateGroups));
            //GroupsStackPanel.Children.Add(groupSettingsItem);

            //if (withUpdate)
            //{
            //    activeGroupId = template.Id;
            //    UpdateGroups(activeGroupId);
            //}

            //updateLiveMenuCharts();
        }

        private void RemoveGroup(int groupId)
        {
            //UIElement deletableItem = new UIElement();
            //foreach (GroupSettingsItem item in GroupsStackPanel.Children)
            //{
            //    if (item.Id == groupId)
            //    {
            //        deletableItem = item;
            //    }
            //}

            //GroupsStackPanel.Children.Remove(deletableItem);

            //if (GroupManager.Groups.Any())
            //{
            //    activeGroupId = GroupManager.Groups.First().Id;
            //}
            //else
            //{
            //    activeGroupId = -1;
            //}

            //UpdateGroups(activeGroupId);

            //updateLiveMenuCharts();
        }

        private void ChangeAddGroupPopUpState(bool open)
        {
            ChangePopUpBackgroundState(open);
            AddPageTemplateGrid.Visibility = open ? Visibility.Visible : Visibility.Hidden;

            if (open)
            {
                AddPageTemplateNameTextBox.Focus();
            }

            AddPageTemplateNameTextBox.Text = string.Empty;
        }

        private void ChangePopUpBackgroundState(bool open)
        {
            PopUpBackground.Visibility = open ? Visibility.Visible : Visibility.Hidden;
        }

        private void ChangeSelectedPageNameCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSensorNameCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSensorNameCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddGroupPopUpState(open: true);
        }

        private void DeletePageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPageTemplatePopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CancelAddPageTemplateCardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
