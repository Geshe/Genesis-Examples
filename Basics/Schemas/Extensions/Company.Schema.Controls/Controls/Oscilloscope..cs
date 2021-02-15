using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Editors;

namespace Company.Schema.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class Oscilloscope : Control
    {
        static Oscilloscope()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Oscilloscope), new FrameworkPropertyMetadata(typeof(Oscilloscope)));
        }

        public Oscilloscope()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Tick += new EventHandler(OnTimedEvent);
            timer.Start();
        }

        const double deltaDelay = 0.2;
        const int toolTipOffset = 15;

        double offsetData = -1;
        ArcScaleNeedle selectedNeedle = null;
        DispatcherTimer timer = new DispatcherTimer();

        DevExpress.Xpf.Charts.Range horizontallAxisRange;
        DevExpress.Xpf.Charts.Range verticalAxisRange;
        ConstantLine сonstantLine;

        SecondaryAxisX2D gridAxisX;
        SecondaryAxisY2D gridAxisY;

        LineStepSeries2D lineStepSeries2D;

        CheckEdit enabledSignalUpCheckEdit = null;
        Popup needleTooltip = null;
        TextEdit ttContent = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //
            horizontallAxisRange = (DevExpress.Xpf.Charts.Range)GetTemplateChild("horizontallAxisRange");
            verticalAxisRange = (DevExpress.Xpf.Charts.Range)GetTemplateChild("verticalAxisRange");

            сonstantLine = (ConstantLine)GetTemplateChild("сonstantLine");

            gridAxisX = (SecondaryAxisX2D)GetTemplateChild("gridAxisX");
            gridAxisY = (SecondaryAxisY2D)GetTemplateChild("gridAxisY");

            lineStepSeries2D = (LineStepSeries2D)GetTemplateChild("lineStepSeries2D");

            enabledSignalUpCheckEdit = (CheckEdit)GetTemplateChild("enabledSignalUpCheckEdit");
            needleTooltip = (Popup)GetTemplateChild("needleTooltip");
            ttContent = (TextEdit)GetTemplateChild("ttContent");

            //
            CreateOscilloscopeGrid();
            //
            ArcScaleNeedle verticalPositionNeedle = (ArcScaleNeedle)GetTemplateChild("verticalPositionNeedle");
            verticalPositionNeedle.ValueChanged += VerticalPositionNeedle_ValueChanged;

            ArcScaleNeedle horizontallPositionNeedle = (ArcScaleNeedle)GetTemplateChild("horizontallPositionNeedle");
            horizontallPositionNeedle.ValueChanged += HorizontallPosition_ValueChanged;

            ArcScaleNeedle referenceVoltageNeedle = (ArcScaleNeedle)GetTemplateChild("referenceVoltageNeedle");
            referenceVoltageNeedle.ValueChanged += ReferenceVoltageNeedle_ValueChanged;

            ArcScaleNeedle verticalSensitivityNeedle = (ArcScaleNeedle)GetTemplateChild("verticalSensitivityNeedle");
            verticalSensitivityNeedle.ValueChanged += VerticalSensitivityNeedle_ValueChanged;

            ArcScaleNeedle horizontallSensitivityNeedle = (ArcScaleNeedle)GetTemplateChild("horizontallSensitivityNeedle");
            horizontallSensitivityNeedle.ValueChanged += HorizontallSensitivityNeedle_ValueChanged;


            CircularGaugeControl circularGauge1 = (CircularGaugeControl)GetTemplateChild("circularGauge1");
            circularGauge1.MouseLeftButtonDown += CircularGaugeControl_MouseLeftButtonDown;
            circularGauge1.MouseLeftButtonUp += CircularGaugeControl_MouseLeftButtonUp;
            circularGauge1.MouseMove += CircularGaugeControl_MouseMove;
            circularGauge1.MouseLeave += CircularGaugeControl_MouseLeave;

            CircularGaugeControl circularGauge2 = (CircularGaugeControl)GetTemplateChild("circularGauge2");
            circularGauge2.MouseLeftButtonDown += CircularGaugeControl_MouseLeftButtonDown;
            circularGauge2.MouseLeftButtonUp += CircularGaugeControl_MouseLeftButtonUp;
            circularGauge2.MouseMove += CircularGaugeControl_MouseMove;
            circularGauge2.MouseLeave += CircularGaugeControl_MouseLeave;

            CircularGaugeControl circularGauge3 = (CircularGaugeControl)GetTemplateChild("circularGauge3");
            circularGauge3.MouseLeftButtonDown += CircularGaugeControl_MouseLeftButtonDown;
            circularGauge3.MouseLeftButtonUp += CircularGaugeControl_MouseLeftButtonUp;
            circularGauge3.MouseMove += CircularGaugeControl_MouseMove;
            circularGauge3.MouseLeave += CircularGaugeControl_MouseLeave;

            CircularGaugeControl circularGauge4 = (CircularGaugeControl)GetTemplateChild("circularGauge4");
            circularGauge4.MouseLeftButtonDown += CircularGaugeControl_MouseLeftButtonDown;
            circularGauge4.MouseLeftButtonUp += CircularGaugeControl_MouseLeftButtonUp;
            circularGauge4.MouseMove += CircularGaugeControl_MouseMove;
            circularGauge4.MouseLeave += CircularGaugeControl_MouseLeave;

            CircularGaugeControl circularGauge5 = (CircularGaugeControl)GetTemplateChild("circularGauge5");
            circularGauge5.MouseLeftButtonDown += CircularGaugeControl_MouseLeftButtonDown;
            circularGauge5.MouseLeftButtonUp += CircularGaugeControl_MouseLeftButtonUp;
            circularGauge5.MouseMove += CircularGaugeControl_MouseMove;
            circularGauge5.MouseLeave += CircularGaugeControl_MouseLeave;

            //
            UpdateData();
        }
        

        double HorizontallPosition
        {
            get { return (horizontallAxisRange.ActualMinValueInternal + horizontallAxisRange.ActualMaxValueInternal) / 2; }
            set { horizontallAxisRange.SetMinMaxValues(value - 0.5 * HorizontallSensitivity, value + 0.5 * HorizontallSensitivity); }
        }
        double HorizontallSensitivity
        {
            get { return horizontallAxisRange.ActualMaxValueInternal - horizontallAxisRange.ActualMinValueInternal; }
            set { horizontallAxisRange.SetMinMaxValues(HorizontallPosition - 0.5 * value, HorizontallPosition + 0.5 * value); }
        }
        double VerticalPosition
        {
            get { return (verticalAxisRange.ActualMinValueInternal + verticalAxisRange.ActualMaxValueInternal) / 2; }
            set { verticalAxisRange.SetMinMaxValues(value - 0.5 * VerticalSensitivity, value + 0.5 * VerticalSensitivity); }
        }
        double VerticalSensitivity
        {
            get { return verticalAxisRange.ActualMaxValueInternal - verticalAxisRange.ActualMinValueInternal; }
            set { verticalAxisRange.SetMinMaxValues(VerticalPosition - 0.5 * value, VerticalPosition + 0.5 * value); }
        }
        double TriggerLavel
        {
            get { return Convert.ToDouble(сonstantLine.Value); }
            set { сonstantLine.Value = value; }
        }
        
        void VerticalSensitivityNeedle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (verticalAxisRange != null && Math.Abs(VerticalSensitivity - e.NewValue) >= deltaDelay)
            {
                VerticalSensitivity = e.NewValue;
                UpdateData();
            }
        }
        void HorizontallSensitivityNeedle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (horizontallAxisRange != null && Math.Abs(HorizontallSensitivity - e.NewValue) >= deltaDelay)
            {
                HorizontallSensitivity = e.NewValue;
                UpdateData();
            }
        }
        void VerticalPositionNeedle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (verticalAxisRange != null && Math.Abs(VerticalPosition - e.NewValue) >= deltaDelay)
            {
                VerticalPosition = e.NewValue;
                UpdateData();
            }
        }
        void HorizontallPosition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (horizontallAxisRange != null && Math.Abs(HorizontallPosition - e.NewValue) >= deltaDelay)
            {
                HorizontallPosition = e.NewValue;
                UpdateData();
            }
        }
        void ReferenceVoltageNeedle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (сonstantLine != null && Math.Abs(TriggerLavel - e.NewValue) >= deltaDelay)
            {
                TriggerLavel = e.NewValue;
                UpdateData();
            }
        }
        void OnTimedEvent(object source, EventArgs e)
        {
            UpdateData();
        }
        void CircularGaugeControl_MouseMove(object sender, MouseEventArgs e)
        {
            CircularGaugeControl gauge = (CircularGaugeControl)sender;
            ArcScaleNeedle currentSelectedNeedle = selectedNeedle != null ? selectedNeedle : gauge.CalcHitInfo(e.GetPosition(gauge)).Needle;
            if (currentSelectedNeedle != null)
                ShowTooltip(currentSelectedNeedle, this, e.GetPosition(this));
            else
                HideTooltip();
        }
        void CircularGaugeControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CircularGaugeControl gauge = sender as CircularGaugeControl;
            if (gauge != null)
            {
                CircularGaugeHitInfo hitInfo = gauge.CalcHitInfo(e.GetPosition(gauge));
                if (hitInfo.InNeedle)
                    selectedNeedle = hitInfo.Needle;
            }
        }
        void CircularGaugeControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedNeedle = null;
            HideTooltip();
        }
        void CircularGaugeControl_MouseLeave(object sender, MouseEventArgs e)
        {
            selectedNeedle = null;
            HideTooltip();
        }
        void HideTooltip()
        {
            ttContent.Text = "";
            needleTooltip.IsOpen = false;
            Cursor = Cursors.Arrow;
        }
        void ShowTooltip(ArcScaleNeedle needle, UIElement placementTarget, Point position)
        {
            ttContent.Text = string.Format("Value = {0:F2}", needle.Value);
            needleTooltip.Placement = PlacementMode.RelativePoint;
            needleTooltip.PlacementTarget = placementTarget;
            needleTooltip.HorizontalOffset = position.X + toolTipOffset;
            needleTooltip.VerticalOffset = position.Y + toolTipOffset;
            needleTooltip.IsOpen = true;
            Cursor = Cursors.Hand;
        }
        void UpdateData()
        {
            if (enabledSignalUpCheckEdit != null)
            {
                if (Math.Abs(TriggerLavel) <= 1)
                    offsetData = ((enabledSignalUpCheckEdit.IsChecked.Value) ? (0) : (1));
                else
                {
                    offsetData += 0.5;
                    if (offsetData > 1)
                        offsetData = -1;
                    if (offsetData < -1)
                        offsetData = 1;
                }
                lineStepSeries2D.Points.BeginInit();
                lineStepSeries2D.Points.Clear();
                for (int i = -25; i < 25; i++)
                {
                    double yValue = Math.Abs(i % 2) * 2 - 1;
                    SeriesPoint seriesPoint = new SeriesPoint(i + offsetData, yValue);
                    lineStepSeries2D.Points.Add(seriesPoint);
                }
                lineStepSeries2D.Points.EndInit();
            }
        }
        void CreateOscilloscopeGrid()
        {
            SolidColorBrush majorConstantLineBrush = new SolidColorBrush(Color.FromArgb(0x80, 0x4B, 0xC7, 0xB9));
            SolidColorBrush minorConstantLineBrush = new SolidColorBrush(Color.FromArgb(0x29, 0x4B, 0xC7, 0xB9));
            for (double i = 0.25; i < 4; i += 0.25)
            {
                ConstantLine constantLineX = new ConstantLine();
                ConstantLine constantLineY = new ConstantLine();
                constantLineX.Value = constantLineY.Value = i;
                constantLineX.Brush = constantLineY.Brush = (i / 0.25) % 2 == 0 ? majorConstantLineBrush : minorConstantLineBrush;
                gridAxisX.ConstantLinesBehind.Add(constantLineX);
                gridAxisY.ConstantLinesBehind.Add(constantLineY);
            }
        }
    }
}
