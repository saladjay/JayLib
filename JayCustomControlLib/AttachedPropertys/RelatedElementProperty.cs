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
        private static Dictionary<FrameworkElement, FrameworkElement> relatedControlDict = new Dictionary<FrameworkElement, FrameworkElement>();

        public static FrameworkElement GetRelatedFrameworkElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(RelatedFrameworkElementProperty);
        }

        public static void SetRelatedFrameworkElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(RelatedFrameworkElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for RelatedFrameworkElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RelatedFrameworkElementProperty =
            DependencyProperty.RegisterAttached("RelatedFrameworkElement", typeof(FrameworkElement), typeof(RelatedElementProperty), new PropertyMetadata(default(FrameworkElement)));

        public static FrameworkElement GetRelatedFrameworkElement(FrameworkElement keyElement)
        {
            return relatedControlDict.TryGetValue(keyElement, out FrameworkElement fe) ? fe : null;
        }

        public static IEnumerable<FrameworkElement> GetRelatedFrameworkElementKeys(FrameworkElement valueElement)
        {
            return from pair in relatedControlDict
                   where pair.Value.Equals(valueElement)
                   select pair.Key;
        }
    }
}
