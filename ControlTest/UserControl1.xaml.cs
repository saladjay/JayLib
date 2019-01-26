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

namespace ControlTest
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            aaa.ItemsSource = new List<Brush> { Brushes.Red, Brushes.Green, Brushes.Blue };

            aaa.SelectionChanged += Aaa_SelectionChanged;
        }

        private void Aaa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyColor = (Brush)aaa.SelectedItem;
        }

        public Brush MyColor
        {
            get { return (Brush)GetValue(MyColorProperty); }
            set { SetValue(MyColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyColorProperty =
            DependencyProperty.Register("MyColor", typeof(Brush), typeof(UserControl1), new PropertyMetadata(Brushes.Red));


    }
}
