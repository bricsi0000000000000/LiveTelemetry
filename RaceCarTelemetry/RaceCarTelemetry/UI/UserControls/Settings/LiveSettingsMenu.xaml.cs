using BusinessLogic;
using DataModel.Constants;
using DataModel.Live;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.Errors;
using UI.Extensions;
using UI.Managers;
using UI.UserControls.Live;
using UI.ValidationRules;
using static UI.Managers.MenuManager;
using ConfigurationManager = UI.Managers.ConfigurationManager;

namespace UI.UserControls.Settings
{
    public partial class LiveSettingsMenu : UserControl
    {
        private static HttpClient client;

        private readonly FieldsViewModel fieldsViewModel;
        private readonly ConfigurationBusinessLogic configurationBusinessLogic;
        private readonly LiveBusinessLogic liveBusinessLogic;
        private readonly UpdateLiveMenu updateLiveMenu;

        private List<LiveSession> sessions;
        private int selectedSessionId;
        private bool gettingsHealthCheck;

        public delegate void ChangeSelectedSession(int selectedSessionId);

        public LiveSettingsMenu(UpdateLiveMenu updateLiveMenu, FinishedReadingConfiguration finishedReadingConfiguration)
        {
            InitializeComponent();

            this.updateLiveMenu = updateLiveMenu;

            client = new HttpClient();
            fieldsViewModel = new FieldsViewModel();
            sessions = new List<LiveSession>();
            gettingsHealthCheck = false;

            UpdateCarStatus();

            SessionDataGridCover.Visibility = Visibility.Visible;

            fieldsViewModel.SessionName = "placeholder";
            DataContext = fieldsViewModel;

            configurationBusinessLogic = new ConfigurationBusinessLogic();
            liveBusinessLogic = new LiveBusinessLogic();

            LoadLiveConfiguration();

            if (ConfigurationManager.Configuration != null)
            {
                finishedReadingConfiguration();

                InitilaizeHttpClient();
                HealthCheck();
            }
        }

        private void LoadLiveConfiguration()
        {
            ConfigurationManager.Configuration = configurationBusinessLogic.LoadLiveConfiguration(FilePathManager.ConfigurationFilePath, out string errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorManager.ShowErrorMessage(errorMessage, ErrorSnackbar, nameof(LiveSettingsMenu), errorMessage);
            }
        }

        private async void HealthCheck()
        {
            gettingsHealthCheck = true;

            bool serverStatus = false;
            try
            {
                ServerStatusIcon.Visibility = Visibility.Hidden;
                ServerStatusProgressBar.Visibility = Visibility.Visible;

                serverStatus = await liveBusinessLogic.HealthCheck(client, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.HEALTH_CHECK));
            }
            catch (Exception exception)
            {
                ErrorManager.ShowErrorMessage("Could not connect to the server", ErrorSnackbar, nameof(LiveSettingsMenu), exception.Message);
            }

            ServerStatusIcon.Visibility = Visibility.Visible;
            ServerStatusProgressBar.Visibility = Visibility.Hidden;

            UpdateServerStatusIcon(serverStatus);

            gettingsHealthCheck = false;
        }

        private void UpdateServerStatusIcon(bool connected)
        {
            ServerStatusIcon.Kind = connected ? PackIconKind.CloudCheck : PackIconKind.CloudAlert;
            ServerStatusIcon.Foreground = connected ? ColorManager.SelectedForeground.ConvertBrush() : ColorManager.Error;
        }

        private void InitilaizeHttpClient()
        {
            //  ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;

            client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(ConfigurationManager.Configuration.TimeOut),
                BaseAddress = new Uri(ConfigurationManager.Configuration.Address)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiCallManager.HTTP_CLIENT_HEADER_TYPE));
        }

        private void RefreshSessionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                UpdateLoadingGrid(visibility: true, "Loading sessions from the server..");

                GetAllSessions(selectedSessionId);
            }
            else
            {
                ErrorManager.ShowErrorMessage("Load ConfigurationManager.Configuration first", ErrorSnackbar, nameof(LiveSettingsMenu));
            }
        }

        private void UpdateLoadingGrid(bool visibility, string message = "", bool progressBarVisibility = true, bool cancelButtonVisibility = false)
        {
            LoadingGrid.Visibility = visibility ? Visibility.Visible : Visibility.Hidden;
            LoadingProgressBar.Visibility = progressBarVisibility ? Visibility.Visible : Visibility.Hidden;
            LoadingCancelButtonCard.Visibility = cancelButtonVisibility ? Visibility.Visible : Visibility.Hidden;

            LoadingLabel.Content = message;
        }

        private async void GetAllSessions(int selectedSessionId = -1)
        {
            try
            {
                sessions = await liveBusinessLogic.GetAllLiveSessions(client, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.ALL_LIVE_SESSIONS));
                UpdateServerStatusIcon(connected: true);
            }
            catch (Exception exception)
            {
                ErrorManager.ShowErrorMessage("There was a problem getting the live sessions", ErrorSnackbar, nameof(LiveSettingsMenu), exception.Message);
                UpdateServerStatusIcon(connected: false);
                UpdateLoadingGrid(visibility: false);

                return;
            }

            if (sessions != null && sessions.Any())
            {
                if (selectedSessionId == -1 || GetSession(selectedSessionId) is null)
                {
                    selectedSessionId = sessions.First().SessionId;
                }

                FillSessionsStackPanel();

                SessionDataGridCover.Visibility = Visibility.Hidden;
            }
            else
            {
                ErrorManager.ShowMessage("There are no sessions on the server", ErrorSnackbar);
                SessionDataGridCover.Visibility = Visibility.Visible;

                selectedSessionId = -1;
                SessionsStackPanel.Children.Clear();
            }

            UpdateLoadingGrid(visibility: false);
        }

        private void FillSessionsStackPanel()
        {
            SessionsCoverGridGrid.Visibility = Visibility.Visible;
            SessionDataGridCover.Visibility = Visibility.Visible;

            SessionsStackPanel.Children.Clear();

            selectedSessionId = sessions.First().SessionId;

            foreach (LiveSession session in sessions)
            {
                SessionsStackPanel.Children.Add(new LiveSessionItem(session, isSelected: session.SessionId == selectedSessionId, new ChangeSelectedSession(UpdateSelectedSession)));
            }

            UpdateSelectedSessionPanel();

            SessionsCoverGridGrid.Visibility = Visibility.Hidden;
            SessionDataGridCover.Visibility = Visibility.Hidden;

            //MenuManager.LiveTelemetry.ResetCharts();
        }

        private void UpdateSelectedSession(int sessionId)
        {
            selectedSessionId = sessionId;

            foreach (LiveSessionItem liveSessionItem in SessionsStackPanel.Children)
            {
                liveSessionItem.ItemCard.IsSelected = liveSessionItem.Id == selectedSessionId;
            }

            UpdateSelectedSessionPanel();
        }

        private async Task GetActiveSessionSensors()
        {
            LiveSession selectedSession = GetSelectedSession;
            UpdateLoadingGrid(visibility: true, $"Getting sensor names of {selectedSession.Name}..");

            try
            {
                selectedSession.SensorNames = await liveBusinessLogic.GetActiveSessionSensors(client, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.ACTIVE_SESSION_SENSORS));
                UpdateServerStatusIcon(connected: true);
            }
            catch (Exception exception)
            {
                ErrorManager.ShowErrorMessage($"There was a problem getting the sensors of {selectedSession.Name}", ErrorSnackbar, nameof(LiveSettingsMenu), exception.Message);
                UpdateServerStatusIcon(connected: false);
            }

            UpdateLoadingGrid(visibility: false);
        }

        private async void UpdateSelectedSessionPanel()
        {
            await GetActiveSessionSensors();

            LiveSession selectedSession = GetSelectedSession;
            SelectedSessionNameTextBox.Text = selectedSession.Name;

            SelectedSessionSensorsTreeView.Items.Clear();

            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = "Sensors",
                FontWeight = FontWeights.Bold,
                IsExpanded = true
            };

            if (selectedSession.SensorNames != null)
            {
                foreach (string sensorName in selectedSession.SensorNames)
                {
                    treeViewItem.Items.Add(new TreeViewItem
                    {
                        Header = sensorName,
                        FontWeight = FontWeights.Normal
                    });
                }
            }

            SelectedSessionSensorsTreeView.Items.Add(treeViewItem);

            UpdateChangeSessionStatusCardButton(selectedSession.IsLive);

            updateLiveMenu(selectedSession);
        }

        private async void ChangeSelectedSessionNameCardButton_Click(object sender, RoutedEventArgs e)
        {
            LiveSession session = GetSelectedSession;

            string name = SelectedSessionNameTextBox.Text;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                SelectedSessionNameTextBox.Text = session.Name;
                ErrorManager.ShowMessage("Name can not be empty", ErrorSnackbar, isError: true);
            }
            else
            {
                UpdateLoadingGrid(visibility: true, "Updating session..");
                session.Name = name;

                try
                {
                    int resultCode = await liveBusinessLogic.ChangeSessionName(client, session, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.CHANGE_SESSION_NAME));

                    if (resultCode == (int)HttpStatusCode.OK)
                    {
                        FillSessionsStackPanel();
                    }
                    else
                    {
                        ErrorManager.ShowErrorMessage("Couldn't update session", ErrorSnackbar, nameof(LiveSettingsMenu), $"Result code: {resultCode}");
                    }

                    UpdateServerStatusIcon(connected: true);
                }
                catch (Exception exception)
                {
                    ErrorManager.ShowErrorMessage("Couldn't update session", ErrorSnackbar, nameof(LiveSettingsMenu), exception.Message);
                    UpdateServerStatusIcon(connected: false);
                }


                UpdateLoadingGrid(visibility: false);
            }
        }

        private async void ChangeSessionStatusCardButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateLoadingGrid(visibility: true);

            LiveSession selectedSession = GetSelectedSession;

            try
            {
                string apiCall;
                if (selectedSession.IsLive) // change to offline
                {
                    apiCall = ConfigurationManager.Configuration.GetApiCall(ApiCallManager.CHANGE_SESSION_TO_OFFLINE);
                }
                else //change to live
                {
                    apiCall = ConfigurationManager.Configuration.GetApiCall(ApiCallManager.CHANGE_SESSION_TO_LIVE);
                }

                int resultCode = await liveBusinessLogic.ChangeSessionState(client, !selectedSession.IsLive, selectedSession.SessionId, apiCall);

                if (resultCode == (int)HttpStatusCode.OK)
                {
                    selectedSession.IsLive = !selectedSession.IsLive;

                    ChangeSessionStatus(selectedSession.SessionId, selectedSession.IsLive);

                    UpdateLoadingGrid(visibility: false);

                    UpdateChangeSessionStatusCardButton(selectedSession.IsLive);
                }
                else if (resultCode == (int)HttpStatusCode.Conflict)
                {
                    UpdateLoadingGrid(visibility: false);
                    ErrorManager.ShowMessage("Only one live session can be at a time!", ErrorSnackbar);
                }
                else
                {
                    UpdateLoadingGrid(visibility: false);
                    ErrorManager.ShowMessage($"Couldn't update {selectedSession.Name}'s status from " +
                                             $"{(selectedSession.IsLive ? "live" : "offline")} to " +
                                             $"{(!selectedSession.IsLive ? "live" : "offline")}", ErrorSnackbar);
                }
            }
            catch (Exception exception)
            {
                ErrorManager.ShowErrorMessage($"There was an error updating {selectedSession.Name}", ErrorSnackbar, nameof(LiveSettingsMenu), exception.Message);
                UpdateServerStatusIcon(connected: false);
                UpdateLoadingGrid(visibility: false);
            }
        }

        private void UpdateChangeSessionStatusCardButton(bool isLive)
        {
            ChangeSessionStatusCardButton.Kind = isLive ? PackIconKind.AccessPoint.ToString() : PackIconKind.AccessPointOff.ToString();
            ChangeSessionStatusCardButton.Foreground = isLive ? ColorManager.Primary.ConvertBrush() : ColorManager.Secondary.ConvertBrush();
        }

        private void ChangeSessionStatus(int sessionId, bool status)
        {
            foreach (LiveSessionItem liveSessionItem in SessionsStackPanel.Children)
            {
                if (liveSessionItem.Id == sessionId)
                {
                    liveSessionItem.ItemCard.ChangeIconStatus(status);
                }
            }
        }

        private async void DeleteSessionCardButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateLoadingGrid(visibility: true, "Deleting session..");

            try
            {
                int resultCode = await liveBusinessLogic.DeleteSession(client, $"{ConfigurationManager.Configuration.GetApiCall(ApiCallManager.DELETE_SESSION)}{selectedSessionId}");
                if (resultCode == (int)HttpStatusCode.OK)
                {
                    UpdateLoadingGrid(visibility: false);
                    GetAllSessions(selectedSessionId);
                }
                else
                {
                    UpdateLoadingGrid(visibility: false);
                    ErrorManager.ShowErrorMessage("Couldn't delete session", ErrorSnackbar);
                }
            }
            catch (Exception)
            {
                UpdateLoadingGrid(visibility: false);
                ErrorManager.ShowErrorMessage("Can't delete session because can't connect to the server", ErrorSnackbar);
            }
        }

        private LiveSession GetSession(int sessionId)
        {
            if (sessions.Any())
            {
                return sessions.Find(x => x.SessionId == sessionId);
            }
            else
            {
                return null;
            }
        }

        private LiveSession GetSelectedSession
        {
            get
            {
                return GetSession(selectedSessionId);
            }
        }

        private void RefreshHealthCheckImageCardButtonWithoutShadow_Click(object sender, RoutedEventArgs e)
        {
            if (!gettingsHealthCheck)
            {
                HealthCheck();
            }
        }

        private void AddSession_Click(object sender, RoutedEventArgs e)
        {
            UpdateLoadingGrid(visibility: true, "Adding new session..");

            AddNewSession(AddLiveSessionNameTextBox.Text);

            AddLiveSessionNameTextBox.Text = string.Empty;
        }

        private async void AddNewSession(string sessionName)
        {
            try
            {
                int resultCode = await liveBusinessLogic.AddSession(client, new LiveSession { Name = sessionName, Date = DateTime.Now }, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.POST_NEW_SESSION));
                if (resultCode == (int)HttpStatusCode.OK)
                {
                    GetAllSessions();
                    UpdateLoadingGrid(visibility: false);
                }
                else if (resultCode == (int)HttpStatusCode.Conflict)
                {
                    UpdateLoadingGrid(visibility: false);
                    ErrorManager.ShowErrorMessage($"There is already a session called {sessionName}", ErrorSnackbar);
                }
                else
                {
                    UpdateLoadingGrid(visibility: false);
                    ErrorManager.ShowErrorMessage("There was a problem with the server", ErrorSnackbar);
                }
            }
            catch (Exception)
            {
                UpdateLoadingGrid(visibility: false);
                ErrorManager.ShowErrorMessage($"Can't add {sessionName} because can't connect to the server", ErrorSnackbar);
            }
        }

        /// <param name="sentTime">Time when the package was sent from the data sender</param>
        /// <param name="arrivedTime">Time when the package was sent from the server</param>
        public void UpdateCarStatus(TimeSpan? sentTime = null, long? arrivedTime = null)
        {
            if (sentTime != null)
            {
                CarToDBConnectionSpeedLabel.Content = $"{sentTime.Value.Milliseconds:f0} ms";
            }
            else
            {
                CarToDBConnectionSpeedLabel.Content = "-";
            }

            if (arrivedTime != null)
            {
                DBToAppConnectionSpeedLabel.Content = $"{(long)arrivedTime:f0} ms";
            }
            else
            {
                DBToAppConnectionSpeedLabel.Content = "-";
            }
        }
    }
}
