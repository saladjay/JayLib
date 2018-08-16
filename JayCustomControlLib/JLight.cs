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
using System.Windows.Threading;

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
    ///     <MyNamespace:JLight/>
    ///
    /// </summary>
    public class JLight : Control
    {
        #region Construction
        static JLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JLight), new FrameworkPropertyMetadata(typeof(JLight)));
        }

        public JLight()
        {
            _BlinkTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _BlinkTimer.Tick += _BlinkTimer_Tick;
            _BlinkTimer.Stop();
        }

        private void _BlinkTimer_Tick(object sender, EventArgs e)
        {
            Background = _Blink ? _BlinkBrush : NormalBrush;
            _Blink = !_Blink;
        }
        #endregion

        #region Property

        #region Brush


        public Brush GreenBrush
        {
            get { return (Brush)GetValue(GreenBrushProperty); }
            set { SetValue(GreenBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GreenBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GreenBrushProperty =
            DependencyProperty.Register("GreenBrush", typeof(Brush), typeof(JLight), new FrameworkPropertyMetadata(Brushes.Green,FrameworkPropertyMetadataOptions.AffectsRender,new PropertyChangedCallback(OnBrushChanged)));



        public Brush RedBrush
        {
            get { return (Brush)GetValue(RedBrushProperty); }
            set { SetValue(RedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RedBrushProperty =
            DependencyProperty.Register("RedBrush", typeof(Brush), typeof(JLight), new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnBrushChanged)));



        public Brush YellowBrush
        {
            get { return (Brush)GetValue(YellowBrushProperty); }
            set { SetValue(YellowBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YellowBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YellowBrushProperty =
            DependencyProperty.Register("YellowBrush", typeof(Brush), typeof(JLight), new FrameworkPropertyMetadata(Brushes.Yellow, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnBrushChanged)));




        public Brush NormalBrush
        {
            get { return (Brush)GetValue(NormalBrushProperty); }
            set { SetValue(NormalBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NormalBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBrushProperty =
            DependencyProperty.Register("NormalBrush", typeof(Brush), typeof(JLight), new FrameworkPropertyMetadata(Brushes.Gray, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnBrushChanged)));


        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLight light)
            {
                switch (light.LightStatus)
                {
                    case LightStatus.LED_Normal:
                        light.Background = light.NormalBrush;
                        break;
                    case LightStatus.LED_Red:
                        light.Background = light.RedBrush;
                        break;
                    case LightStatus.LED_Yellow:
                        light.Background = light.YellowBrush;
                        break;
                    case LightStatus.LED_Green:
                        light.Background = light.GreenBrush;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Blink support


        public LightStatus BlinkStatus
        {
            get { return (LightStatus)GetValue(BlinkStatusProperty); }
            set { SetValue(BlinkStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlinkStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlinkStatusProperty =
            DependencyProperty.Register("BlinkStatus", typeof(LightStatus), typeof(JLight), new PropertyMetadata(LightStatus.LED_Normal,OnBlinkStatusChanged));

        private static void OnBlinkStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLight light&&e.NewValue is LightStatus lightStatus)
            {
                switch (lightStatus)
                {
                    case LightStatus.LED_Normal:
                        light._BlinkBrush = light.NormalBrush;
                        break;
                    case LightStatus.LED_Red:
                        light._BlinkBrush = light.RedBrush;
                        break;
                    case LightStatus.LED_Yellow:
                        light._BlinkBrush = light.YellowBrush;
                        break;
                    case LightStatus.LED_Green:
                        light._BlinkBrush = light.GreenBrush;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool BlinkEnable
        {
            get { return (bool)GetValue(BlinkEnableProperty); }
            set { SetValue(BlinkEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlinkEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlinkEnableProperty =
            DependencyProperty.Register("BlinkEnable", typeof(bool), typeof(JLight), new PropertyMetadata(false,OnBlinkEnableChanged));

        private static void OnBlinkEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLight light && e.NewValue is bool enable)
            {
                light._BlinkTimer.IsEnabled = enable;
                light.Background = light.NormalBrush;
            }
        }

        #endregion

        
        public LightStatus LightStatus
        {
            get { return (LightStatus)GetValue(LightStatusProperty); }
            set { SetValue(LightStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LightStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightStatusProperty =
            DependencyProperty.Register("LightStatus", typeof(LightStatus), typeof(JLight), new FrameworkPropertyMetadata(LightStatus.LED_Normal, FrameworkPropertyMetadataOptions.AffectsRender, OnLightStatusChanged));

        private static void OnLightStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JLight light && e.NewValue is LightStatus lightStatus)
            {
                switch (lightStatus)
                {
                    case LightStatus.LED_Normal:
                        light.Background = light.NormalBrush;
                        break;
                    case LightStatus.LED_Red:
                        light.Background = light.RedBrush;
                        break;
                    case LightStatus.LED_Yellow:
                        light.Background = light.YellowBrush;
                        break;
                    case LightStatus.LED_Green:
                        light.Background = light.GreenBrush;
                        break;
                    default:
                        break;
                }
            }
        }




        #endregion

        #region Private fields
        private DispatcherTimer _BlinkTimer = new DispatcherTimer();
        private Brush _BlinkBrush = Brushes.Lime;
        private bool _Blink = false;
        #endregion
    }

    public enum LightStatus
    {
        LED_Normal = 0,
        LED_Red = 1,
        LED_Yellow = 2,
        LED_Green = 3
    };
}
