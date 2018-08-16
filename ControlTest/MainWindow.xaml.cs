using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using JayCustomControlLib;
using JayLib.WPF.BasicClass;

namespace ControlTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] FaderTickStrings { get; private set; } = new string[]
{
            "OFF",  //0
            "-70",//1
            "","","","","","","","","","",
            "-25",//12
            "","","","","","","","",
            "-18",//21
            "","","","","","","","","",
            "-13",//31
            "","","","","","","","",
            "-9",//40
            "","","","","","","","","",
            "-6",//50
            "","","","","","","","","",
            "0",//60
            "","","","","","","","","","",
            "+6",//71
            "","","","","","","","",
            "+10",//80
};

        ccc c = new ccc();
        System.Timers.Timer timer = new System.Timers.Timer(1000);

        public MainWindow()
        {
            InitializeComponent();
            bbb.CalculateValueFunction = dddd;
            //List<int> aa = new List<int>();
            //for (int i = 0; i < 10; i++)
            //{
            //    aa.Add(i);
            //}
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
            //bbb.ItemsSource = aa;
            //ttt.ItemsSource = aa;
            //bbb.SetBinding(JSpinner.ValueProperty, new Binding("Value") { Source = c ,Mode= BindingMode.OneWay});
            //ttt.SetBinding(ComboBox.SelectedIndexProperty, new Binding("Value") { Source = c ,Mode= BindingMode.OneWay});

            JLimitGDSlider limitGDSlider = new JLimitGDSlider();
            limitGDSlider.RatioStrings = new string[]
{
            "1:1",
            "1.2:1",
            "1.3:1",
            "1.5:1",
            "1.7:1",
            "2.0:1",
            "2.2:1",
            "2.3:1",
            "2.5:1",
            "3:1",
            "3.5:1",
            "4:1",
            "4.5:1",
            "5:1",
            "5.5:1",
            "6:1",
            "6.5:1",
            "7:1",
            "7.5:1",
            "8:1",
            "8.5:1",
            "9:1",
            "9.5:1",
            "10:1",
            "Limit"
};
        }
        int i = 0;
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                i++;
                c.Value = i;
                c.Text = i.ToString();
                if (i==8)
                {
                    i = 0;
                }
            });
        }

        private int dddd(string b)
        {
            return 0;
        }
    }

    public class ccc : NotificationObject
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(() => Value); }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(() => Text); }
        }
    }
}
