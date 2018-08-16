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
    ///     <MyNamespace:UniformPanel/>
    ///
    /// </summary>
    public class UniformPanel : Panel
    {
        #region Propertys

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UniformPanel), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UniformPanel uniformPanel)
            {
                uniformPanel.ResetUniformPanel();
            }
        }

        public bool IsReversed
        {
            get { return (bool)GetValue(IsReversedProperty); }
            set { SetValue(IsReversedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResersed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReversedProperty =
            DependencyProperty.Register("IsReversed", typeof(bool), typeof(UniformPanel), new PropertyMetadata(false, OnIsResersedChanged));

        private static void OnIsResersedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UniformPanel uniformPanel)
            {
                uniformPanel.ResetUniformPanel();
            }
        }

        #endregion Propertys

        #region Private methods
        private void ResetUniformPanel()
        {
            this.InvalidateMeasure();
        }

        #endregion Private methods

        #region Override methods

        /// <summary>
        /// Arrange children position after measure children size.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect lastRect, uniformRect;
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
            if (IsReversed)
            {
                elements.Reverse();
            }
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        UIElement lastElement = elements.Last();
                        double lastElementOffsetX = Math.Max(0.0, finalSize.Width - lastElement.DesiredSize.Width);
                        lastRect = new Rect(lastElementOffsetX, 0, lastElement.DesiredSize.Width, finalSize.Height);
                        lastElement.Arrange(lastRect);
                        int uniformElementCount = elements.Count - 1;
                        if (uniformElementCount == 0)
                        {
                            break;
                        }
                        uniformRect = new Rect(0, 0, lastElementOffsetX / uniformElementCount, finalSize.Height);
                        for (int i = 0; i < uniformElementCount; i++)
                        {
                            elements[i].Arrange(uniformRect);
                            uniformRect.X += uniformRect.Width;
                        }
                        break;
                    }
                case Orientation.Vertical:
                    {
                        UIElement lastElement = elements.Last();
                        double lastElementOffsetY = Math.Max(0.0, finalSize.Height - lastElement.DesiredSize.Height);
                        lastRect = new Rect(0, lastElementOffsetY, finalSize.Width, lastElement.DesiredSize.Height);
                        lastElement.Arrange(lastRect);
                        int uniformElementCount = elements.Count - 1;
                        if (uniformElementCount == 0)
                        {
                            break;
                        }
                        uniformRect = new Rect(0, 0, finalSize.Width, lastElementOffsetY / uniformElementCount);
                        for (int i = 0; i < uniformElementCount; i++)
                        {
                            elements[i].Arrange(uniformRect);
                            uniformRect.Y += uniformRect.Height;
                        }
                        break;
                    }
                default:
                    break;
            }
            return finalSize;
        }

        /// <summary>
        /// Measure children sizes before arrange children position.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
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
            if (IsReversed)
            {
                elements.Reverse();
            }
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        UIElement lastElement = elements.Last();
                        lastElement.Measure(availableSize);
                        Size lastElementSize = lastElement.DesiredSize;
                        finalSize.Width += lastElementSize.Width;
                        finalSize.Height = Math.Max(lastElementSize.Height, finalSize.Height);
                        int uniformElementCount = elements.Count - 1;
                        if (uniformElementCount == 0)
                        {
                            break;
                        }
                        Size uniformSize = new Size(Math.Max(0.0, ((availableSize.Width - lastElementSize.Width) / uniformElementCount)), availableSize.Height);
                        for (int i = 0; i < uniformElementCount; i++)
                        {
                            elements[i].Measure(uniformSize);
                            Size elementSize = elements[i].DesiredSize;
                            finalSize.Width += elementSize.Width;
                            finalSize.Height = Math.Max(elementSize.Height, finalSize.Height);
                        }
                        break;
                    }
                case Orientation.Vertical:
                    {
                        UIElement lastElement = elements.Last();
                        lastElement.Measure(availableSize);
                        Size lastElementSize = lastElement.DesiredSize;
                        finalSize.Width = Math.Max(lastElementSize.Width, finalSize.Width);
                        finalSize.Height += lastElementSize.Height;
                        int uniformElementCount = elements.Count - 1;
                        if (uniformElementCount == 0)
                        {
                            break;
                        }
                        Size uniformSize = new Size(availableSize.Width, (availableSize.Height - lastElementSize.Height) / uniformElementCount);
                        for (int i = 0; i < uniformElementCount; i++)
                        {
                            elements[i].Measure(uniformSize);
                            Size elementSize = elements[i].DesiredSize;
                            finalSize.Width = Math.Max(elementSize.Width, finalSize.Width);
                            finalSize.Height += elementSize.Height;
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
