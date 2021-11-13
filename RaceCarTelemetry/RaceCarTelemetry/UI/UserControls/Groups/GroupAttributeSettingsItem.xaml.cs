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

        public void ChangeColorMode(bool selected)
        {
            CardItem.IsSelected = selected;
            CardItem.LabelColor = selected ? ColorManager.SelectedForeground.ConvertBrush() : ColorManager.NotSelectedForeground.ConvertBrush();
        }

        private void CardItem_Click(object sender, RoutedEventArgs e)
        {
            changeActiveAttribute(Id);
        }
    }
}
