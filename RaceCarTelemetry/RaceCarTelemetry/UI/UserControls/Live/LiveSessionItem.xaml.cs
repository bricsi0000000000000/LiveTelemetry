using DataModel.Live;
using System.Windows.Controls;
using static UI.UserControls.Settings.LiveSettingsMenu;

namespace UI.UserControls.Live
{
    public partial class LiveSessionItem : UserControl
    {
        public int Id { get; private set; }

        private readonly ChangeSelectedSession changeSelectedSession;

        public LiveSessionItem(LiveSession session, bool isSelected, ChangeSelectedSession changeSelectedSession)
        {
            InitializeComponent();

            Id = session.SessionId;
            this.changeSelectedSession = changeSelectedSession;

            ItemCard.SessionName = session.Name;
            ItemCard.SessionDate = session.Date.ToString("yyyy.MM.dd");
            ItemCard.ChangeIconStatus(session.IsLive);
            ItemCard.IsSelected = isSelected;
        }

        private void ItemCard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            changeSelectedSession(Id);
        }
    }
}
