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
        public override void BeginInit()
        {
            base.BeginInit();
        }

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
            colorPickerTest colorPickerTest = new colorPickerTest();
            //bbb.CalculateValueFunction = dddd;
            List<int> aa = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                aa.Add(i);
            }
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
            bbb.ItemsSource = aa;
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

            cac cccc = new cac()
            {
                Text = "ac",
                Value = 5,
                MyProperty = 1.1456,
                listint=new List<int>() { 1,2,3},
                listccc=new List<ccc>()
                {
                    new ccc(){Text="a"},
                    new ccc(){Text="b"}
                }
            };
            ccc aaa = new ccc()
            {
                Text = "aaa",
                Value = 6,
            };
            JSerializer.Serialize(cccc, AppDomain.CurrentDomain.BaseDirectory + "c.bin");
            JSerializer.Serialize(aaa, AppDomain.CurrentDomain.BaseDirectory + "a.bin");

            var dccc = JSerializer.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "c.bin") as cac;
            var accc = JSerializer.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "a.bin") as ccc;
            int b = 0;
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

    public class colorPickerTest:NotificationObject
    {
        private Brush mycolor;

        public Brush myColor
        {
            get { return mycolor; }
            set { mycolor = value; OnPropertyChanged(() => myColor); }
        }


        private Brush mycolor2;

        public Brush myColor2
        {
            get { return mycolor2; }
            set { mycolor2 = value; OnPropertyChanged(() => myColor2); }
        }

    }

    [Serializable]
    public class ccc : NotificationObject,ISerializable
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

        public List<int> listint { get; set; }
        public RelayCommand RelayCommand1 { get; set; }
        public RelayCommand<int> RelayCommand2 { get; set; }

        private string testbool;

        public ccc()
        {
            RelayCommand1 = new RelayCommand(RelayCommand1E);
            RelayCommand2 = new RelayCommand<int>(RelayCommand2E);
            testbool = "aaaa";
        }

        private void RelayCommand2E(int obj)
        {
            
        }

        private void RelayCommand1E()
        {
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializerHelper.SerializationFieldHelper(info, this);
            SerializerHelper.SerializationPropertyHelper(info, this,typeof(int),typeof(RelayCommand),typeof(RelayCommand<int>));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ccc(SerializationInfo info, StreamingContext context)
        {
            SerializerHelper.DeserializtionPropertyHelper(info, this, typeof(int), typeof(RelayCommand), typeof(RelayCommand<int>));
            SerializerHelper.DeSerializationFieldHelper(info, this);
        }
    }
    [Serializable]
    public class cac:ccc
    {
        public double MyProperty { get; set; }
        public List<ccc> listccc { get; set; }
        public cac()
        {

        }
        protected cac(SerializationInfo info, StreamingContext context)
        {
            SerializerHelper.DeserializtionPropertyHelper(info, this, typeof(int), typeof(RelayCommand), typeof(RelayCommand<int>));
            SerializerHelper.DeSerializationFieldHelper(info, this);
        }
    }
}
