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
    public class HierarchicalPanel : Panel
    {
        #region Propertys

        #region Dock
        public static Dock GetDock(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return (Dock)obj.GetValue(DockPanel.DockProperty);
        }

        public static void SetDock(DependencyObject obj, Dock value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            obj.SetValue(DockPanel.DockProperty, (object)obj);
        }

        // Using a DependencyProperty as the backing store for Dock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.RegisterAttached("Dock", typeof(Dock), typeof(HierarchicalPanel), new PropertyMetadata(Dock.Left,new PropertyChangedCallback(OnDockChanged)));

        private static void OnDockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement uiElement) || !(VisualTreeHelper.GetParent((DependencyObject)uiElement) is DockPanel parent))
                return;
            parent.InvalidateMeasure();
        }
        #endregion

        #region DockLevel
        
        public static DockLevel GetDockLevel(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return (DockLevel)obj.GetValue(DockLevelProperty);
        }

        public static void SetDockLevel(DependencyObject obj, DockLevel value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            obj.SetValue(DockLevelProperty, value);
        }

        // Using a DependencyProperty as the backing store for DockLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DockLevelProperty =
            DependencyProperty.RegisterAttached("DockLevel", typeof(DockLevel), typeof(HierarchicalPanel), new PropertyMetadata(DockLevel.Low,OnDockLevelChanged));

        private static void OnDockLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement uiElement) || !(VisualTreeHelper.GetParent((DependencyObject)uiElement) is DockPanel parent))
                return;
            parent.InvalidateMeasure();
        }
        
        #endregion

        #endregion Propertys

        #region enum
        public enum DockLevel
        {
            High,
            Low
        }
        #endregion

        #region Private methods
        private void ReinvalidateMeasure()
        {
            this.InvalidateMeasure();
        }

        #endregion Private methods

        #region Override methods

        /// <summary>
        /// Measure children sizes before arrange children position.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            UIElementCollection internalChildren = this.InternalChildren;
            double val1_1 = 0.0;
            double val1_2 = 0.0;
            double val2_1 = 0.0;
            double val2_2 = 0.0;
            int index = 0;
            for (int count = internalChildren.Count; index < count; ++index)
            {
                UIElement element = internalChildren[index];
                if (element != null && element.Visibility != Visibility.Collapsed)
                {
                    Size availableSize = new Size(Math.Max(0.0, constraint.Width - val2_1), Math.Max(0.0, constraint.Height - val2_2));
                    element.Measure(availableSize);
                    Size desiredSize = element.DesiredSize;
                    switch (HierarchicalPanel.GetDock(element))
                    {
                        case Dock.Left:
                        case Dock.Right:
                            val1_2 = Math.Max(val1_2, val2_2 + desiredSize.Height);
                            val2_1 += desiredSize.Width;
                            continue;
                        case Dock.Top:
                        case Dock.Bottom:
                            val1_1 = Math.Max(val1_1, val2_1 + desiredSize.Width);
                            val2_2 += desiredSize.Height;
                            continue;
                        default:
                            continue;
                    }
                }
            }
            return new Size(Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
        }

        /// <summary>
        /// Arrange children position after measure children size.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            UIElementCollection internalChildren = this.InternalChildren;
            int count = internalChildren.Count;


            double width = arrangeSize.Width;
            double height = arrangeSize.Height;
            double highLevelCount = 0;
            for (int i = 0; i < count; i++)
            {
                UIElement element = InternalChildren[i];
                if (element != null && element.Visibility != Visibility.Collapsed)
                {
                    Size desiredSize = element.DesiredSize;
                    DockLevel dockLevel = HierarchicalPanel.GetDockLevel(element);
                    if (dockLevel == DockLevel.Low)
                    {
                        width -= desiredSize.Width;
                        height -= desiredSize.Height;
                    }
                    else
                    {
                        highLevelCount++;
                    }
                }
            }

            double x = 0.0;
            double y = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            for (int index = 0; index < count; ++index)
            {
                UIElement element = internalChildren[index];
                if (element != null && element.Visibility != Visibility.Collapsed)
                {
                    Size desiredSize = element.DesiredSize;
                    Rect finalRect = new Rect(x, y, Math.Max(0.0, arrangeSize.Width - (x + num2)), Math.Max(0.0, arrangeSize.Height - (y + num3)));
                    if (HierarchicalPanel.GetDockLevel(element) == DockLevel.Low)
                    {
                        switch (DockPanel.GetDock(element))
                        {
                            case Dock.Left:
                                x += desiredSize.Width;
                                finalRect.Width = desiredSize.Width;
                                break;
                            case Dock.Top:
                                y += desiredSize.Height;
                                finalRect.Height = desiredSize.Height;
                                break;
                            case Dock.Right:
                                num2 += desiredSize.Width;
                                finalRect.X = Math.Max(0.0, arrangeSize.Width - num2);
                                finalRect.Width = desiredSize.Width;
                                break;
                            case Dock.Bottom:
                                num3 += desiredSize.Height;
                                finalRect.Y = Math.Max(0.0, arrangeSize.Height - num3);
                                finalRect.Height = desiredSize.Height;
                                break;
                        }
                    }
                    else
                    {
                        switch (DockPanel.GetDock(element))
                        {
                            case Dock.Left:
                                x += width / highLevelCount;
                                finalRect.Width = width / highLevelCount;
                                break;
                            case Dock.Top:
                                y += height / highLevelCount;
                                finalRect.Height = height / highLevelCount;
                                break;
                            case Dock.Right:
                                num2 += width / highLevelCount;
                                finalRect.X = Math.Max(0.0, arrangeSize.Width - num2);
                                finalRect.Width = desiredSize.Width;
                                break;
                            case Dock.Bottom:
                                num3 += height / highLevelCount;
                                finalRect.Y = Math.Max(0.0, arrangeSize.Height - num3);
                                finalRect.Height = desiredSize.Height;
                                break;
                        }
                    }
                    element.Arrange(finalRect);
                }
            }
            return arrangeSize;
        }
        #endregion Override methods
    }
}
