using JayCustomControlLib.AttachedPropertys;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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



        public EditedProperty EditedContentProperty
        {
            get { return (EditedProperty)GetValue(EditedContentPropertyProperty); }
            set { SetValue(EditedContentPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditedContentProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditedContentPropertyProperty =
            DependencyProperty.Register("EditedContentProperty", typeof(EditedProperty), typeof(EditedShapeThumb), new PropertyMetadata(EditedProperty.shape));







        public string RelatedString { get; set; }

        public FrameworkElement RelatedFramework { get; set; } = null;

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
            if (RelatedFramework != null)
            {
                if (EditedContentProperty == EditedProperty.shape)
                {
                    double deltaVertical, deltaHorizontal;

                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            deltaVertical = Math.Min(-e.VerticalChange,
                                RelatedFramework.ActualHeight - RelatedFramework.MinHeight);
                            RelatedFramework.Height -= deltaVertical;
                            break;
                        case VerticalAlignment.Top:
                            if (RelatedFramework.Parent is Canvas)
                            {
                                var py = Canvas.GetTop(RelatedFramework);
                                py = double.IsNaN(py) ? 0 : py;
                                Canvas.SetTop(RelatedFramework, py + e.VerticalChange);
                            }
                            deltaVertical = Math.Min(e.VerticalChange,
                                RelatedFramework.ActualHeight - RelatedFramework.MinHeight);
                            RelatedFramework.Height -= deltaVertical;
                            break;
                        default:
                            break;
                    }

                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            if (RelatedFramework.Parent is Canvas)
                            {
                                var px = Canvas.GetLeft(RelatedFramework);
                                px = double.IsNaN(px) ? 0 : px;
                                Canvas.SetLeft(RelatedFramework, px + e.HorizontalChange);
                            }
                            deltaHorizontal = Math.Min(e.HorizontalChange,
                                RelatedFramework.ActualWidth - RelatedFramework.MinWidth);
                            RelatedFramework.Width -= deltaHorizontal;
                            break;
                        case HorizontalAlignment.Right:
                            deltaHorizontal = Math.Min(-e.HorizontalChange,
                                RelatedFramework.ActualWidth - RelatedFramework.MinWidth);
                            RelatedFramework.Width -= deltaHorizontal;
                            break;
                        default:
                            break;
                    }
                }
                else if (EditedContentProperty == EditedProperty.position)
                {
                    if (RelatedFramework.Parent is Canvas)
                    {
                        var px = Canvas.GetLeft(RelatedFramework);
                        var py = Canvas.GetTop(RelatedFramework);
                        px = double.IsNaN(px) ? 0 : px;
                        py = double.IsNaN(py) ? 0 : py;
                        Canvas.SetLeft(RelatedFramework, px + e.HorizontalChange);
                        Canvas.SetTop(RelatedFramework, py + e.VerticalChange);

                    }
                }
            }
            e.Handled = true;
        }
    }

    public enum EditedProperty
    {
        position,
        shape,
    }

    public class ThumbParent : Panel
    {
        static ThumbParent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThumbParent), new FrameworkPropertyMetadata(typeof(ThumbParent)));
        }

        public ThumbParent()
        {
            Loaded += ThumbParent_Loaded;
        }

        private void ThumbParent_Loaded(object sender, RoutedEventArgs e)
        {
            if (TemplatedParent is null && Parent is FrameworkElement fe)
            {
                foreach (var item in Children)
                {
                    if (item is EditedShapeThumb thumb)
                    {
                        thumb.RelatedFramework = fe;
                    }
                }
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (TemplatedParent is FrameworkElement fe)
            {
                foreach (var item in Children)
                {
                    if (item is EditedShapeThumb thumb)
                    {
                        thumb.RelatedFramework = fe;
                    }
                }
            }
        }

        

        

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                if (child is Thumb thumb)
                {
                    if (thumb.HorizontalAlignment == HorizontalAlignment.Stretch&& thumb.VerticalAlignment == VerticalAlignment.Stretch)
                    {
                        thumb.Measure(new Size(availableSize.Width, availableSize.Height));
                    }
                    else if (thumb.HorizontalAlignment== HorizontalAlignment.Stretch)
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
            if(TemplatedParent is FrameworkElement fe){
                finalSize.Width = fe.Width;
                finalSize.Height = fe.Height;

            }
            foreach (UIElement child in Children)
            {
                if (child is Thumb thumb)
                {
                    var halfw = double.IsNaN(thumb.Width) ? finalSize.Width / 2 : thumb.Width / 2;
                    var halfh = double.IsNaN(thumb.Height) ? finalSize.Height / 2 : thumb.Height / 2;
                    Point lt = new Point(), rb = new Point();
                    SetZIndex(thumb, 1);
                    switch (thumb.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            lt.X = 0 - halfw;
                            break;
                        case HorizontalAlignment.Center:
                            lt.X = finalSize.Width / 2 - halfw;
                            break;
                        case HorizontalAlignment.Right:
                            lt.X = finalSize.Width - halfw;
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
                            lt.Y = finalSize.Height / 2 - halfh;
                            break;
                        case VerticalAlignment.Bottom:
                            lt.Y = finalSize.Height - halfh;
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
