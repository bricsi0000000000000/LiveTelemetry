using DataModel;
using System.Windows;
using System.Windows.Controls;
using static UI.UserControls.Settings.PageTemplateSettingsMenu;

namespace UI.UserControls.PageTemplates
{
    public partial class PageTemplateSettingsItem : UserControl
    {
        public int Id { get; private set; }
        public new string Name { get; private set; }

        private readonly ChangeActivePageTemplate changeActivePageTemplate;

        public PageTemplateSettingsItem(PageTemplate template, ChangeActivePageTemplate changeActivePageTemplate)
        {
            InitializeComponent();

            Id = template.Id;
            Name = template.Name;

            CardItem.Text = Name;
            this.changeActivePageTemplate = changeActivePageTemplate;
        }

        public void ChangeColorMode(bool change)
        {
            CardItem.IsSelected = change;
        }

        private void CardItem_Click(object sender, RoutedEventArgs e)
        {
            changeActivePageTemplate(Id);
        }
    }
}
