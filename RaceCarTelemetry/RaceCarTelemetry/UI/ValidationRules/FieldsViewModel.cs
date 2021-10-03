using System;
using System.ComponentModel;

namespace UI.ValidationRules
{
    public class FieldsViewModel : INotifyPropertyChanged
    {
#nullable enable
        private string? name;
        private string? groupName;
        private string? attributeName;
        private string? channelName;
        private string? addGroupName;
        private string? addAttributeName;
        private int addAttributeLineWidth;
        private string? formula;
        private string? driverlessHorizontalAxis;
        private string? driverlessC0refChannel;
        private string? driverlessYChannel;
        private int lineWidth;
        private string? fileName;
        private string? horizontalAxis;
        private string? changeLineWidth;
        private string? sessionName;
        private string? sessionDate;
        private string? pageTemplateName;
        private string? addPageTemplateName;

        public string? Name
        {
            get => name;
            set => this.MutateVerbose(ref name, value, RaisePropertyChanged());
        }

        public string? GroupName
        {
            get => groupName;
            set => this.MutateVerbose(ref groupName, value, RaisePropertyChanged());
        }

        public string? AttributeName
        {
            get => attributeName;
            set => this.MutateVerbose(ref attributeName, value, RaisePropertyChanged());
        }

        public string? ChannelName
        {
            get => channelName;
            set => this.MutateVerbose(ref channelName, value, RaisePropertyChanged());
        }

        public string? AddGroupName
        {
            get => addGroupName;
            set => this.MutateVerbose(ref addGroupName, value, RaisePropertyChanged());
        }

        public string? AddAttributeName
        {
            get => addAttributeName;
            set => this.MutateVerbose(ref addAttributeName, value, RaisePropertyChanged());
        }

        public int AddAttributeLineWidth
        {
            get => addAttributeLineWidth;
            set => this.MutateVerbose(ref addAttributeLineWidth, value, RaisePropertyChanged());
        }

        public string? Formula
        {
            get => formula;
            set => this.MutateVerbose(ref formula, value, RaisePropertyChanged());
        }

        public string? DriverlessHorizontalAxis
        {
            get => driverlessHorizontalAxis;
            set => this.MutateVerbose(ref driverlessHorizontalAxis, value, RaisePropertyChanged());
        }

        public string? DriverlessC0refChannel
        {
            get => driverlessC0refChannel;
            set => this.MutateVerbose(ref driverlessC0refChannel, value, RaisePropertyChanged());
        }

        public string? DriverlessYChannel
        {
            get => driverlessYChannel;
            set => this.MutateVerbose(ref driverlessYChannel, value, RaisePropertyChanged());
        }

        public int LineWidth
        {
            get => lineWidth;
            set => this.MutateVerbose(ref lineWidth, value, RaisePropertyChanged());
        }

        public string? FileName
        {
            get => fileName;
            set => this.MutateVerbose(ref fileName, value, RaisePropertyChanged());
        }

        public string? HorizontalAxis
        {
            get => horizontalAxis;
            set => this.MutateVerbose(ref horizontalAxis, value, RaisePropertyChanged());
        }

        public string? ChangeLineWidth
        {
            get => changeLineWidth;
            set => this.MutateVerbose(ref changeLineWidth, value, RaisePropertyChanged());
        }

        public string? SessionName
        {
            get => sessionName;
            set => this.MutateVerbose(ref sessionName, value, RaisePropertyChanged());
        }

        public string? PageTemplateName
        {
            get => pageTemplateName;
            set => this.MutateVerbose(ref pageTemplateName, value, RaisePropertyChanged());
        }

        public string? AddPageTemplateName
        {
            get => addPageTemplateName;
            set => this.MutateVerbose(ref addPageTemplateName, value, RaisePropertyChanged());
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged() => args => PropertyChanged?.Invoke(this, args);
    }
}
