using System.Windows.Controls;

namespace UI.UserControls.PageTemplates
{
    public partial class PageTemplateSensorNameSettingsItem : UserControl
    {
        public delegate void RemovePageTemplateSensorNameSettingsItem(string name);

        private readonly RemovePageTemplateSensorNameSettingsItem remove;
        private readonly string name;

        public PageTemplateSensorNameSettingsItem(string name, RemovePageTemplateSensorNameSettingsItem remove)
        {
            InitializeComponent();

            this.name = name;
            this.remove = remove;

            SensorNameLabel.Content = name;
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            remove(name);
        }
    }
}
