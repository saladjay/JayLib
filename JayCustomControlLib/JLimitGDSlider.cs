using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JayCustomControlLib
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib;assembly=JayCustomControlLib"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:JLimitGDSlider/>
    ///
    /// </summary>
    public class JLimitGDSlider : Control
    {
        static JLimitGDSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JLimitGDSlider), new FrameworkPropertyMetadata(typeof(JLimitGDSlider)));
        }

        #region DependencyObject


        public Brush PenColor
        {
            get { return (Brush)GetValue(PenColorProperty); }
            set { SetValue(PenColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PenColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenColorProperty =
            DependencyProperty.Register("PenColor", typeof(Brush), typeof(JLimitGDSlider), new PropertyMetadata(Brushes.Yellow));



        public SolidColorBrush LineBrush
        {
            get { return (SolidColorBrush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(SolidColorBrush), typeof(JLimitGDSlider), new PropertyMetadata(Brushes.DarkGray));



        public double LineWidth
        {
            get { return (double)GetValue(LineWidthProperty); }
            set { SetValue(LineWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.Register("LineWidth", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0.5d));



        public SolidColorBrush CanvasBrush
        {
            get { return (SolidColorBrush)GetValue(CanvasBrushProperty); }
            set { SetValue(CanvasBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasBrushProperty =
            DependencyProperty.Register("CanvasBrush", typeof(SolidColorBrush), typeof(JLimitGDSlider), new PropertyMetadata(Brushes.Black));



        public IEnumerable<string> AxisTexts
        {
            get { return (IEnumerable<string>)GetValue(AxisTextsProperty); }
            set { SetValue(AxisTextsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisTexts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxisTextsProperty =
            DependencyProperty.Register("AxisTexts", typeof(IEnumerable<string>), typeof(JLimitGDSlider), new PropertyMetadata(null, AxisTextsChanged));

        private static void AxisTextsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider)
            {
                // axis texts
                IEnumerable<string> texts = (IEnumerable<string>)e.NewValue;
                // axis line
                int index = 0;
                List<string> coordinateTexts = new List<string>();
                foreach (var text in texts)
                {
                    if (index == 0)
                    {
                        jLimitGDSlider.OriginalText = text;
                    }
                    else
                    {
                        coordinateTexts.Add(text);
                    }
                    index++;
                }
                jLimitGDSlider.CoordinateTexts = coordinateTexts;
            }
        }


        
        public string OriginalText
        {
            get { return (string)GetValue(OriginalTextProperty); }
            set { SetValue(OriginalTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OriginalText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OriginalTextProperty =
            DependencyProperty.Register("OriginalText", typeof(string), typeof(JLimitGDSlider), new PropertyMetadata("0"));



        public IEnumerable CoordinateTexts
        {
            get { return (IEnumerable)GetValue(CoordinateTextsProperty); }
            set { SetValue(CoordinateTextsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CoordinatePoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoordinateTextsProperty =
            DependencyProperty.Register("CoordinateTexts", typeof(IEnumerable), typeof(JLimitGDSlider), new PropertyMetadata(new string[] { "1", "2", "3" }));



        public double Threshold
        {
            get { return (double)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Threshold.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d, OnThresholdChanged, OnThresholdExamination));

        private static object OnThresholdExamination(DependencyObject d, object baseValue)
        {
            JLimitGDSlider jLimitGDSlider = d as JLimitGDSlider;
            return Math.Max(jLimitGDSlider.MinThreshold, Math.Min((double)baseValue, jLimitGDSlider.MaxThreshold));
        }

        private static void OnThresholdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider)
            {
                double threshold = (double)e.NewValue;
                jLimitGDSlider._PercentOffset = (threshold - jLimitGDSlider.MinThreshold) / (jLimitGDSlider.MaxThreshold - jLimitGDSlider.MinThreshold);
                jLimitGDSlider.CalculateCoordinate();
            }
        }

        public double MinThreshold
        {
            get { return (double)GetValue(MinThresholdProperty); }
            set { SetValue(MinThresholdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinThreshold.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinThresholdProperty =
            DependencyProperty.Register("MinThreshold", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double MaxThreshold
        {
            get { return (double)GetValue(MaxThresholdProperty); }
            set { SetValue(MaxThresholdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxThreshold.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxThresholdProperty =
            DependencyProperty.Register("MaxThreshold", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public int MaxThresholdIndex
        {
            get { return (int)GetValue(MaxThresholdIndexProperty); }
            set { SetValue(MaxThresholdIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxThresholdIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxThresholdIndexProperty =
            DependencyProperty.Register("MaxThresholdIndex", typeof(int), typeof(JLimitGDSlider), new PropertyMetadata(0, OnMaxThresholdIndexChanged, OnMaxThresholdIndexExamination));

        private static object OnMaxThresholdIndexExamination(DependencyObject d, object baseValue)
        {
            if (d is JLimitGDSlider jLimitGDSlider && (int)baseValue >= 0)
            {
                return baseValue;
            }
            else
            {
                throw new Exception();
            }
        }

        private static void OnMaxThresholdIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public int ThresholdIndex
        {
            get { return (int)GetValue(ThresholdIndexProperty); }
            set { SetValue(ThresholdIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThresholdIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThresholdIndexProperty =
            DependencyProperty.Register("ThresholdIndex", typeof(int), typeof(JLimitGDSlider), new PropertyMetadata(0, OnThresholdIndexChanged, OnThresholdIndexExamination));

        private static object OnThresholdIndexExamination(DependencyObject d, object baseValue)
        {
            JLimitGDSlider jLimitGDSlider = d as JLimitGDSlider;
            return Math.Max(0, Math.Min(jLimitGDSlider.MaxThresholdIndex, (int)baseValue));
        }

        private static void OnThresholdIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider)
            {
                int thresholdIndex = (int)e.NewValue;
                jLimitGDSlider._PercentOffset = (double)thresholdIndex / (double)jLimitGDSlider.MaxThresholdIndex;
                jLimitGDSlider.CalculateCoordinate();
            }
        }

        public double Ratio
        {
            get { return (double)GetValue(RatioProperty); }
            set { SetValue(RatioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ratio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatioProperty =
            DependencyProperty.Register("Ratio", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(1d, OnRaioChanged));

        private static void OnRaioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider && (double)e.NewValue >= 0)
            {
                jLimitGDSlider._Ratio = (double)e.NewValue;
            }
        }

        public int RatioIndex
        {
            get { return (int)GetValue(RatioIndexProperty); }
            set { SetValue(RatioIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ratio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatioIndexProperty =
            DependencyProperty.Register("RatioIndex", typeof(int), typeof(JLimitGDSlider), new PropertyMetadata(0, OnRatioIndexChanged, OnRatioIndexExamination));

        private static void OnRatioIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider && (int)e.NewValue != -1)
            {
                jLimitGDSlider._Ratio = jLimitGDSlider._RatioList[(int)e.NewValue];
                jLimitGDSlider.CalculateCoordinate();
            }
        }

        private static object OnRatioIndexExamination(DependencyObject d, object baseValue)
        {
            JLimitGDSlider jLimitGDSlider = d as JLimitGDSlider;
            if (jLimitGDSlider._RatioList == null)
            {
                return -1;
            }
            return Math.Max(0, Math.Min((int)baseValue, jLimitGDSlider._RatioList.Count - 1));
        }


        public IEnumerable<string> RatioStrings
        {
            get { return (IEnumerable<string>)GetValue(RatioStringsProperty); }
            set { SetValue(RatioStringsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RatioStrings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RatioStringsProperty =
            DependencyProperty.Register("RatioStrings", typeof(IEnumerable<string>), typeof(JLimitGDSlider), new PropertyMetadata(null, OnRatioStringsChanged));

        private static void OnRatioStringsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLimitGDSlider jLimitGDSlider && e.NewValue is IEnumerable<string> ratioStrings)
            {
                List<double> lists = new List<double>();
                foreach (var ratioString in ratioStrings)
                {
                    if (jLimitGDSlider.RatioStringExamination(ratioString, out double res))
                    {
                        lists.Add(res);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                jLimitGDSlider._RatioList = lists;
            }
        }

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double X3
        {
            get { return (double)GetValue(X3Property); }
            set { SetValue(X3Property, value); }
        }

        // Using a DependencyProperty as the backing store for X3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X3Property =
            DependencyProperty.Register("X3", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public double Y3
        {
            get { return (double)GetValue(Y3Property); }
            set { SetValue(Y3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Y3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y3Property =
            DependencyProperty.Register("Y3", typeof(double), typeof(JLimitGDSlider), new PropertyMetadata(0d));



        public LimitType LimitType
        {
            get { return (LimitType)GetValue(LimitTypeProperty); }
            set { SetValue(LimitTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitTypeProperty =
            DependencyProperty.Register("LimitType", typeof(LimitType), typeof(JLimitGDSlider), new PropertyMetadata(default(LimitType)));


        #endregion

        #region construction
        public JLimitGDSlider()
        {
            X1 = Y1 = 0d;
            X2 = Y2 = 50d;
            X3 = Y3 = 100d;
        }
        #endregion

        #region private method
        private void CalculateCoordinate()
        {
            CalculateKValue(_Ratio);
            switch (LimitType)
            {
                case LimitType.Gate:
                    Gate();
                    break;
                case LimitType.Compressor:
                    Compressor();
                    break;
                default:
                    break;
            }
        }
        private void Compressor()
        {
            X1 = 0d;
            Y1 = 0d;
            X2 = _PercentOffset * 100d;
            Y2 = X2;
            X3 = 100d;
            Y3 = Y2 + _K * (X3 - X2);
        }

        private void Gate()
        {
            X1 = 100d;
            Y1 = 100d;
            X2 = _PercentOffset * 100d;
            Y2 = X2;
            X3 = _K == double.PositiveInfinity ? X2 : (_K - 1) / _K * X2;
            Y3 = 0d;
        }

        private void CalculateKValue(double ratio)
        {
            switch (LimitType)
            {
                case LimitType.Gate:
                    _K = ratio > 1 ? ratio : 1 / ratio;
                    break;
                case LimitType.Compressor:
                    _K = ratio < 1 ? ratio : 1 / ratio;
                    break;
                default:
                    break;
            }
        }

        private bool CalculateRatio(string[] ratioStrings, out double res)
        {
            res = 0;
            if (ratioStrings.Length == 2)
            {
                if (double.TryParse(ratioStrings[0], System.Globalization.NumberStyles.Float,CultureInfo.InvariantCulture, out double dividend) && double.TryParse(ratioStrings[1], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out double divisor))
                {
                    res = dividend / divisor;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool RatioStringExamination(string expression, out double result)
        {
            result = 0d;
            if (Regex.IsMatch(expression, _ExpressionRegex))
            {
                string[] numbers = expression.Split(':');
                if (numbers.Length == 2 & Regex.IsMatch(numbers[0], _PositiveNumberRegex) && Regex.IsMatch(numbers[1], _PositiveNumberRegex))
                {
                    if (CalculateRatio(numbers, out double res))
                    {
                        result = res;
                    }
                }
            }
            else if (Regex.IsMatch(expression, _PositiveNumberRegex))
            {
                if (double.TryParse(expression, out double res))
                {
                    result = res;
                }
            }
            else if (Regex.IsMatch(expression, _LimitRegex))
            {
                result = 0d;
                return true;
            }
            return result != 0;
        }
        

        #endregion

        #region private field
        private string _PositiveNumberRegex = "^[0-9]+\\.{1}[0-9]*[1-9]+[0-9]*$|^[1-9]+\\.{1}[0-9]+|^[1-9]{1}[0-9]*$";
        private string _ExpressionRegex = "^[^:]+:{1}[^:]+$";
        private string _LimitRegex = "^[Ll][Ii][Mm][Ii][Tt]$";
        private List<double> _RatioList = new List<double>();
        private double _Ratio=1d;
        private double _K;
        private double _PercentOffset;
        #endregion
    }

    public enum LimitType
    {
        Gate,
        Compressor,
    }
}
