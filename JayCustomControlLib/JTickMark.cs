using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    ///     <MyNamespace:JTickMark/>
    ///
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListDisplayItem))]
    public class JTickMark : ItemsControl
    {
        static JTickMark()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JTickMark), new FrameworkPropertyMetadata(typeof(JTickMark)));
            ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            itemsPanelTemplate.Seal();
            ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(JTickMark), (PropertyMetadata)new FrameworkPropertyMetadata((object)itemsPanelTemplate));
        }

        
        public DoubleCollection TickCoordinates
        {
            get { return (DoubleCollection)GetValue(TickCoordinatesProperty); }
            set { SetValue(TickCoordinatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickCoordinates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickCoordinatesProperty =
            DependencyProperty.Register("TickCoordinates", typeof(DoubleCollection), typeof(JTickMark), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(JTickMark), new FrameworkPropertyMetadata(0d,FrameworkPropertyMetadataOptions.Inherits));



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(JTickMark), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.Inherits));



        public bool IsResersed
        {
            get { return (bool)GetValue(IsResersedProperty); }
            set { SetValue(IsResersedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResersed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsResersedProperty =
            DependencyProperty.Register("IsResersed", typeof(bool), typeof(JTickMark), new PropertyMetadata(false,OnIsReseredChanged));

        private static void OnIsReseredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JTickMark tickMark&&tickMark.TickTexts!=null)
            {
                List<string> strings = tickMark.TickTexts.ToList();
                List<string> tickStrings = new List<string>();
                DoubleCollection tickCoordinates = new DoubleCollection();
                if (tickMark.IsResersed)
                {
                    strings.Reverse();
                }
                int index = 0;
                foreach (var str in strings)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {

                    }
                    else
                    {
                        tickStrings.Add(str);
                        tickCoordinates.Add(index);
                    }
                    index++;
                }
                tickMark.Maximum = index;
                tickMark.ItemsSource = tickStrings;
                tickMark.TickCoordinates = tickCoordinates;
            }
        }

        public IEnumerable<string> TickTexts
        {
            get { return (IEnumerable<string>)GetValue(TickTextsProperty); }
            set { SetValue(TickTextsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickTexts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickTextsProperty =
            JSlider.TickTextsProperty.AddOwner(typeof(JTickMark), new FrameworkPropertyMetadata(default(IEnumerable<string>), FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnTickTextsChanged)));

        private static void OnTickTextsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JTickMark tickMark &&e.NewValue is IEnumerable<string> texts && texts != null && texts.Count() != 0)
            {
                List<string> strings = texts.ToList();
                List<string> tickStrings = new List<string>();
                DoubleCollection tickCoordinates = new DoubleCollection();
                if (tickMark.IsResersed)
                {
                    strings.Reverse();
                }
                int index = 0;
                foreach (var str in strings)
                {
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        
                    }
                    else
                    {
                        tickStrings.Add(str);
                        tickCoordinates.Add(index);
                    }
                    index++;
                }
                tickMark.Maximum = index;
                tickMark.ItemsSource = tickStrings;
                tickMark.TickCoordinates = tickCoordinates;
            }
        }
    }
}
