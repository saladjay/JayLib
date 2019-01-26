using JayLib;
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

namespace ROMTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DBCURD.CreateDatabase();
            DBCURD.Open();
            //DBCURD.QueryBulk();
            //DBCURD.DeleteBulk();
            //DBCURD.CreateTable();
            //DBCURD.QueryBulk();
            //DBCURD.MultiQuery();
            //DBCURD.QueryBulk();
            //DBCURD.InsertOrUpdate();
            //DBCURD.InsertOrIgnore();
            DBCURD.InsertOrReplace();
            //string white = Brushes.White.ToString();

            //Brush brush = (Brush)SingleTon<BrushConverter>.GetInstance().ConvertFromString(white);
        }
    }
}
