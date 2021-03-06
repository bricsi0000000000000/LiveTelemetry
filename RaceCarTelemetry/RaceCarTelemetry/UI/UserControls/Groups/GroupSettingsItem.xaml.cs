using DataModel;
using System.Windows;
using System.Windows.Controls;
using UI.Extensions;
using UI.Managers;
using static UI.UserControls.Settings.GroupSettingsMenu;

namespace UI.UserControls.Groups
{
    public partial class GroupSettingsItem : UserControl
    {
        public int Id { get; private set; }
        public new string Name { get; private set; }

        private readonly ChangeActiveGroup changeActiveGroup;

        public GroupSettingsItem(Group group, ChangeActiveGroup changeActiveGroup)
        {
            InitializeComponent();

            Id = group.Id;
            Name = group.Name;
            
            CardItem.Text = Name;
            this.changeActiveGroup = changeActiveGroup;
        }

        public void ChangeColorMode(bool change)
        {
            CardItem.IsSelected = change;
        }

        public void ChangeBackgroundColor(bool change)
        {
            CardItem.Background = change ? ColorManager.Gray.ConvertBrush() : ColorManager.White.ConvertBrush();
        }

        private void CardItem_Click(object sender, RoutedEventArgs e)
        {
            changeActiveGroup(Id);
        }
    }
}
