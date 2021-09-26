using System;

namespace UI.CustomEventArguments
{
    public enum RangeSliderSide
    {
        Left,
        Right
    }
    public class RangeSliderEventArgs : EventArgs
    {
        public double LowerValue { get; set; }
        public double UpperValue { get; set; }
        public RangeSliderSide Side { get; set; }
    }
}
