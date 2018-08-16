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


namespace JayCustomControlLib.Panels
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib.Themes"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib.Themes;assembly=JayCustomControlLib.Themes"
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
    ///     <MyNamespace:TickMarkPanel/>
    ///
    /// </summary>
    public class TickMarkPanel : Panel
    {


        public DoubleCollection TickCoordinates
        {
            get { return (DoubleCollection)GetValue(TickCoordinatesProperty); }
            set { SetValue(TickCoordinatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickCoordinates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickCoordinatesProperty =
            JTickMark.TickCoordinatesProperty.AddOwner(typeof(TickMarkPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            JTickMark.MaximumProperty.AddOwner(typeof(TickMarkPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            JTickMark.OrientationProperty.AddOwner(typeof(TickMarkPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsArrange));



        #region Override methods
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Maximum <= 0||TickCoordinates==null)
            {
                return finalSize;
            }
            Rect lastRect = new Rect();
            double x = 0, y = 0, width = 0, height = 0;
            UIElementCollection elementCollection = this.InternalChildren;
            if (elementCollection.Count == 0)
            {
                return finalSize;
            }
            List<UIElement> elements = new List<UIElement>();
            for (int i = 0; i < elementCollection.Count; i++)
            {
                if (elementCollection[i].Visibility != Visibility.Collapsed)
                {
                    elements.Add(elementCollection[i]);
                }
            }
            if (elements.Count == 0)
            {
                return finalSize;
            }
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        for (int i = 0; i < elements.Count && i < TickCoordinates.Count && TickCoordinates[i] <= Maximum; i++)
                        {
                            double percentOffset = TickCoordinates[i] / Maximum;
                            if (i == 0)
                            {
                                x = percentOffset * finalSize.Width - elements[i].DesiredSize.Width / 2;
                                y = 0;
                                width = elements[i].DesiredSize.Width;
                                height = elements[i].DesiredSize.Height;
                            }
                            else
                            {
                                x = Math.Max(percentOffset * finalSize.Width - elements[i].DesiredSize.Width / 2, lastRect.X + lastRect.Width);
                                y = 0;
                                width = elements[i].DesiredSize.Width;
                                height = elements[i].DesiredSize.Height;
                            }
                            lastRect = new Rect(x, y, width, height);
                            elements[i].Arrange(lastRect);
                        }
                    }
                    break;
                case Orientation.Vertical:
                    {
                        for (int i = 0; i < elements.Count && i < TickCoordinates.Count && TickCoordinates[i] <= Maximum; i++)
                        {
                            double percentOffset = TickCoordinates[i] / Maximum;
                            if (i == 0)
                            {
                                x = 0;
                                y = percentOffset * finalSize.Height - elements[i].DesiredSize.Height / 2;
                                width = elements[i].DesiredSize.Width;
                                height = elements[i].DesiredSize.Height;
                            }
                            else
                            {
                                x = 0;
                                y = Math.Max(percentOffset * finalSize.Height - elements[i].DesiredSize.Height / 2, lastRect.Y + lastRect.Height);
                                width = elements[i].DesiredSize.Width;
                                height = elements[i].DesiredSize.Height;
                            }
                            lastRect = new Rect(x, y, width, height);
                            elements[i].Arrange(lastRect);
                        }
                    }
                    break;
                default:
                    break;
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size finalSize = new Size();
            UIElementCollection elementCollection = this.InternalChildren;
            if (elementCollection.Count == 0)
            {
                return new Size(0, 0);
            }
            List<UIElement> elements = new List<UIElement>();
            for (int i = 0; i < elementCollection.Count; i++)
            {
                if (elementCollection[i].Visibility != Visibility.Collapsed)
                {
                    elements.Add(elementCollection[i]);
                }
            }
            if (elements.Count == 0)
            {
                return new Size(0, 0);
            }

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        foreach (var element in elements)
                        {
                            element.Measure(availableSize);
                            Size elementSize = element.DesiredSize;
                            finalSize.Width += elementSize.Width;
                            finalSize.Height = Math.Max(finalSize.Height, elementSize.Height);
                        }
                        break;
                    }
                case Orientation.Vertical:
                    {
                        foreach (var element in elements)
                        {
                            element.Measure(availableSize);
                            Size elementSize = element.DesiredSize;
                            finalSize.Height += elementSize.Height;
                            finalSize.Width = Math.Max(finalSize.Width, elementSize.Width);
                        }
                        break;
                    }
                default:
                    break;
            }
            return finalSize;
        }
        #endregion Override methods
    }
}
