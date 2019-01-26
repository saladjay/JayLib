using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WindowTest
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

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        public override void BeginInit()
        {
            base.BeginInit();
            var executableName = Process.GetCurrentProcess().MainModule.FileName;
            if (Properties.Settings.Default.SoftwareLength != 0)
            {
                long length = Properties.Settings.Default.SoftwareLength;
                using (FileStream file = File.OpenRead(executableName))
                {
                    if (file.Seek(0, SeekOrigin.End) > length)
                    {
                        try
                        {
                            file.Seek(length, SeekOrigin.Begin);
                            var bytes = new byte[1024];

                            file.Read(bytes, 0, bytes.Length);
                            var a = Encoding.Default.GetString(bytes);
                            MessageBox.Show(a);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            try
            {

                var executableName = Process.GetCurrentProcess().MainModule.FileName;
                //// rename executable file
                //var newExecutableName = System.AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "_.exe");
                //FileInfo fi = new FileInfo(executableName);
                //fi.MoveTo(newExecutableName);
                //// make copy of executable file to original name
                //File.Copy(newExecutableName, executableName, true);

                //// write data end of new file
                //var bytes = Encoding.ASCII.GetBytes("new data...");
                //var bytes2 = new byte[1024];
                //Array.Copy(bytes, bytes2, bytes.Length);
                //using (FileStream file = File.OpenWrite(executableName))
                //{
                //    if (Properties.Settings.Default.SoftwareLength == 0)
                //    {
                //        Properties.Settings.Default.SoftwareLength = file.Seek(0, SeekOrigin.End);
                //    }
                //    else
                //    {
                //        file.Seek(Properties.Settings.Default.SoftwareLength, SeekOrigin.Begin);
                //    }
                //    file.Write(bytes2, 0, 1024);
                //}
                //Properties.Settings.Default.Save();
                //ProcessStartInfo Info = new ProcessStartInfo();
                //Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                //               executableName.Replace(".exe", "_.exe");
                //Info.WindowStyle = ProcessWindowStyle.Hidden;
                //Info.CreateNoWindow = true;
                //Info.FileName = "cmd.exe";
                //Process.Start(Info);
                SaveDataIntoExe saveDataIntoExe = new SaveDataIntoExe(1024);
                saveDataIntoExe.SaveData(new byte[] { });

                string exePath = executableName.Replace(".exe", "_.exe");
                if (File.Exists(exePath))
                {
                    saveDataIntoExe.DeleteFile(exePath);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
