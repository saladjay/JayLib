using System;
using System.Collections.Generic;
using System.Globalization;
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
    ///     <MyNamespace:JProgressRing/>
    ///
    /// </summary>
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public class JProgressRing : Control
    {
        public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(JProgressRing), new PropertyMetadata(default(double), BindableWidthCallback));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(JProgressRing), new PropertyMetadata(true, IsActiveChanged));

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(JProgressRing), new PropertyMetadata(true, IsLargeChangedCallback));

        public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(JProgressRing), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(JProgressRing), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(JProgressRing), new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty EllipseDiameterScaleProperty = DependencyProperty.Register("EllipseDiameterScale", typeof(double), typeof(JProgressRing), new PropertyMetadata(1D));

        private List<Action> _deferredActions = new List<Action>();

        static JProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JProgressRing), new FrameworkPropertyMetadata(typeof(JProgressRing)));
            VisibilityProperty.OverrideMetadata(typeof(JProgressRing), new FrameworkPropertyMetadata(new PropertyChangedCallback(
                (ringObject, e) =>
                {
                    if (e.NewValue != e.OldValue)
                    {
                        var ring = (JProgressRing)ringObject;
                        //auto set IsActive to false if we're hiding it.
                        if ((Visibility)e.NewValue != Visibility.Visible)
                        {
                            //sets the value without overriding it's binding (if any).
                            ring.SetCurrentValue(JProgressRing.IsActiveProperty, false);
                        }
                        else
                        {
                            // #1105 don't forget to re-activate
                            ring.IsActive = true;
                        }
                    }
                })));
        }

        public JProgressRing()
        {
            SizeChanged += OnSizeChanged;
        }

        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            private set { SetValue(MaxSideLengthProperty, value); }
        }

        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            private set { SetValue(EllipseDiameterProperty, value); }
        }

        public double EllipseDiameterScale
        {
            get { return (double)GetValue(EllipseDiameterScaleProperty); }
            set { SetValue(EllipseDiameterScaleProperty, value); }
        }

        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            private set { SetValue(EllipseOffsetProperty, value); }
        }

        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            private set { SetValue(BindableWidthProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as JProgressRing;
            if (ring == null)
                return;

            var action = new Action(() =>
            {

                ring.SetEllipseDiameter(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetEllipseOffset(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
                ring.SetMaxSideLength(
                    (double)dependencyPropertyChangedEventArgs.NewValue);
            });

            if (ring._deferredActions != null)
                ring._deferredActions.Add(action);
            else
                action();
        }

        private void SetMaxSideLength(double width)
        {
            MaxSideLength = width <= 20 ? 20 : width;
        }

        private void SetEllipseDiameter(double width)
        {
            EllipseDiameter = (width / 8) * EllipseDiameterScale;
        }

        private void SetEllipseOffset(double width)
        {
            EllipseOffset = new Thickness(0, width / 2, 0, 0);
        }

        private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as JProgressRing;
            if (ring == null)
                return;

            ring.UpdateLargeState();
        }

        private void UpdateLargeState()
        {
            Action action;

            if (IsLarge)
                action = () => VisualStateManager.GoToState(this, "Large", true);
            else
                action = () => VisualStateManager.GoToState(this, "Small", true);

            if (_deferredActions != null)
                _deferredActions.Add(action);

            else
                action();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            BindableWidth = ActualWidth;
        }

        private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as JProgressRing;
            if (ring == null)
                return;

            ring.UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            Action action;

            if (IsActive)
                action = () => VisualStateManager.GoToState(this, "Active", true);
            else
                action = () => VisualStateManager.GoToState(this, "Inactive", true);

            if (_deferredActions != null)
                _deferredActions.Add(action);

            else
                action();
        }

        public override void OnApplyTemplate()
        {
            //make sure the states get updated
            UpdateLargeState();
            UpdateActiveState();
            base.OnApplyTemplate();
            if (_deferredActions != null)
                foreach (var action in _deferredActions)
                    action();
            _deferredActions = null;
        }
    }

    internal class WidthToMaxSideLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                return width <= 20 ? 20 : width;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
