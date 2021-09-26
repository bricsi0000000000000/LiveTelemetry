using System.Windows.Controls;
using UI.Extensions;

namespace UI.UserControls.Charts
{
    public partial class ChartValueItem : UserControl
    {
        private readonly string groupName;

        private string colorCode;
        private string attributeName;
        private int inputFileId;

        public ChartValueItem(string channelName, string unitOfMeasure, string groupName)
        {
            InitializeComponent();

            NameLabel.Opacity = .4f;
            ValueLabel.Opacity = .4f;
            UnitOfMeasureLabel.Opacity = .4f;

            this.groupName = groupName;
            AttributeName = channelName;
            UnitOfMeasureLabel.Content = unitOfMeasure;

            SetAttributeValue(0);
        }


        public string AttributeName
        {
            get
            {
                return attributeName;
            }
            set
            {
                attributeName = value;

                SetAttributeName(attributeName);
            }
        }

        private void SetAttributeName(string name)
        {
            if (inputFileId != -1)
            {
                NameLabel.Content = $"{name} {inputFileId}";
            }
            else
            {
                NameLabel.Content = name;
            }
        }

        public void SetAttributeValue(double value)
        {
            if (inputFileId != -1)
            {
                ValueLabel.Content = $"{value:f3}";
            }
        }

        public void SetLiveAttributeValue(double value)
        {
            ValueLabel.Content = $"{value:f3}";
        }

        public void SetUp(string colorText, int inputFileId)
        {
            NameLabel.Opacity = 1;
            ValueLabel.Opacity = 1;
            UnitOfMeasureLabel.Opacity = 1;

            this.inputFileId = inputFileId;
            colorCode = colorText;
            ColorCard.Background = colorText.ConvertBrush();

            SetAttributeName(attributeName);
        }
    }
}
