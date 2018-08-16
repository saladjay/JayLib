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
    ///     <MyNamespace:LedSlider/>
    ///
    /// </summary>
    public class LedSlider : Control
    {
        static LedSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LedSlider), new FrameworkPropertyMetadata(typeof(LedSlider)));
        }
        public LedSlider()
        {
            this.Focusable = true;
        }


        #region Event

        public delegate void SliderValueChangedEventHandler(Object sender, int value);
        public event SliderValueChangedEventHandler SliderValueChanged;

        #endregion


        #region Propertys

        #endregion Propertys
        public SolidColorBrush BackgroundBrush
        {
            get { return (SolidColorBrush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register("BackgroundBrush", typeof(SolidColorBrush), typeof(LedSlider), new PropertyMetadata(Brushes.Black));

        public SolidColorBrush LedgroundBrush
        {
            get { return (SolidColorBrush)GetValue(LedgroundBrushProperty); }
            set { SetValue(LedgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty LedgroundBrushProperty =
            DependencyProperty.Register("LedgroundBrush", typeof(SolidColorBrush), typeof(LedSlider), new PropertyMetadata(Brushes.Lime));


        public int MaxLedNum
        {
            get { return (int)GetValue(MaxLedNumProperty); }
            set
            {
                SetValue(MaxLedNumProperty, value);
                this.InvalidateVisual();
            }
        }

        public static readonly DependencyProperty LedNumProperty =
            DependencyProperty.Register("LedNum", typeof(int), typeof(LedSlider), new PropertyMetadata(10,OnLedNumChanged));

        private static void OnLedNumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LedSlider ledSlider)
            {
                ledSlider.InvalidateVisual();
            }
        }

        public int LedNum
        {
            get { return (int)GetValue(LedNumProperty); }
            set { SetValue(LedNumProperty, value); }
        }

        public static readonly DependencyProperty MaxLedNumProperty =
            DependencyProperty.Register("MaxLedNum", typeof(int), typeof(LedSlider), new PropertyMetadata(0));

        

        public int Mar
        {
            get { return (int)GetValue(MarProperty); }
            set
            {
                SetValue(MarProperty, value);
                this.InvalidateVisual();
            }
        }

        public static readonly DependencyProperty MarProperty =
            DependencyProperty.Register("Mar", typeof(int), typeof(LedSlider), new PropertyMetadata(2));

        #region Override methods

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            marAndWidth = (this.ActualWidth - Mar) / (MaxLedNum + 1);
            LedWidth = marAndWidth - Mar;
            dc.DrawRectangle(BackgroundBrush, new Pen(), new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            for (int i = 0; i < MaxLedNum + 1; i++)
            {
                dc.DrawRectangle(Brushes.Gray, new Pen(), new Rect(i * marAndWidth + Mar, Mar, LedWidth, this.ActualHeight - 2 * Mar));
            }

            int half = (MaxLedNum + 1) / 2;

            if (LedNum < half)
            {
                for (int i = LedNum; i < half; i++)
                {
                    dc.DrawRectangle(LedgroundBrush, new Pen(), new Rect(i * marAndWidth + Mar, Mar, LedWidth, this.ActualHeight - 2 * Mar));
                }
            }
            else if (LedNum > half)
            {
                for (int i = half; i <= LedNum; i++)
                {
                    dc.DrawRectangle(LedgroundBrush, new Pen(), new Rect(i * marAndWidth + Mar, Mar, LedWidth, this.ActualHeight - 2 * Mar));

                }
            }
            dc.DrawRectangle(Brushes.Red, new Pen(), new Rect((MaxLedNum + 1) / 2 * marAndWidth + Mar, Mar, LedWidth, this.ActualHeight - 2 * Mar));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.Focus();
            this.CaptureMouse();
            Point pt = e.GetPosition(this);
            int num = (int)((pt.X - Mar) / marAndWidth);
            if (pt.X >= (num * marAndWidth /*+ Mar*/) && pt.X < this.ActualWidth - Mar /*((num + 1) * marAndWidth)*/)
            {
                if (num < 0)
                    num = 0;
                else if (num >= MaxLedNum)
                    num = MaxLedNum;
                isTouched = true;
                SliderValueChanged?.Invoke(this, num);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isTouched) return;
            if (e.LeftButton == MouseButtonState.Released)
            {
                isTouched = false;
                return;
            }
            Point pt = e.GetPosition(this);
            int num = (int)((pt.X - Mar) / marAndWidth);
            if (num < 0)
                num = 0;
            else if (num >= MaxLedNum)
                num = MaxLedNum;
            SliderValueChanged?.Invoke(this, num);

        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            isTouched = false;
            this.ReleaseMouseCapture();
            this.Focus();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!this.IsFocused) return;
            int num = LedNum + e.Delta / 120;
            if (num < 0)
                num = 0;
            else if (num >= MaxLedNum)
                num = MaxLedNum;
            SliderValueChanged?.Invoke(this, num);
        }

        #endregion Override methods

        #region Private fields

        double marAndWidth = 0;
        double LedWidth = 0;
        bool isTouched = false;

        #endregion Private fields

    }
}
