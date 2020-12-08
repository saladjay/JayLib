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

namespace JayCustomControlLib
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib;assembly=JayCustomControlLib"
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
    ///     <MyNamespace:OrientationThumb/>
    ///
    /// </summary>
    public class OrientationThumb : Thumb
    {
        static OrientationThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OrientationThumb), new FrameworkPropertyMetadata(typeof(OrientationThumb)));
        }
        
        public Orientation MovedDirection
        {
            get { return (Orientation)GetValue(MovedDirectionProperty); }
            set { SetValue(MovedDirectionProperty, value); }
        }

        public static Orientation GetMovedDirection(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(MovedDirectionProperty);
        }

        public static void SetMovedDirection(DependencyObject obj, Orientation value)
        {
            obj.SetValue(MovedDirectionProperty, value);
        }

        // Using a DependencyProperty as the backing store for MovedDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MovedDirectionProperty =
            DependencyProperty.RegisterAttached("MovedDirection", typeof(Orientation), typeof(OrientationThumb), new PropertyMetadata(Orientation.Horizontal));
        
        private Point _originThumbPoint;
        private Point _originScreenCoordPosition;
        private Point _previousScreenCoordPosition;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsDragging)
            {
                e.Handled = true;
                base.Focus();
                base.CaptureMouse();
                IsDragging = true;
                _originThumbPoint = e.GetPosition(this);
                _previousScreenCoordPosition = _originScreenCoordPosition = e.GetPosition(this);
                bool flag = true;
                try
                {
                    base.RaiseEvent(new DragStartedEventArgs(_originThumbPoint.X, _originThumbPoint.Y));
                    flag = false;
                }
                finally
                {
                    if (flag)
                    {
                        CancelDrag();
                    }
                }
            }
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (base.IsMouseCaptured && IsDragging)
            {
                e.Handled = true;
                IsDragging = false;
                base.ReleaseMouseCapture();
                Point point = e.GetPosition(this);
                base.RaiseEvent(new DragCompletedEventArgs(point.X - _originScreenCoordPosition.X, point.Y - _originScreenCoordPosition.Y, false));
            }
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsDragging)
            {
                if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                {
                    Point position = e.GetPosition(this);
                    Point point = e.GetPosition(this);
                    if (point != _previousScreenCoordPosition)
                    {
                        _previousScreenCoordPosition = point;
                        e.Handled = true;
                        switch (GetMovedDirection(this))
                        {
                            case Orientation.Horizontal:
                                base.RaiseEvent(new DragDeltaEventArgs(position.X - _originThumbPoint.X, _originThumbPoint.Y));
                                break;
                            case Orientation.Vertical:
                                base.RaiseEvent(new DragDeltaEventArgs(_originThumbPoint.X, position.Y - _originThumbPoint.Y));
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (e.MouseDevice.Captured == this)
                    {
                        base.ReleaseMouseCapture();
                    }
                    IsDragging = false;
                    _originThumbPoint.X = 0.0;
                    _originThumbPoint.Y = 0.0;
                }
            }
        }
    }
}
