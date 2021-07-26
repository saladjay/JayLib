using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Random random = new Random();

            //dataGrid.ItemsSource = new ObservableCollection<Person>()
            //{
            //    new Person(){A=false,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name="戴戴1"},
            //    new Person(){A=true,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name="戴雯"},
            //    new Person(){A=false,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name="进击"},
            //    new Person(){A=true,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name="进去"},
            //    new Person(){A=false,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name="病案"},
            //    new Person(){A=true,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100),Name=""},
            //    new Person(){A=false,B=random.Next(1,100),C=random.Next(1,100),D=random.Next(1,100),E=random.Next(1,100)},
            //};


            //var cvs = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            //if (cvs != null && cvs.CanSort)
            //{
            //    cvs.SortDescriptions.Clear();

            //    cvs.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            //    cvs.SortDescriptions.Add(new System.ComponentModel.SortDescription("B", System.ComponentModel.ListSortDirection.Ascending));
            //}
            //this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var sender_key = e.Key;
            var status_key = e.KeyStates;
            if (e.KeyStates == Keyboard.GetKeyStates(Key.LeftAlt) && e.KeyStates == Keyboard.GetKeyStates(Key.Space))
            {
                int a = 0;
            }
            if (ModifierKeys.Alt == Keyboard.Modifiers && e.KeyStates == Keyboard.GetKeyStates(Key.Space))
            {
                int b = 0;
            }
        }
    }

    public class Person
    {
        public bool A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public string Name { get; set; }

    }
}