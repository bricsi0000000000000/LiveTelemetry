using System.Windows;

namespace UI.Errors
{
    public partial class ErrorMessagePopUp : Window
    {
        public ErrorMessagePopUp(string message)
        {
            InitializeComponent();

            TitleLabel.Text = message;
        }
    }
}
