using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JayCustomControlLib.AttachedPropertys
{
    public class PanelZIndexAssist
    {


        public static bool GetZIndexHelperEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(ZIndexHelperEnableProperty);
        }

        public static void SetZIndexHelperEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(ZIndexHelperEnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for ZIndexHelperEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZIndexHelperEnableProperty =
            DependencyProperty.RegisterAttached("ZIndexHelperEnable", typeof(bool), typeof(PanelZIndexAssist), new PropertyMetadata(false,OnZIndexHelperEnableChanged));

        private static void OnZIndexHelperEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock  textBlock)
            {
                if (string.IsNullOrWhiteSpace(textBlock.Text))
                {
                    Panel.SetZIndex(textBlock, 5);
                    textBlock.Visibility = Visibility.Collapsed;
                    textBlock.Background = Brushes.Transparent;
                }
                else
                {
                    Panel.SetZIndex(textBlock, 0);
                }
            }
        }
    }
}
