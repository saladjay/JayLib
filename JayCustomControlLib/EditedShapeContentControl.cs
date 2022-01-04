using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:EditedShapeContentControl/>
    ///
    /// </summary>

    public class EditedShapeContentControl : ContentControl
    {
        static EditedShapeContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditedShapeContentControl), new FrameworkPropertyMetadata(typeof(EditedShapeContentControl)));
        }




        public bool AutoHideEditedThumb
        {
            get { return (bool)GetValue(AutoHideEditedThumbProperty); }
            set { SetValue(AutoHideEditedThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoHideEditedThumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoHideEditedThumbProperty =
            DependencyProperty.Register("AutoHideEditedThumb", typeof(bool), typeof(EditedShapeContentControl), new PropertyMetadata(true));




        public Brush ThumbBrush
        {
            get { return (Brush)GetValue(ThumbBrushProperty); }
            set { SetValue(ThumbBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbBrushProperty =
            DependencyProperty.Register("ThumbBrush", typeof(Brush), typeof(EditedShapeContentControl), new PropertyMetadata(Brushes.BlueViolet));




        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Parent is Canvas canvas)
            {
                if (e.Key == Key.Up || e.Key == Key.W)
                {
                    double top = Canvas.GetTop(this);
                    Canvas.SetTop(this, top - 1);
                }
                else if(e.Key == Key.Left || e.Key == Key.A)
                {
                    double left = Canvas.GetLeft(this);
                    Canvas.SetLeft(this, left - 1);
                }
                else if(e.Key == Key.Right || e.Key == Key.D)
                {
                    double left = Canvas.GetLeft(this);
                    Canvas.SetLeft(this, left + 1);
                }
                else if(e.Key == Key.Down || e.Key == Key.S)
                {
                    double top = Canvas.GetTop(this);
                    Canvas.SetTop(this, top + 1);
                }
            }

            base.OnKeyDown(e);
        }
    }
}
