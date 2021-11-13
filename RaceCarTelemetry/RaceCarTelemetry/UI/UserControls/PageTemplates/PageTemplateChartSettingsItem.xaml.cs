using System.Windows.Controls;
using UI.Managers;
using UI.Extensions;
using System;
using ScottPlot.Drawing;

namespace UI.UserControls.PageTemplates
{
    public partial class PageTemplateChartSettingsItem : UserControl
    {
        public delegate void PageTemplateChartSettingsItemMoveUp(string name, int oldIndex);
        public delegate void PageTemplateChartSettingsItemMoveDown(string name, int oldIndex);

        private readonly PageTemplateChartSettingsItemMoveUp moveUp;
        private readonly PageTemplateChartSettingsItemMoveDown moveDown;

        public int Index { get; set; }
        public string ChartName { get; set; }

        public PageTemplateChartSettingsItem(string name, int index, PageTemplateChartSettingsItemMoveUp moveUp, PageTemplateChartSettingsItemMoveDown moveDown)
        {
            InitializeComponent();

            this.moveUp = moveUp;
            this.moveDown = moveDown;
            this.Index = index;
            this.ChartName = name;

            ScottPlotChart.Configure(enableScrollWheelZoom: false);
            ScottPlotChart.plt.YLabel(name, bold: true);
            ScottPlotChart.plt.Style(ScottPlot.Style.Light1);
            ScottPlotChart.plt.Colorset(Colorset.OneHalfDark);
            ScottPlotChart.plt.Legend();
            ScottPlotChart.Render();

            Plot();
        }

        private void MoveUpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            moveUp(ChartName, Index);
        }

        private void MoveDownButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            moveDown(ChartName, Index);
        }

        private void Plot()
        {
            Random random = new Random();
            double[] data = new double[10];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = random.NextDouble();
            }

            ScottPlotChart.plt.PlotSignal(ys: data,
                                          color: ColorManager.GetChartColor.ToString().ConvertToChartColor(),
                                          lineWidth: 1,
                                          markerSize: 1);
            ScottPlotChart.plt.Legend();
            ScottPlotChart.Render();
        }
    }
}
