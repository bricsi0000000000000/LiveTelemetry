using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using UI.CustomEventArguments;

namespace UI.CustomControls
{
    public partial class RangeSlider : UserControl
    {
        private double previousUpperValue = 0;
        private double previousLowerValue = 0;

        public RangeSlider()
        {
            InitializeComponent();

            Loaded += RangeSlider_Loaded;
        }

        public delegate void ValueChange(RangeSliderEventArgs rangeSliderEventArgs);
        public event ValueChange ValueChanged;

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(1d));
        public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));
        public static readonly DependencyProperty UpperValueProperty = DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double LowerValue
        {
            get => (double)GetValue(LowerValueProperty);
            set => SetValue(LowerValueProperty, value);
        }

        public double UpperValue
        {
            get => (double)GetValue(UpperValueProperty);
            set => SetValue(UpperValueProperty, value);
        }

        private void RangeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            UpperSlider.ValueChanged += UpperSlider_ValueChanged;
        }

        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value);

            InvokeValueChanged(RangeSliderSide.Left);
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSlider.Value = Math.Min(UpperSlider.Value, LowerSlider.Value);

            InvokeValueChanged(RangeSliderSide.Right);
        }

        private void InvokeValueChanged(RangeSliderSide side)
        {
            switch (side)
            {
                case RangeSliderSide.Left:
                    if (previousLowerValue != LowerValue)
                    {
                        ValueChanged?.Invoke(new RangeSliderEventArgs
                        {
                            LowerValue = LowerValue,
                            UpperValue = UpperValue,
                            Side = side
                        });

                        previousUpperValue = UpperValue;
                    }
                    break;
                case RangeSliderSide.Right:
                    if (previousUpperValue != UpperValue)
                    {
                        ValueChanged?.Invoke(new RangeSliderEventArgs
                        {
                            LowerValue = LowerValue,
                            UpperValue = UpperValue,
                            Side = side
                        });

                        previousUpperValue = UpperValue;
                    }
                    break;
            }
            
        }
    }
}
