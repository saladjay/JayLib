using JayLib;
using System;
using System.Collections.Generic;
using System.Data;
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
            test();
            //DBCURD.CreateDatabase();
            //DBCURD.Open();
            //DBCURD.QueryBulk();
            //DBCURD.DeleteBulk();
            //DBCURD.CreateTable();
            //DBCURD.QueryBulk();
            //DBCURD.MultiQuery();
            //DBCURD.QueryBulk();
            //DBCURD.InsertOrUpdate();
            //DBCURD.InsertOrIgnore();
            //DBCURD.InsertOrReplace();
            //string white = Brushes.White.ToString();

            //Brush brush = (Brush)SingleTon<BrushConverter>.GetInstance().ConvertFromString(white);
        }

        public void test()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("age", typeof(string)));

            for (int i = 0; i < 15; i++)
            {
                var row = dataTable.NewRow();
                row["name"] = i.ToString().PadLeft(5, '0');
                row["age"] = i.ToString().PadRight(6, '0');
                dataTable.Rows.Add(row);
            }
            var s = new List<string>();
            s.Add("110000");
            s.Add("200000");
            s.Add("120000");
            for (int i = 0; i < s.Count; i++)
            {
                s[i] = $"'{s[i]}'";
            }
            var dt = dataTable.Select($"age in ({string.Join(",",s.ToArray())})");
            var dt1 = dataTable.Select("age='100000'");
            int a = 1;
        }
    }
}
