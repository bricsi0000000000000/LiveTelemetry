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

        //private void OkButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    OkButton.Background = ColorManager.Secondary700.ConvertBrush();
        //}

        //private void OkButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    OkButton.Background = ColorManager.Secondary800.ConvertBrush();

        //    Close();
        //}

        //private void OkButton_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    OkButton.Background = ColorManager.Secondary800.ConvertBrush();
        //}

        //private void OkButton_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    OkButton.Background = ColorManager.Secondary900.ConvertBrush();
        //}
    }
}
