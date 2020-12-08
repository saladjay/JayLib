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
    /// UserControl2.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private void Line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            var left = point.X;
            var UserEllipse = new UserEllipse();
            var width = UserEllipse.Width;
            canvas.Children.Add(UserEllipse);
            Canvas.SetLeft(UserEllipse, left);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
