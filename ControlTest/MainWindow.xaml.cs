using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
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
using JayCustomControlLib;
using JayLib.JaySerialization;
using JayLib.WPF.BasicClass;

namespace ControlTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrientationThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetTop(sender as OrientationThumb, Canvas.GetTop(sender as OrientationThumb) + e.VerticalChange);
            Canvas.SetLeft(sender as OrientationThumb, Canvas.GetLeft(sender as OrientationThumb) + e.HorizontalChange);
            //Debug.WriteLine(e.VerticalChange.ToString());
            //Debug.WriteLine(Canvas.GetTop(sender as OrientationThumb).ToString());
            Debug.WriteLine(e.VerticalChange.ToString());
            Debug.WriteLine(Canvas.GetTop(sender as OrientationThumb).ToString());
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetTop(sender as Thumb, Canvas.GetTop(sender as Thumb) + e.VerticalChange);
        }
    }
}