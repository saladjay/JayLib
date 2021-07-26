using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


namespace JayCustomControlLib.AttachedPropertys
{
    public static class RelatedElementProperty
    {
        private static Dictionary<string, UIElement> relatedValueControlDict = new Dictionary<string, UIElement>();
        private static Dictionary<UIElement, string> relatedKeyControlDict = new Dictionary<UIElement, string>();


        public static string GetRelatedKey(DependencyObject obj)
        {
            return (string)obj.GetValue(RelatedKeyProperty);
        }

        public static void SetRelatedKey(DependencyObject obj, string value)
        {
            obj.SetValue(RelatedKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for RelatedKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedKeyProperty =
            DependencyProperty.RegisterAttached("RelatedKey", typeof(string), typeof(RelatedElementProperty), new PropertyMetadata(default(string), OnRelatedKeyChanged));

        private static void OnRelatedKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement relatedControl)
            {
                relatedKeyControlDict[relatedControl] = (string)e.NewValue;
            }
        }



        public static string GetRelatedValue(DependencyObject obj)
        {
            return (string)obj.GetValue(RelatedValueProperty);
        }

        public static void SetRelatedValue(DependencyObject obj, string value)
        {
            obj.SetValue(RelatedValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for RelatedValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedValueProperty =
            DependencyProperty.RegisterAttached("RelatedValue", typeof(string), typeof(RelatedElementProperty), new PropertyMetadata(default(string), OnRelatedValueChanged));

        private static void OnRelatedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement relatedControl)
            {
                relatedValueControlDict[(string)e.NewValue] = relatedControl;
            }
        }

        public static UIElement GetRelatedUIElement(UIElement keyElement)
        {
            return (relatedKeyControlDict.TryGetValue(keyElement, out string key) && relatedValueControlDict.TryGetValue(key, out UIElement valueElement)) ? valueElement : null;
        }

        public static FrameworkElement GetRelatedFrameworkElement(UIElement keyElement)
        {
            return (relatedKeyControlDict.TryGetValue(keyElement, out string key) && relatedValueControlDict.TryGetValue(key, out UIElement valueElement) && valueElement is FrameworkElement fe) ? fe : null;
        }

        public static Control GetRelatedControl(UIElement keyElement)
        {
            return (relatedKeyControlDict.TryGetValue(keyElement, out string key) && relatedValueControlDict.TryGetValue(key, out UIElement valueElement) && valueElement is Control control) ? control : null;
        }
    }


}
