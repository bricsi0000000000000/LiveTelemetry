using DataModel.Chart;
using MaterialDesignThemes.Wpf;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UI.Errors;
using UI.Extensions;
using UI.Managers;
using static UI.Managers.MenuManager;

namespace UI.UserControls.Charts
{
    public partial class Chart : UserControl
    {
        private readonly PlottableScatterHighlight plottableScatterHighlight;
        private PlottableVLine plottableVLine;
        private readonly Style chartStyle = ScottPlot.Style.Light1;

        public string ChartName { get; set; }

        public List<string> AttributeNames { get; private set; }

        public bool HasVLine { get; set; } = false;

        private List<ChartValue> values = new List<ChartValue>();
        private Snackbar messageSnackbar;
        private Action<string, string> afterGroupDrop;
        private Action buildCharts;
        private InitializeGroups initializeGroups;

        public Chart(string name, ref Snackbar messageSnackbar, Action<string, string> afterGroupDrop, Action buildCharts, InitializeGroups initializeGroups)
        {
            InitializeComponent();

            ChartName = name;
            this.messageSnackbar = messageSnackbar;
            this.afterGroupDrop = afterGroupDrop;
            this.buildCharts = buildCharts;
            this.initializeGroups = initializeGroups;

            AttributeNames = new List<string>();

            ScottPlotChart.Configure(enableScrollWheelZoom: false);
            ScottPlotChart.plt.YLabel(name);
            ScottPlotChart.plt.Style(chartStyle);
            ScottPlotChart.plt.Colorset(Colorset.OneHalfDark);
            ScottPlotChart.plt.Legend();
            ScottPlotChart.Render();
        }

        public void AddAttributeName(string attributeName)
        {
            AttributeNames.Add(attributeName);
        }

        ///// <summary>
        ///// Checks if the <paramref name="channelName"/> is in the <see cref="attributeNames"/> list.
        ///// </summary>
        ///// <param name="channelName">Channels name you want to check.</param>
        ///// <returns></returns>
        //public bool HasChannelName(string channelName) => attributeNames.Contains(channelName);

        ///// <summary>
        ///// Adds and renders a plot for <see cref="Driverless"/>.
        ///// </summary>
        ///// <param name="xAxisValues">Values on <b>horizontal</b> axis.</param>
        ///// <param name="yAxisValues">Values on <b>vertical</b> axis.</param>
        ///// <param name="lineColor"><see cref="Color"/> of the line.</param>
        ///// <param name="xAxisLabel">Label on <b>horizontal</b> axis. Default is <c>x</c></param>
        //public void AddPlot(double[] xAxisValues,
        //                    double[] yAxisValues,
        //                    Color lineColor,
        //                    int lineWidth,
        //                    string xAxisLabel)
        //{
        //    ScottPlotChart.plt.PlotScatter(xs: xAxisValues, ys: yAxisValues, markerShape: MarkerShape.none, color: lineColor, lineWidth: lineWidth);

        //    ScottPlotChart.plt.XLabel(xAxisLabel, bold: true);
        //    ScottPlotChart.plt.Legend();
        //    ScottPlotChart.Render();
        //}

        //public void AddPlot(double[] xAxisValues,
        //                    Color lineColor,
        //                    int lineWidth,
        //                    string xAxisLabel)
        //{
        //    if (xAxisValues.Any())
        //    {
        //        ScottPlotChart.plt.PlotSignal(xAxisValues, color: lineColor, lineWidth: lineWidth, markerSize: 1);
        //        ScottPlotChart.plt.XLabel(xAxisLabel, bold: true);
        //        ScottPlotChart.plt.Legend();
        //        ScottPlotChart.Render();
        //    }
        //}

        public void AddLivePlot(List<double> xAxisValues,
                                System.Drawing.Color lineColor,
                                string attributeName,
                                int minRenderIndex,
                                int maxRenderIndex,
                                string xAxisLabel = "",
                                int lineWidth = 1
                                )
        {
            try
            {
                if (xAxisValues.Any())
                {
                    // save the incoming values because based on liveChartValues can the side value be updated
                    int index = values.FindIndex(x => x.Name.Equals(xAxisLabel));
                    if (index == -1)
                    {
                        values.Add(new ChartValue
                        {
                            Name = attributeName,
                            Values = xAxisValues
                        });
                    }
                    else
                    {
                        values[index].Values.AddRange(xAxisValues);
                    }

                    ScottPlotChart.plt.PlotSignal(ys: xAxisValues.ToArray(),
                                                  color: lineColor,
                                                  lineWidth: lineWidth,
                                                  markerSize: 1,
                                                  minRenderIndex: minRenderIndex,
                                                  maxRenderIndex: maxRenderIndex);
                    ScottPlotChart.plt.XLabel(xAxisLabel, bold: true);
                    ScottPlotChart.plt.Legend();
                    ScottPlotChart.Render();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //public void UpdateHighlight(double xValue)
        //{
        //    if (plottableScatterHighlight != null)
        //    {
        //        plottableScatterHighlight.HighlightClear();
        //    }

        //    if (plottableVLine != null)
        //    {
        //        ScottPlotChart.plt.Clear(plottableVLine);
        //    }

        //    plottableVLine = ScottPlotChart.plt.PlotVLine(xValue, lineStyle: LineStyle.Dash, color: DefaultsManager.DefaultChartHighlightColor);

        //    ScottPlotChart.Render();
        //}

        public void UpdateLiveHighlight(int dataIndex)
        {
            if (plottableScatterHighlight != null)
            {
                plottableScatterHighlight.HighlightClear();
            }

            if (plottableVLine != null)
            {
                ScottPlotChart.plt.Clear(plottableVLine);
            }

            plottableVLine = ScottPlotChart.plt.PlotVLine(dataIndex, lineStyle: LineStyle.Dash, color: ColorManager.Primary.ConvertToDrawingColor());

            ScottPlotChart.Render();
        }

        ///// <summary>
        ///// Sets the axis limits to automatic.
        ///// </summary>
        public void SetAxisLimitsToAuto()
        {
            ScottPlotChart.plt.AxisAuto();
        }

        ///// <summary>
        ///// Sets the frame border visible or not.
        ///// </summary>
        ///// <param name="left">Left border.</param>
        ///// <param name="bottom">Bottom border.</param>
        ///// <param name="top">Top border.</param>
        ///// <param name="right">Right</param>
        //public void SetFrameBorder(bool left = true, bool bottom = true, bool top = true, bool right = true)
        //{
        //    ScottPlotChart.plt.Frame(left: left, bottom: bottom, top: top, right: right);
        //}

        private ChartValueItem CreateSideValue(string attributeName)
        {
            ChartValueItem chartValue = new ChartValueItem(attributeName, "", ChartName);

            return chartValue;
        }

        public void AddSideValue(string attributeName, List<double> xAxisValues, bool isActive = false, int inputFileId = -1, string colorCode = "", int lineWidth = 1)
        {
            values.Add(new ChartValue
            {
                Name = attributeName,
                Values = xAxisValues
            });

            ChartValueItem chartValue = CreateSideValue(attributeName);

            if (!string.IsNullOrEmpty(colorCode))
            {
                if (isActive)
                {
                    chartValue.SetUp(colorText: colorCode, inputFileId: inputFileId);
                }
            }

            ValuesStackPanel.Children.Add(chartValue);
        }

        public void AddEmptySideValue(string attributeName)
        {
            AddSideValue(attributeName, null, isActive: false);
        }

        //public void UpdateSideValues(ref int dataIndex)
        //{
        //    //  var channel = InputFileManager.GetInputFile(inputFileID).GetChannel(channelName);

        //    /*var unit = UnitOfMeasureManager.GetUnitOfMeasure(channelName);
        //    if (unit == null)
        //    {
        //        unit = UnitOfMeasureManager.GetUnitOfMeasure(channelName.Replace("_", ""));
        //    }*/

        //    //var chartValue = new ChartValue(channelName, unit == null ? "" : unit.UnitOfMeasure, ChartName, color, inputFileID);
        //    //chartValue.SetUp(color, inputFileID);

        //    // double value = dataIndex < channel.Data.Count ? channel.Data[dataIndex] : channel.Data.Last();

        //    foreach (var item in ValuesStackPanel.Children)
        //    {
        //        if (item is ChartValue chartValue)
        //        {
        //            var currentValues = values.Find(x => x.Item1.Equals(chartValue.ChannelName));
        //            if (currentValues != null)
        //            {
        //                if (currentValues.Item2.Length > 0)
        //                {
        //                    double currentValue = dataIndex < currentValues.Item2.Length ? currentValues.Item2[dataIndex] : currentValues.Item2.Last();
        //                    chartValue.SetChannelValue(currentValue);
        //                }
        //            }
        //        }
        //    }
        //    // chartValue.SetChannelValue(value);

        //    // ValuesStackPanel.Children.Add(chartValue);

        //    /*SettingsStackPanel.Children.Add(new ChartValueSettings(channelName: channelName,
        //                                                           groupName: ChartName,
        //                                                           lineWidth: lineWidth,
        //                                                           color: color,
        //                                                           inputFileID: inputFileID,
        //                                                           isActive: isActive));*/

        //    ChannelsGrid.Visibility = System.Windows.Visibility.Hidden;
        //}

        public void UpdateLiveSideValue(int dataIndex)
        {
            if (values.Any())
            {
                foreach (object item in ValuesStackPanel.Children)
                {
                    if (item is ChartValueItem chartValueItem)
                    {
                        List<double> attributeValues = values.Find(x => x.Name.Equals(chartValueItem.AttributeName)).Values;
                        if (attributeValues.Any())
                        {
                            double value;
                            if (dataIndex < attributeValues.Count)
                            {
                                value = attributeValues[dataIndex];
                            }
                            else
                            {
                                value = attributeValues.Last();
                            }

                            chartValueItem.SetLiveAttributeValue(value);
                        }
                    }
                }
            }
        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string sensorName = e.Data.GetData(typeof(string)).ToString();

            DataModel.Group group = GroupManager.GetGroup(ChartName);

            if (group != null)
            {
                if (group.GetAttribute(sensorName) == null)
                {
                    GroupManager.GetGroup(ChartName).AddAttribute(sensorName, ColorManager.GetChartColor.ToString(), 1);
                }
            }
            else
            {
                if (!AttributeNames.Contains(sensorName))
                {
                    AttributeNames.Add(sensorName);

                    string oldName = ChartName;

                    DataModel.Group temporaryGroup = GroupManager.GetGroup($"Temporary{GroupManager.TemporaryGroupIndex}");

                    while (temporaryGroup != null)
                    {
                        GroupManager.TemporaryGroupIndex++;
                        temporaryGroup = GroupManager.GetGroup($"Temporary{GroupManager.TemporaryGroupIndex}");
                    }

                    ChartName = $"Temporary{GroupManager.TemporaryGroupIndex}";

                    GroupManager.AddGroup(GroupManager.MakeGroupWithAttributes(ChartName, AttributeNames), out string errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ErrorManager.ShowMessage(errorMessage, messageSnackbar, MessageType.Error, className: nameof(Chart));
                        return;
                    }

                    initializeGroups();
                    afterGroupDrop(oldName, ChartName);
                }
            }

            buildCharts();

            Mouse.OverrideCursor = null;
        }
    }
}
