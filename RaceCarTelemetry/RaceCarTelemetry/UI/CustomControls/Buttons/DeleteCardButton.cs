using System.Windows;
using System.Windows.Controls;

namespace UI.CustomControls
{
    public class DeleteCardButton : Button
    {
        static DeleteCardButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteCardButton), new FrameworkPropertyMetadata(typeof(DeleteCardButton)));
        }
    }
}
