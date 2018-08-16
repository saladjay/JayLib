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
    public static class PanelItemsCollectionProperty
    {
        const string _Canvas = "System.Windows.Controls.Canvas";
        const string _Grid = "System.Windows.Controls.Grid";
        const string _UniformGrid = "System.Windows.Controls.Primitives.UniformGrid";
        const string _WrapGrid = "System.Windows.Controls.WrapPanel";
        const string _StackPanel = "System.Windows.Controls.StackPanel";
        const string _DockPanel = "System.Windows.Controls.DockPanel";
        const string _UniformPanel = "DCS_100.MyCustomControl.UniformPanel";

        public static IEnumerable<UIElement> GetItemsSource(DependencyObject obj)
        {
            return (IEnumerable<UIElement>)obj.GetValue(ItemsSourceProperty);
        }

        public static void SetItemsSource(DependencyObject obj, IEnumerable<UIElement> value)
        {
            obj.SetValue(ItemsSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable<UIElement>), typeof(PanelItemsCollectionProperty), new PropertyMetadata(null, OnItemsSourceChanged));
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null || e.NewValue == null)
            {
                return;
            }
            IEnumerable itemsSource = (IEnumerable)e.NewValue;
            switch (d.GetType().ToString())
            {
                case _Canvas:
                    {
                        Canvas canvas = d as Canvas;
                        canvas.Children.Clear();
                        if (itemsSource is INotifyCollectionChanged notifyCollection)
                        {
                            notifyCollection.CollectionChanged += (sender, args) =>
                            {
                                if (args.Action == NotifyCollectionChangedAction.Add)
                                {
                                    foreach (var item in args.NewItems)
                                    {
                                        canvas.Children.Add((UIElement)item);
                                    }
                                }
                                if (args.Action == NotifyCollectionChangedAction.Remove)
                                {
                                    foreach (var item in args.OldItems)
                                    {
                                        canvas.Children.Add((UIElement)item);
                                    }
                                }
                            };
                        }
                        foreach (var item in itemsSource)
                        {
                            canvas.Children.Add((UIElement)item);
                        }
                        break;
                    }
                case _UniformGrid:
                    {
                        UniformGrid uniformGrid = d as UniformGrid;
                        uniformGrid.Children.Clear();
                        if (itemsSource is INotifyCollectionChanged notifyCollection)
                        {
                            notifyCollection.CollectionChanged += (sender, args) =>
                            {
                                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                                {
                                    foreach (var item in args.NewItems)
                                    {
                                        uniformGrid.Children.Add((UIElement)item);
                                    }
                                }
                                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                                {
                                    foreach (var item in args.OldItems)
                                    {
                                        uniformGrid.Children.Add((UIElement)item);
                                    }
                                }
                            };
                        }
                        foreach (var item in itemsSource)
                        {
                            uniformGrid.Children.Add((UIElement)item);
                        }
                        break;
                    }
                case _UniformPanel:
                    {
                        UniformPanel uniformPanel = d as UniformPanel;
                        uniformPanel.Children.Clear();
                        if (itemsSource is INotifyCollectionChanged notifyCollection)
                        {
                            notifyCollection.CollectionChanged += (sender, args) =>
                            {
                                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                                {
                                    foreach (var item in args.NewItems)
                                    {
                                        uniformPanel.Children.Add((UIElement)item);
                                    }
                                }
                                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                                {
                                    foreach (var item in args.OldItems)
                                    {
                                        uniformPanel.Children.Add((UIElement)item);
                                    }
                                }
                            };
                        }
                        foreach (var item in itemsSource)
                        {
                            uniformPanel.Children.Add((UIElement)item);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
