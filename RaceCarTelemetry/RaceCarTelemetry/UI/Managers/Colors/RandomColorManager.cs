using System.Windows.Media;
using UI.Extensions;

namespace UI.Managers
{
    public static partial class ColorManager
    {
        private static Color[] ChartColors => new Color[]
        {
            "#fc0505".ConvertColor(),
            "#fc7c05".ConvertColor(),
            "#fce705".ConvertColor(),
            "#80fc05".ConvertColor(),
            "#05fcf4".ConvertColor(),
            "#8005fc".ConvertColor(),
            "#e305fc".ConvertColor()
        };

        private static int chartColorIndex = 0;

        private static int ChartColorIndex
        {
            get
            {
                return chartColorIndex;
            }
            set
            {
                if (value >= ChartColors.Length - 1)
                {
                    value = 0;
                }
                chartColorIndex = value;
            }
        }

        public static Color GetGetChartColor
        {
            get
            {
                return ChartColors[ChartColorIndex++];
            }
        }
    }
}
