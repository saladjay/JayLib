using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:JMeter/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_ActiveRect", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PART_RightActiveRect", Type = typeof(Rectangle))]
    public class JMeter : Control
    {
        static JMeter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JMeter), new FrameworkPropertyMetadata(typeof(JMeter)));
        }

        public IEnumerable TextCollection
        {
            get { return (IEnumerable)GetValue(TextCollectionProperty); }
            set { SetValue(TextCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextCollectionProperty =
            DependencyProperty.Register("TextCollection", typeof(IEnumerable), typeof(JMeter), new PropertyMetadata(new string[] { "0", "1", "2", "3", "4" }, OnTextCollectionChanged));

        private static void OnTextCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JMeter meter)
            {
                int count = 0;
                foreach (var value in meter.TextCollection)
                {
                    count++;
                }
                meter.Maximum = count;
            }
        }

        public Brush BorderRimBrush
        {
            get { return (Brush)GetValue(BorderRimBrushProperty); }
            set { SetValue(BorderRimBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderRimBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderRimBrushProperty =
            DependencyProperty.Register("BorderRimBrush", typeof(Brush), typeof(JMeter), new PropertyMetadata(Brushes.Black));

        
        public Thickness Thickness
        {
            get { return (Thickness)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(Thickness), typeof(JMeter), new PropertyMetadata(new Thickness(1)));
        

        public double BoxesWidth
        {
            get { return (double)GetValue(BoxesWidthProperty); }
            set { SetValue(BoxesWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxesWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxesWidthProperty =
            DependencyProperty.Register("BoxesWidth", typeof(double), typeof(JMeter), new PropertyMetadata(5d));


        public bool IsDockRight
        {
            get { return (bool)GetValue(IsDockRightProperty); }
            set { SetValue(IsDockRightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDockRight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDockRightProperty =
            DependencyProperty.Register("IsDockRight", typeof(bool), typeof(JMeter), new PropertyMetadata(true));
        

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(JMeter), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged, CoerceValueChanged));

        private static object CoerceValueChanged(DependencyObject d, object baseValue)
        {
            if (d is JMeter meter && baseValue is int value)
            {
                if (value > meter.Maximum || value < 0)
                {
                    return meter.Value;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return 0;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JMeter meter && e.NewValue is int value)
            {
                double percentValue = (double)value / meter.Maximum;
                double height = meter.ActualHeight;
                if (meter.ActiveColorRectangle != null)
                {
                    meter.ActiveColorRectangle.Height = percentValue * height;
                    meter.RightActiveColorRectangle.Height = percentValue * height;
                }
            }
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(JMeter), new PropertyMetadata(5));



        public Brush ActiveBrush
        {
            get { return (Brush)GetValue(ActiveBrushProperty); }
            set { SetValue(ActiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(Brush), typeof(JMeter), new FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.AffectsRender));



        public Brush InactiveBrush
        {
            get { return (Brush)GetValue(InactiveBrushProperty); }
            set { SetValue(InactiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveBrushProperty =
            DependencyProperty.Register("InactiveBrush", typeof(Brush), typeof(JMeter), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));
        

        public bool IsMainMeter
        {
            get { return (bool)GetValue(IsMainMeterProperty); }
            set { SetValue(IsMainMeterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMainMeter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMainMeterProperty =
            DependencyProperty.Register("IsMainMeter", typeof(bool), typeof(JMeter), new PropertyMetadata(false));
        

        public Rectangle ActiveColorRectangle
        {
            get => _activeColorRectangle;
            set => _activeColorRectangle = value;
        }

        public Rectangle RightActiveColorRectangle
        {
            get => _rightActiveColorRectangle;
            set => _rightActiveColorRectangle = value;
        }

        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActiveColorRectangle = GetTemplateChild(ActiveColorControl) as Rectangle;
            RightActiveColorRectangle = GetTemplateChild(ActiveRightColorControl) as Rectangle;
        }
        #endregion

        #region Private fields
        private const string ActiveColorControl = "PART_ActiveRect";
        private const string ActiveRightColorControl = "PART_RightActiveRect";
        private Rectangle _activeColorRectangle;
        private Rectangle _rightActiveColorRectangle;
        #endregion
    }
}
