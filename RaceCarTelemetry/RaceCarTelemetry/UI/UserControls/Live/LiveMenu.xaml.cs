﻿using BusinessLogic;
using DataModel;
using DataModel.Constants;
using DataModel.Live;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.CustomEventArguments;
using UI.Errors;
using UI.Extensions;
using UI.Managers;
using UI.UserControls.Charts;
using static UI.Managers.MenuManager;
using ConfigurationManager = UI.Managers.ConfigurationManager;

namespace UI.UserControls.Live
{
    public partial class LiveMenu : UserControl
    {
        private static HttpClient client;

        private class SelectedSensor
        {
            public string Name { get; set; }
            public string ColorCode { get; set; }
        }

        private readonly List<string> selectedGroups;
        private readonly List<SelectedSensor> selectedSensors;
        private readonly LiveBusinessLogic liveBusinessLogic;
        private readonly AutoResetEvent getDataSignal;
        private readonly object getDataLock;

        private const int NO_OPACITY = 1;
        private const float LITTLE_OPACITY = .2f;

        private List<SensorValue> sensorValues;
        private List<Sensor> sensors;
        private LiveSession activeSession;
        private int lastPackageId;
        private bool gettingData;
        private bool stickRangeSliderLeft;
        private bool stickRangeSliderRight;
        private bool stickDataSlider;
        private bool stickSliderFromHere;
        private double stickRangeSliderUpperValue;
        private double stickRangeSliderLowerValue;

        public UpdateLiveSettingsCarStatus UpdateCarStatus { get; set; }
        public bool CanUpdateCharts { get; private set; }
        public bool IsSelectedSession { get; set; }

        public LiveMenu()
        {
            InitializeComponent();

            client = new HttpClient();

            selectedGroups = new List<string>();
            getDataSignal = new AutoResetEvent(false);
            getDataLock = new object();

            lastPackageId = 0;
            gettingData = false;
            stickRangeSliderLeft = true;
            stickRangeSliderRight = true;
            stickDataSlider = true;
            stickSliderFromHere = false;

            CanUpdateCharts = false;
            IsSelectedSession = false;
            selectedSensors = new List<SelectedSensor>();
            sensorValues = new List<SensorValue>();
            sensors = new List<Sensor>();

            RangeSliderStickLeftButton.Toggle(stickRangeSliderLeft);
            RangeSliderStickRightButton.Toggle(stickRangeSliderRight);
            DataSliderStickButton.Toggle(stickDataSlider);

            UpdateCoverGridsVisibilities();

            liveBusinessLogic = new LiveBusinessLogic();
        }

        private void UpdateCoverGridsVisibilities()
        {
            NoSessionGrid.Visibility = IsSelectedSession ? Visibility.Hidden : Visibility.Visible;
            NoChannelsGrid.Visibility = IsSelectedSession ? Visibility.Hidden : Visibility.Visible;
            NoGroupsGrid.Visibility = IsSelectedSession ? Visibility.Hidden : Visibility.Visible;
            NoChartsGrid.Visibility = IsSelectedSession ? Visibility.Hidden : Visibility.Visible;
            PlayImageCardButton.Opacity = IsSelectedSession ? NO_OPACITY : LITTLE_OPACITY;
        }

        public void InitilaizeHttpClient()
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

        public void InitializeGroupItems()
        {
            GroupsStackPanel.Children.Clear();

            foreach (Group group in GroupManager.Groups)
            {
                CheckBox checkBox = new CheckBox()
                {
                    Content = group.Name
                };

                checkBox.IsChecked = selectedGroups.Contains(group.Name);
                checkBox.Checked += GroupCheckBox_CheckedClick;
                checkBox.Unchecked += GroupCheckBox_CheckedClick;

                GroupsStackPanel.Children.Add(checkBox);
            }
        }

        private void GroupCheckBox_CheckedClick(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string content = checkBox.Content.ToString();

            if ((bool)checkBox.IsChecked)
            {
                selectedGroups.Add(content);
            }
            else
            {
                selectedGroups.Remove(content);
            }


            SetUpSliders();

            BuildCharts();
        }

        public async void Update(LiveSession session)
        {
            selectedSensors.Clear();
            selectedGroups.Clear();
            sensorValues.Clear();

            SensorsStackPanel.Children.Clear();

            activeSession = session;
            IsSelectedSession = session != null;

            UpdateCoverGridsVisibilities();

            try
            {
                sensors = await liveBusinessLogic.GetAllSensors(client, ConfigurationManager.Configuration.GetApiCall(ApiCallManager.GET_ALL_SENSORS));
            }
            catch (Exception exception)
            {
                ErrorManager.ShowMessage("There was an error getting the sensors from the server", MessageSnackbar, MessageType.Error, className: nameof(LiveMenu), exceptionMessage: exception.Message);
            }

            if (session != null)
            {
                SessionStatusIcon.Kind = session.IsLive ? PackIconKind.AccessPoint : PackIconKind.AccessPointOff;
                SessionStatusIcon.Foreground = session.IsLive ? ColorManager.Primary.ConvertBrush() : ColorManager.Secondary.ConvertBrush();
                SessionNameTextBox.Text = session.Name;
                SessionDateLabel.Text = session.Date.ToShortDateString();

                UpdateSensorsStackPanel(session.SensorNames);

                UpdateRecieveDataStatus();
            }
            else
            {
                SessionNameTextBox.Text = "No active session";
                SessionStatusIcon.Kind = PackIconKind.AccessPointOff;
                SessionStatusIcon.Foreground = ColorManager.Secondary.ConvertBrush();
            }

            SetUpSliders();

            BuildCharts();
        }

        private void UpdateRecieveDataStatus()
        {
            if (activeSession != null)
            {
                if (activeSession.IsLive)
                {
                    PlayImageCardButton.Kind = CanUpdateCharts ? PackIconKind.Pause.ToString() : PackIconKind.Play.ToString();
                }
                else
                {
                    PlayImageCardButton.Kind = PackIconKind.TrayArrowDown.ToString();
                    PlayImageCardButton.Opacity = gettingData ? LITTLE_OPACITY : NO_OPACITY;
                    Mouse.OverrideCursor = gettingData ? Cursors.Wait : null;
                }
            }
        }

        private void UpdateSensorsStackPanel(List<string> sensorNames)
        {
            if (sensorNames != null)
            {
                SensorsStackPanel.Children.Clear();
                foreach (string sensorName in sensorNames)
                {
                    CheckBox checkBox = new CheckBox
                    {
                        Name = $"{sensorName.Replace(" ", "")}_checkBox",
                        Content = sensorName
                    };
                    checkBox.Checked += SensorCheckBox_Checked;
                    checkBox.Unchecked += SensorCheckBox_Checked;

                    SensorsStackPanel.Children.Add(checkBox);
                }
            }
        }

        private void SensorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string content = checkBox.Content.ToString();
            bool isChecked = (bool)checkBox.IsChecked;

            if (isChecked)
            {
                selectedSensors.Add(new SelectedSensor { Name = content, ColorCode = ColorManager.GetGetChartColor.ToString() });
            }
            else
            {
                selectedSensors.RemoveAt(selectedSensors.FindIndex(x => x.Name.Equals(content)));
            }

            SetUpSliders();

            BuildCharts();
        }

        private void PlayImageCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (!gettingData)
            {
                if (IsSelectedSession)
                {
                    //PlayImageCardButton.Background = ColorManager.Secondary100.ConvertBrush();

                    if (activeSession.IsLive)
                    {
                        CanUpdateCharts = !CanUpdateCharts;
                    }
                    else
                    {
                        gettingData = true;
                        CanUpdateCharts = true;
                    }

                    if (CanUpdateCharts)
                    {
                        Thread collectDataThread = new Thread(new ThreadStart(CollectData));
                        collectDataThread.Start();
                    }
                    else
                    {
                        //InputFileManager.SaveFile(activeSession.Name);
                        UpdateCarStatus();
                    }

                    UpdateRecieveDataStatus();
                }
            }
        }

        private void UpdateSensorNames()
        {
            if (activeSession.SensorNames == null || !activeSession.SensorNames.Any())
            {
                GetSensorNames();
            }
        }

        private async void GetSensorNames()
        {
            string apiCall = $"{ConfigurationManager.Configuration.GetApiCall(ApiCallManager.GET_SENSOR_NAMES)}{activeSession.SessionId}";

            try
            {
                activeSession.SensorNames = await liveBusinessLogic.GetSensorNames(client, apiCall);
                UpdateSensorsStackPanel(activeSession.SensorNames);
            }
            catch (Exception exception)
            {
                ErrorManager.ShowMessage($"There was a problem getting sensors from {activeSession.Name}", MessageSnackbar, type: MessageType.Error, className: nameof(LiveMenu), exceptionMessage: exception.Message);
            }
        }

        private async void CollectData()
        {
            while (CanUpdateCharts)
            {
                if (activeSession == null)
                {
                    continue;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateSensorNames();
                });

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string apiCall;
                // if lastPackageId is 0 (so it's the first call), then get all package and continue getting from that
                if (lastPackageId == 0)
                {
                    apiCall = $"{ConfigurationManager.Configuration.GetApiCall(ApiCallManager.GET_ALL_PACKAGES)}/{activeSession.SessionId}";
                }
                else
                {
                    apiCall = $"{ConfigurationManager.Configuration.GetApiCall(ApiCallManager.GET_PACKAGES_AFTER)}/{lastPackageId}/{activeSession.SessionId}";
                }

                try
                {
                    List<SensorValue> newSensorValues = await liveBusinessLogic.GetPackagesSensorValues(client, apiCall);

                    stopwatch.Stop();

                    if (newSensorValues.Any())
                    {
                        lock (getDataLock)
                        {
                            sensorValues.AddRange(newSensorValues);
                        }

                        lastPackageId = sensorValues.Last().PackageId;

                        getDataSignal.Set();

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            BuildCharts();
                            SetUpSliders();
                            // MenuManager.LiveSettings.ChangeLoadedPackagesLabel(lastPackageId);
                        });

                        if (activeSession.IsLive)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //MenuManager.LiveSettings.UpdateCarStatus(sensorValues.First().SentTime, stopwatch.ElapsedMilliseconds);
                            });
                        }
                    }

                    if (!activeSession.IsLive)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            gettingData = false;
                            CanUpdateCharts = false;
                            UpdateRecieveDataStatus();
                        });
                    }

                    Thread.Sleep(ConfigurationManager.Configuration.WaitBetweenCollectData);
                }
                catch (Exception exception)
                {
                    ErrorManager.ShowMessage("There was a problem getting data from the server", MessageSnackbar, MessageType.Error, className: nameof(LiveMenu), exceptionMessage: exception.Message);
                }
            }
        }

        private void SetUpSliders()
        {
            List<double> values = new List<double>();

            if (selectedSensors.Any())
            {
                values = sensorValues.FindAll(x => x.SensorId == sensors.Find(i => i.Name.Equals(selectedSensors.First().Name)).SensorId).Select(x => x.Value).ToList();
            }
            else if (selectedGroups.Any())
            {
                foreach (Group group in GroupManager.Groups)
                {
                    if (group.Name.Equals(selectedGroups.First()))
                    {
                        if (group.Attributes.Any())
                        {
                            values = sensorValues.FindAll(x => x.SensorId == sensors.Find(i => i.Name.Equals(group.Attributes.First().Name)).SensorId).Select(x => x.Value).ToList();
                        }
                    }
                }
            }

            if (values.Any())
            {
                DataSlider.Maximum =
                RangeSlider.Maximum = values.Count;
            }
            else
            {
                DataSlider.Maximum =
                RangeSlider.Maximum = 0;
            }

            if (stickDataSlider)
            {
                DataSlider.Value = DataSlider.Maximum;
            }

            StickRangeSlider();
        }

        private void StickRangeSlider()
        {
            stickSliderFromHere = true;

            RangeSlider.LowerValue = stickRangeSliderLeft ? RangeSlider.Minimum : stickRangeSliderLowerValue;
            RangeSlider.UpperValue = stickRangeSliderRight ? RangeSlider.Maximum : stickRangeSliderUpperValue;
        }

        public void UpdateRangeSlider()
        {
            BuildCharts();
        }

        private void BuildCharts()
        {
            ChartsGrid.Children.Clear();
            ChartsGrid.RowDefinitions.Clear();

            int rowIndex = 0;
            foreach (SelectedSensor sensor in selectedSensors) // basic single chart
            {
                Group group = new Group(GroupManager.LastGroupId++, sensor.Name);

                group.AddAttribute(sensor.Name, sensor.ColorCode, 1);

                BuildChartGrid(group, ref rowIndex);
            }

            foreach (Group group in GroupManager.Groups)
            {
                if (selectedGroups.Contains(group.Name))
                {
                    BuildChartGrid(group, ref rowIndex);
                }
            }

            RowDefinition chartRow = new RowDefinition()
            {
                Height = new GridLength()
            };

            ChartsGrid.RowDefinitions.Add(chartRow);

            Grid.SetRow(new Grid(), rowIndex++);

            RefreshCharts();
        }

        private void BuildChartGrid(Group group, ref int rowIndex)
        {
            RowDefinition chartRow = new RowDefinition()
            {
                Height = new GridLength(200)
            };
            RowDefinition gridSplitterRow = new RowDefinition
            {
                Height = new GridLength(5)
            };

            ChartsGrid.RowDefinitions.Add(chartRow);
            ChartsGrid.RowDefinitions.Add(gridSplitterRow);

            GridSplitter splitter = new GridSplitter
            {
                ResizeDirection = GridResizeDirection.Rows,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                //Background = ColorManager.Secondary100.ConvertBrush()
            };
            ChartsGrid.Children.Add(splitter);

            Chart chart = BuildGroupChart(group);
            ChartsGrid.Children.Add(chart);

            Grid.SetRow(chart, rowIndex++);
            Grid.SetRow(splitter, rowIndex++);
        }

        private Chart BuildGroupChart(Group group)
        {
            Chart chart = new Chart(group.Name);

            foreach (GroupAttribute attribute in group.Attributes)
            {
                chart.AddAttributeName(attribute.Name);

                Sensor sensor = sensors.Find(x => x.Name.Equals(attribute.Name));
                if (sensor != null)
                {
                    int sensorId = sensor.SensorId;

                    List<double> values = sensorValues.FindAll(x => x.SensorId == sensorId).Select(x => x.Value).ToList();

                    if (values.Any())
                    {
                        if (RangeSlider.UpperValue > 0)
                        {
                            int minRenderIndex = (int)RangeSlider.LowerValue;
                            int maxRenderIndex = (int)RangeSlider.UpperValue;
                            if (maxRenderIndex >= values.Count)
                            {
                                maxRenderIndex = values.Count - 1;
                            }

                            chart.AddLivePlot(values, attribute.ColorCode.ConvertToChartColor(), attribute.Name, minRenderIndex, maxRenderIndex);
                        }
                    }

                    chart.AddSideValue(attribute.Name, values, colorCode: attribute.ColorCode, isActive: values.Any());
                }
            }

            chart.SetAxisLimitsToAuto();

            return chart;
        }

        private void RefreshCharts()
        {
            if (selectedSensors.Any())
            {
                foreach (object item in ChartsGrid.Children)
                {
                    if (item is Chart chart)
                    {
                        List<double> values = new List<double>();
                        foreach (SensorValue value in sensorValues)
                        {
                            foreach (Sensor sensor in sensors)
                            {
                                if (sensor.Name.Equals(chart.AttributeNames.First()))
                                {
                                    if (value.SensorId == sensor.SensorId)
                                    {
                                        values.Add(value.Value);
                                    }
                                }
                            }
                        }

                        if (values.Any())
                        {
                            if (RangeSlider.UpperValue > 0)
                            {
                                int minRenderIndex = (int)RangeSlider.LowerValue;
                                int maxRenderIndex = (int)RangeSlider.UpperValue;
                                if (maxRenderIndex >= values.Count)
                                {
                                    maxRenderIndex = values.Count - 1;
                                }

                                maxRenderIndex -= minRenderIndex;

                                int dataIndex = (int)DataSlider.Value;
                                double dataIndexRate = (double)dataIndex / values.Count;

                                dataIndex = (int)((maxRenderIndex * dataIndexRate) + minRenderIndex);
                                chart.UpdateLiveHighlight(dataIndex);
                                chart.UpdateLiveSideValue(dataIndex);
                            }
                        }
                    }
                }
            }
        }

        private void DataSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (stickDataSlider)
            {
                DataSliderStickButton.Toggle(stickDataSlider);
                stickDataSlider = false;
            }

            UpdateRangeSlider();
        }

        private void RangeSlider_ValueChanged(RangeSliderEventArgs rangeSliderEventArgs)
        {
            if (!stickSliderFromHere)
            {
                switch (rangeSliderEventArgs.Side)
                {
                    case RangeSliderSide.Left:
                        if (stickRangeSliderLeft)
                        {
                            stickRangeSliderLeft = false;
                            RangeSliderStickLeftButton.Toggle(stickRangeSliderLeft);
                        }
                        break;

                    case RangeSliderSide.Right:
                        if (stickRangeSliderRight)
                        {
                            stickRangeSliderRight = false;
                            RangeSliderStickRightButton.Toggle(stickRangeSliderRight);
                        }
                        break;
                }
            }

            stickRangeSliderUpperValue = RangeSlider.UpperValue;
            stickRangeSliderLowerValue = RangeSlider.LowerValue;

            UpdateRangeSlider();
        }

        private void RangeSliderStickLeftButton_Click(object sender, RoutedEventArgs e)
        {
            stickSliderFromHere = true;
            stickRangeSliderLeft = !stickRangeSliderLeft;
            RangeSliderStickLeftButton.Toggle(stickRangeSliderLeft);
            StickRangeSlider();
        }

        private void RangeSliderStickRightButton_Click(object sender, RoutedEventArgs e)
        {
            stickSliderFromHere = true;
            stickRangeSliderRight = !stickRangeSliderRight;
            RangeSliderStickRightButton.Toggle(stickRangeSliderRight);
            StickRangeSlider();
        }

        private void DataSliderStickButton_Click(object sender, RoutedEventArgs e)
        {
            stickDataSlider = !stickDataSlider;
            DataSliderStickButton.Toggle(stickDataSlider);
            if (stickDataSlider)
            {
                DataSlider.Value = DataSlider.Maximum;
            }
        }
    }
}