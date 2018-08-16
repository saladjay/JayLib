using System;
using System.Collections.Generic;
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

namespace JayCustomControlLib.Panels
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib.Panels"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib.Panels;assembly=JayCustomControlLib.Panels"
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
    ///     <MyNamespace:ReservedUniformGrid/>
    ///
    /// </summary>
    public class ReversedUniformGrid : UniformGrid
    {
        static ReversedUniformGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReversedUniformGrid), new FrameworkPropertyMetadata(typeof(ReversedUniformGrid)));
        }



        public bool IsReversed
        {
            get { return (bool)GetValue(IsReversedProperty); }
            set { SetValue(IsReversedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsResered.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReversedProperty =
            DependencyProperty.Register("IsReversed", typeof(bool), typeof(ReversedUniformGrid), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));



        #region Override methods
        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateComputedValues();

            Size childConstraint = new Size(availableSize.Width / _columns, availableSize.Height / _rows);
            double maxChildDesiredWidth = 0.0;
            double maxChildDesiredHeight = 0.0;
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in InternalChildren)
            {
                list.Add(item);
            }
            if (IsReversed)
            {
                list.Reverse();
            }
            //  Measure each child, keeping track of maximum desired width and height.
            for (int i = 0, count = list.Count; i < count; ++i)
            {
                UIElement child = list[i];

                // Measure the child.
                child.Measure(childConstraint);
                Size childDesiredSize = child.DesiredSize;

                if (maxChildDesiredWidth < childDesiredSize.Width)
                {
                    maxChildDesiredWidth = childDesiredSize.Width;
                }

                if (maxChildDesiredHeight < childDesiredSize.Height)
                {
                    maxChildDesiredHeight = childDesiredSize.Height;
                }
            }

            return new Size((maxChildDesiredWidth * _columns), (maxChildDesiredHeight * _rows));
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Rect childBounds = new Rect(0, 0, arrangeSize.Width / _columns, arrangeSize.Height / _rows);
            double xStep = childBounds.Width;
            double xBound = arrangeSize.Width - 1.0;

            childBounds.X += childBounds.Width * FirstColumn;
            List<UIElement> list = new List<UIElement>();
            foreach (UIElement item in InternalChildren)
            {
                list.Add(item);
            }
            if (IsReversed)
            {
                list.Reverse();
            }
            // Arrange and Position each child to the same cell size
            foreach (UIElement child in list)
            {
                child.Arrange(childBounds);

                // only advance to the next grid cell if the child was not collapsed
                if (child.Visibility != Visibility.Collapsed)
                {
                    childBounds.X += xStep;
                    if (childBounds.X >= xBound)
                    {
                        childBounds.Y += childBounds.Height;
                        childBounds.X = 0;
                    }
                }
            }

            return arrangeSize;
        }
        #endregion


        #region Private Methods

        /// <summary>
        /// If either Rows or Columns are set to 0, then dynamically compute these
        /// values based on the actual number of non-collapsed children.
        ///
        /// In the case when both Rows and Columns are set to 0, then make Rows 
        /// and Columns be equal, thus laying out in a square grid.
        /// </summary>
        private void UpdateComputedValues()
        {
            _columns = Columns;
            _rows = Rows;

            //parameter checking. 
            if (FirstColumn >= _columns)
            {
                //NOTE: maybe we shall throw here. But this is somewhat out of 
                //the MCC itself. We need a whole new panel spec.
                FirstColumn = 0;
            }

            if ((_rows == 0) || (_columns == 0))
            {
                int nonCollapsedCount = 0;

                // First compute the actual # of non-collapsed children to be laid out
                for (int i = 0, count = InternalChildren.Count; i < count; ++i)
                {
                    UIElement child = InternalChildren[i];
                    if (child.Visibility != Visibility.Collapsed)
                    {
                        nonCollapsedCount++;
                    }
                }

                // to ensure that we have at leat one row & column, make sure
                // that nonCollapsedCount is at least 1
                if (nonCollapsedCount == 0)
                {
                    nonCollapsedCount = 1;
                }

                if (_rows == 0)
                {
                    if (_columns > 0)
                    {
                        // take FirstColumn into account, because it should really affect the result
                        _rows = (nonCollapsedCount + FirstColumn + (_columns - 1)) / _columns;
                    }
                    else
                    {
                        // both rows and columns are unset -- lay out in a square
                        _rows = (int)Math.Sqrt(nonCollapsedCount);
                        if ((_rows * _rows) < nonCollapsedCount)
                        {
                            _rows++;
                        }
                        _columns = _rows;
                    }
                }
                else if (_columns == 0)
                {
                    // guaranteed that _rows is not 0, because we're in the else clause of the check for _rows == 0
                    _columns = (nonCollapsedCount + (_rows - 1)) / _rows;
                }
            }
        }

        #endregion Private Properties

        private int _rows;
        private int _columns;
    }
}
