using System.Windows.Controls;
using System.Windows;
using UI.ValidationRules;
using UI.Managers;
using UI.Errors;
using System.Linq;
using DataModel;
using UI.UserControls.Groups;
using System.Collections.Generic;
using UI.Extensions;
using System.Windows.Input;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Settings
{
    public partial class GroupSettingsMenu : UserControl
    {
        private readonly FieldsViewModel fieldsViewModel = new FieldsViewModel();

        private int activeGroupId;
        private int activeAttributeId;

        public delegate void ChangeActiveGroup(int selectedGroupId);
        public delegate void ChangeActiveAttribute(int selectedAttributeId);

        private UpdateLiveMenuCharts updateLiveMenuCharts;

        public GroupSettingsMenu(FinishedReadingGroups finishedReadingGroups, UpdateLiveMenuCharts updateLiveMenuCharts)
        {
            InitializeComponent();

            this.updateLiveMenuCharts = updateLiveMenuCharts;

            fieldsViewModel.AddGroupName =
            fieldsViewModel.AddAttributeName =
            fieldsViewModel.AttributeName = string.Empty;
            fieldsViewModel.AddAttributeLineWidth = 1;
            DataContext = fieldsViewModel;

            GroupManager.LoadGroups(out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }

            finishedReadingGroups();

            if (GroupManager.Groups.Any())
            {
                activeGroupId = GroupManager.Groups.First().Id;
                InitializeGroups();
            }
            else
            {
                activeGroupId = -1;
            }
        }

        public void InitializeGroups()
        {
            GroupsStackPanel.Children.Clear();

            foreach (Group group in GroupManager.Groups)
            {
                AddGroup(group, withUpdate: false);
            }

            Group activeGroup = GroupManager.GetGroup(activeGroupId);
            if (activeGroup != null)
            {
                SelectedGroupNameTextBox.Text = activeGroup.Name;

                if (activeGroup.Attributes.Any())
                {
                    activeAttributeId = activeGroup.Attributes.First().Id;
                }
                else
                {
                    activeAttributeId = -1;
                }

                fieldsViewModel.GroupName = activeGroup.Name;

                UpdateGroups(activeGroupId);
            }
        }

        private void AddGroup(Group group, bool withUpdate = true)
        {
            GroupSettingsItem groupSettingsItem = new GroupSettingsItem(group, new ChangeActiveGroup(UpdateGroups));
            GroupsStackPanel.Children.Add(groupSettingsItem);

            if (withUpdate)
            {
                activeGroupId = group.Id;
                UpdateGroups(activeGroupId);
            }

            updateLiveMenuCharts();
        }

        private void RemoveGroup(int groupId)
        {
            UIElement deletableItem = new UIElement();
            foreach (GroupSettingsItem item in GroupsStackPanel.Children)
            {
                if (item.Id == groupId)
                {
                    deletableItem = item;
                }
            }

            GroupsStackPanel.Children.Remove(deletableItem);

            if (GroupManager.Groups.Any())
            {
                activeGroupId = GroupManager.Groups.First().Id;
            }
            else
            {
                activeGroupId = -1;
            }

            UpdateGroups(activeGroupId);

            updateLiveMenuCharts();
        }

        private void UpdateGroups(int selectedGroupId)
        {
            activeGroupId = selectedGroupId;
            foreach (GroupSettingsItem item in GroupsStackPanel.Children)
            {
                item.ChangeColorMode(item.Id == activeGroupId);
            }

            Group activeGroup = GroupManager.GetGroup(activeGroupId);
            SelectedGroupNameTextBox.Text = activeGroup.Name;

            InitAttributes();
        }

        private void AddGroupCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddGroupPopUpState(open: true);
        }

        private void ChangeAddGroupPopUpState(bool open)
        {
            ChangePopUpBackgroundState(open);
            AddGroupGrid.Visibility = open ? Visibility.Visible : Visibility.Hidden;

            if (open)
            {
                AddGroupNameTextBox.Focus();
            }

            AddGroupNameTextBox.Text = string.Empty;
        }

        private void ChangePopUpBackgroundState(bool open)
        {
            PopUpBackground.Visibility = open ? Visibility.Visible : Visibility.Hidden;
        }

        private void DeleteGroupCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeGroupId != -1)
            {
                GroupManager.RemoveGroup(activeGroupId, out string errorMessage);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    RemoveGroup(activeGroupId);
                    ChangeAddGroupPopUpState(open: false);
                }
                else
                {
                    ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
                }
            }
        }

        private void AddGroupPopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AddGroupNameTextBox.Text) || string.IsNullOrWhiteSpace(AddGroupNameTextBox.Text))
            {
                ErrorManager.ShowMessage("Name can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            Group group = new Group(GroupManager.LastGroupId++, AddGroupNameTextBox.Text);
            ChangeAddGroupPopUpState(open: false);
            GroupManager.AddGroup(group, out string errorMessage);
            if (string.IsNullOrEmpty(errorMessage))
            {
                AddGroup(group);
            }
            else
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void CancelAddGroupCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddGroupPopUpState(open: false);
        }

        private void ChangeSelectedGroupNameCardButton_Click(object sender, RoutedEventArgs e)
        {
            GroupManager.ChangeGroupName(activeGroupId, SelectedGroupNameTextBox.Text, out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                InitializeGroups();
                updateLiveMenuCharts();
            }
            else
            {
                SelectedGroupNameTextBox.Text = GroupManager.GetGroup(activeGroupId).Name;
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }


        public void InitAttributes(bool wasSelectedAttribute = false)
        {
            AttributesStackPanel.Children.Clear();

            Group group = GroupManager.GetGroup(activeGroupId);
            if (group != null)
            {
                List<GroupAttribute> attributes = group.Attributes;

                if (attributes.Any())
                {
                    if (!wasSelectedAttribute)
                    {
                        activeAttributeId = attributes.First().Id;
                    }

                    foreach (GroupAttribute attribute in attributes)
                    {
                        AddAttribute(attribute, withUpdate: false);
                    }
                }
                else
                {
                    activeAttributeId = -1;
                }
            }

            UpdateAttributes(activeAttributeId);
        }

        private void AddAttribute(GroupAttribute attribute, bool withUpdate = true)
        {
            GroupAttributeSettingsItem groupAttributeSettingsItem = new GroupAttributeSettingsItem(attribute, new ChangeActiveAttribute(UpdateAttributes));
            AttributesStackPanel.Children.Add(groupAttributeSettingsItem);

            if (withUpdate)
            {
                activeAttributeId = attribute.Id;
                UpdateAttributes(activeAttributeId);
            }

            updateLiveMenuCharts();
        }

        private void UpdateAttributes(int selectedAttributeId)
        {
            activeAttributeId = selectedAttributeId;
            foreach (GroupAttributeSettingsItem item in AttributesStackPanel.Children)
            {
                item.ChangeColorMode(item.Id == activeAttributeId);
            }

            GroupAttribute activeAttribute = GroupManager.GetGroup(activeGroupId).GetAttribute(activeAttributeId);
            if (activeAttribute != null)
            {
                fieldsViewModel.AttributeName = SelectedAttributeNameTextBox.Text = activeAttribute.Name;
                SelectedAttributeLineWidthTextBox.Text = activeAttribute.LineWidth.ToString();
                fieldsViewModel.LineWidth = activeAttribute.LineWidth;
                SelectedAttributeColorPicker.Color = activeAttribute.ColorCode.ConvertColor();
            }
        }

        private void AddAttributeCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddAttriutePopUpState(open: true);
        }

        private void DeleteAttributeCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (activeAttributeId != -1)
            {
                GroupManager.RemoveAttributeFromGroup(activeGroupId, activeAttributeId, out string errorMessage);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    RemoveAttribute(activeAttributeId);
                    ChangeAddAttriutePopUpState(open: false);
                }
                else
                {
                    ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
                }
            }
        }

        private void RemoveAttribute(int attributeId)
        {
            UIElement deletableItem = new UIElement();
            foreach (GroupAttributeSettingsItem item in AttributesStackPanel.Children)
            {
                if (item.Id == attributeId)
                {
                    deletableItem = item;
                }
            }

            AttributesStackPanel.Children.Remove(deletableItem);

            List<GroupAttribute> attributes = GroupManager.GetGroup(activeGroupId).Attributes;
            if (attributes.Any())
            {
                activeAttributeId = attributes.First().Id;
            }
            else
            {
                activeAttributeId = -1;
            }

            UpdateAttributes(activeAttributeId);
            updateLiveMenuCharts();
        }

        private void ChangeAddAttriutePopUpState(bool open)
        {
            ChangePopUpBackgroundState(open);
            AddAttributeGrid.Visibility = open ? Visibility.Visible : Visibility.Hidden;

            if (open)
            {
                AddAttributeNameTextBox.Focus();
            }

            AddAttributeNameTextBox.Text = string.Empty;
            fieldsViewModel.AddAttributeLineWidth = 1;
        }

        private void AddAttributePopUpCardButton_Click(object sender, RoutedEventArgs e)
        {
            string attributeName = AddAttributeNameTextBox.Text;
            if (string.IsNullOrEmpty(attributeName) || string.IsNullOrWhiteSpace(attributeName))
            {
                ErrorManager.ShowMessage("Name can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            string attributeLineWidth = AddAttributeLineWidthTextBox.Text;
            if (string.IsNullOrEmpty(attributeLineWidth) || string.IsNullOrWhiteSpace(attributeLineWidth))
            {
                ErrorManager.ShowMessage("Line width can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            if (int.TryParse(attributeLineWidth, out int lineWidth))
            {
                if (lineWidth <= 0)
                {
                    ErrorManager.ShowMessage("Line width must be greater than 0", MessageSnackbar, MessageType.Error);
                    return;
                }
            }
            else
            {
                ErrorManager.ShowMessage("Line width must be a number", MessageSnackbar, MessageType.Error);
                return;
            }

            Group activeGroup = GroupManager.GetGroup(activeGroupId);
            GroupAttribute attribute = new GroupAttribute(activeGroup.LastAttributeId++, activeGroupId, attributeName, ColorManager.GetGetChartColor.ToString(), lineWidth);
            ChangeAddAttriutePopUpState(open: false);
            GroupManager.AddAttributeToGroup(activeGroupId, attribute, out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                AddAttribute(attribute);
            }
            else
            {
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void CancelAddAttributeCardButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeAddAttriutePopUpState(open: false);
        }

        private void ChangeSelectedAttributeNameCardButton_Click(object sender, RoutedEventArgs e)
        {
            GroupManager.ChangeAttributeNameInGroup(activeGroupId, activeAttributeId, SelectedAttributeNameTextBox.Text, out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                InitAttributes(wasSelectedAttribute: true);
                updateLiveMenuCharts();
            }
            else
            {
                SelectedAttributeNameTextBox.Text = GroupManager.GetGroup(activeGroupId).GetAttribute(activeAttributeId).Name;
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void ChangeSelectedAttributeLineWidthCardButton_Click(object sender, RoutedEventArgs e)
        {
            string attributeLineWidth = SelectedAttributeLineWidthTextBox.Text;
            if (string.IsNullOrEmpty(attributeLineWidth) || string.IsNullOrWhiteSpace(attributeLineWidth))
            {
                ErrorManager.ShowMessage("Line width can not be empty", MessageSnackbar, MessageType.Error);
                return;
            }

            if (int.TryParse(attributeLineWidth, out int lineWidth))
            {
                if (lineWidth <= 0)
                {
                    ErrorManager.ShowMessage("Line width must be greater than 0", MessageSnackbar, MessageType.Error);
                    return;
                }
            }
            else
            {
                ErrorManager.ShowMessage("Line width must be a number", MessageSnackbar, MessageType.Error);
                return;
            }

            GroupManager.ChangeAttributeLineWidthInGroup(activeGroupId, activeAttributeId, lineWidth, out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                InitAttributes(wasSelectedAttribute: true);
                updateLiveMenuCharts();
            }
            else
            {
                SelectedAttributeLineWidthTextBox.Text = GroupManager.GetGroup(activeGroupId).GetAttribute(activeAttributeId).LineWidth.ToString();
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }

        private void SelectedAttributeColorPicker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GroupManager.ChangeAttributeColorInGroup(activeGroupId, activeAttributeId, SelectedAttributeColorPicker.Color.ToString(), out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                InitAttributes(wasSelectedAttribute: true);
                updateLiveMenuCharts();
            }
            else
            {
                SelectedAttributeColorPicker.Color = GroupManager.GetGroup(activeGroupId).GetAttribute(activeAttributeId).ColorCode.ConvertColor();
                ErrorManager.ShowMessage(errorMessage, MessageSnackbar, MessageType.Error);
            }
        }
    }
}
