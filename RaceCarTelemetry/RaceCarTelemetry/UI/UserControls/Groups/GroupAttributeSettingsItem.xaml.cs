using DataModel;
using System.Windows;
using System.Windows.Controls;
using UI.Extensions;
using UI.Managers;
using static UI.UserControls.Settings.GroupSettingsMenu;

namespace UI.UserControls.Groups
{
    public partial class GroupAttributeSettingsItem : UserControl
    {
        public int Id { get; private set; }
        public int GroupId { get; private set; }

        private int lineWidth;

        public int LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
                CardItem.LineWidth = $"{lineWidth} pt";
            }
        }

        private readonly ChangeActiveAttribute changeActiveAttribute;

        public GroupAttributeSettingsItem(GroupAttribute attribute, ChangeActiveAttribute changeActiveAttribute)
        {
            InitializeComponent();

            Id = attribute.Id;
            GroupId = attribute.GroupId;

            CardItem.AttributeName = attribute.Name;
            CardItem.AttributeColor = attribute.ColorCode.ConvertBrush();
            LineWidth = attribute.LineWidth;

            this.changeActiveAttribute = changeActiveAttribute;
        }

        private void ChangeColorBtn_Click(object sender, RoutedEventArgs e)
        {
            //PickColor pickColor = new PickColor(colorCode);
            //if (pickColor.ShowDialog() == true)
            //{
            //    var pickedColor = pickColor.ColorPicker.Color;
            //    GroupManager.GetGroup(groupName).GetAttribute(AttributeName).ColorText = pickedColor.ToString();

            //    foreach (var inputFile in InputFileManager.InputFiles)
            //    {
            //        var channel = inputFile.GetChannel(AttributeName);
            //        if (channel != null)
            //        {
            //            channel.ColorText = pickedColor.ToString();
            //        }
            //    }

            //    foreach (var group in GroupManager.Groups)
            //    {
            //        var channel = group.GetAttribute(AttributeName);
            //        if (channel != null)
            //        {
            //            channel.ColorText = pickedColor.ToString();
            //        }
            //    }

            //    GroupManager.SaveGroups();
            //    ((GroupSettings)((SettingsMenu)MenuManager.GetMenuTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).InitAttributes();
            //    ((DriverlessMenu)MenuManager.GetMenuTab(TextManager.DriverlessMenuName).Content).BuildCharts();
            //    MenuManager.LiveTelemetry.BuildCharts();
            //    ((InputFilesSettings)((SettingsMenu)MenuManager.GetMenuTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.FilesSettingsName).Content).InitInputFileSettingsItems();
            //}
        }


        /*  GroupManager.GetGroup(groupName).RemoveAttribute(AttributeName);
          if (GroupManager.Groups.Count > 0)
          {
              ((GroupSettings)((SettingsMenu)MenuManager.GetTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).ActiveAttribute = GroupManager.Groups.First().Attributes.First();
          }
          ((GroupSettings)((SettingsMenu)MenuManager.GetTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).InitGroups();
          ((GroupSettings)((SettingsMenu)MenuManager.GetTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).InitActiveChannelSelectableAttributes();
          ((DriverlessMenu)MenuManager.GetTab(TextManager.DriverlessMenuName).Content).UpdateCharts();
          GroupManager.SaveGroups();
        */

        public void ChangeColor(string colorCode)
        {
            CardItem.AttributeColor = colorCode.ConvertBrush();
        }

        public void ChangeColorMode(bool selected)
        {
            CardItem.IsSelected = selected;
            CardItem.LabelColor = selected ? ColorManager.SelectedForeground.ConvertBrush() : ColorManager.NotSelectedForeground.ConvertBrush();
        }

        private void CardItem_Click(object sender, RoutedEventArgs e)
        {
            changeActiveAttribute(Id);
            //((GroupSettings)((SettingsMenu)MenuManager.GetMenuTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).ChangeActiveAttributeItem(Id);
            //((GroupSettings)((SettingsMenu)MenuManager.GetMenuTab(TextManager.SettingsMenuName).Content).GetTab(TextManager.GroupsSettingsName).Content).SelectInputFile();
        }
    }
}
