using JayCustomControlLib.AttachedPropertys;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:iCare.EmrEditor.ResizeImageExt"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:iCare.EmrEditor.ResizeImageExt;assembly=iCare.EmrEditor.ResizeImageExt"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class EditedShapeThumb : Thumb
    {
        static EditedShapeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditedShapeThumb), new FrameworkPropertyMetadata(typeof(EditedShapeThumb)));
        }

        public string RelatedString { get; set; }
        public UIElement RelatedUIElement { get; set; } = null;
        public FrameworkElement RelatedFrameworkElement { get; set; } = null;
        public Control RelatedControl { get; set; } = null;

        public EditedShapeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
        }

        /// <summary>
        /// TODO:使用附加依赖属性找出Grid。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (RelatedControl == null)
            {
                RelatedControl = RelatedElementProperty.GetRelatedControl(this);
            }
            var b = VisualTreeHelper.GetParent(this) as FrameworkElement;
            var c = VisualTreeHelper.GetParent(b) as FrameworkElement;
            var d = VisualTreeHelper.GetParent(c) as FrameworkElement;

            if (RelatedControl != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            RelatedControl.ActualHeight - RelatedControl.MinHeight);
                        RelatedControl.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            RelatedControl.ActualHeight - RelatedControl.MinHeight);
                        //Canvas.SetTop(EditedShapeContentControl, Canvas.GetTop(EditedShapeContentControl) + deltaVertical);
                        RelatedControl.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,
                            RelatedControl.ActualWidth - RelatedControl.MinWidth);
                        //Canvas.SetLeft(EditedShapeContentControl, Canvas.GetLeft(EditedShapeContentControl) + deltaHorizontal);
                        RelatedControl.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            RelatedControl.ActualWidth - RelatedControl.MinWidth);
                        RelatedControl.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }
            e.Handled = true;
        }
    }

    public class ThumbParent : Panel
    {
        static ThumbParent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThumbParent), new FrameworkPropertyMetadata(typeof(ThumbParent)));
        }

        public override void OnApplyTemplate()
        {
            foreach (UIElement child in Children)
            {
                if (child is EditedShapeThumb thumb && this.Parent is FrameworkElement fe)
                {
                    thumb.RelatedFrameworkElement = fe;
                }
            }
            base.OnApplyTemplate();
        }

        protected override void OnIsMouseDirectlyOverChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            base.OnIsMouseDirectlyOverChanged(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            base.OnMouseLeave(e);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                if (child is Thumb thumb)
                {
                    if (thumb.HorizontalAlignment== HorizontalAlignment.Stretch)
                    {
                        thumb.Measure(new Size(availableSize.Width,thumb.Height));
                    }
                    else if (thumb.VerticalAlignment == VerticalAlignment.Stretch)
                    {
                        thumb.Measure(new Size(thumb.Width, availableSize.Height));
                    }
                    else
                    {
                        thumb.Measure(new Size(thumb.Width, thumb.Height));
                    }
                    
                    var size = thumb.DesiredSize;
                    var w = thumb.Width;
                    var h = thumb.Height;
                }
                else
                {
                    child.Measure(availableSize);
                }
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                if (child is Thumb thumb)
                {
                    var halfw = double.IsNaN(thumb.Width) ? this.Width / 2 : thumb.Width / 2;
                    var halfh = double.IsNaN(thumb.Height) ? this.Height / 2 : thumb.Height / 2;
                    Point lt = new Point(), rb = new Point();
                    SetZIndex(thumb, 1);
                    switch (thumb.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            lt.X = 0 - halfw;
                            break;
                        case HorizontalAlignment.Center:
                            lt.X = this.ActualWidth / 2 - halfw;
                            break;
                        case HorizontalAlignment.Right:
                            lt.X = this.ActualWidth - halfw;
                            break;
                        case HorizontalAlignment.Stretch:
                            lt.X = 0;
                            SetZIndex(thumb, 0);
                            break;
                        default:
                            break;
                    }
                    rb.X = lt.X + halfw * 2;
                    switch (thumb.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            lt.Y = 0 - halfh;
                            break;
                        case VerticalAlignment.Center:
                            lt.Y = this.ActualHeight / 2 - halfh;
                            break;
                        case VerticalAlignment.Bottom:
                            lt.Y = this.ActualHeight - halfh;
                            break;
                        case VerticalAlignment.Stretch:
                            lt.Y = 0;
                            break;
                        default:
                            break;
                    }
                    rb.Y = lt.Y + halfh * 2;
                    thumb.Arrange(new Rect(lt, rb));
                }
            }
            return finalSize;
        }
    }
}
