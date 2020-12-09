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

        public EditedShapeContentControl EditedShapeContentControl { get; set; }

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
            if (EditedShapeContentControl != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            EditedShapeContentControl.ActualHeight - EditedShapeContentControl.MinHeight);
                        EditedShapeContentControl.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            EditedShapeContentControl.ActualHeight - EditedShapeContentControl.MinHeight);
                        //Canvas.SetTop(EditedShapeContentControl, Canvas.GetTop(EditedShapeContentControl) + deltaVertical);
                        EditedShapeContentControl.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,
                            EditedShapeContentControl.ActualWidth - EditedShapeContentControl.MinWidth);
                        //Canvas.SetLeft(EditedShapeContentControl, Canvas.GetLeft(EditedShapeContentControl) + deltaHorizontal);
                        EditedShapeContentControl.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            EditedShapeContentControl.ActualWidth - EditedShapeContentControl.MinWidth);
                        EditedShapeContentControl.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }
            e.Handled = true;
        }
    }
}
