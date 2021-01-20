using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsynchronousProgramming
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string text = "http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe";
        public MainWindow()
        {
            InitializeComponent();
            txbUrl.Text = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rtbState.AppendText("downloading......\n");
            if (string.IsNullOrWhiteSpace(txbUrl.Text))
            {
                MessageBox.Show("please input download address");
                return;
            }
            DownloadFileSync(txbUrl.Text.Trim());
        }

        private void DownloadFileSync(string url)
        {
            int buffSize = 2048;
            byte[] bufferRead = new byte[buffSize];
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\dotNetFx 35setup.exe";
            FileStream fileStream = null;
            HttpWebResponse myWebResponse = null;
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            fileStream = new FileStream(savePath, FileMode.OpenOrCreate);
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                if (myHttpWebRequest != null)
                {
                    myWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    Stream responseStream = myWebResponse.GetResponseStream();
                    int readSize = responseStream.Read(bufferRead, 0, buffSize);
                    while (readSize > 0)
                    {
                        rtbState.AppendText($"get {readSize} bytes\n");
                        fileStream.Write(bufferRead, 0, readSize);
                        readSize = responseStream.Read(bufferRead, 0, buffSize);
                    }
                    rtbState.AppendText($"finish download, the size of file is {fileStream.Length}, and the path of file is {savePath}\n");
                }
            }
            catch(Exception e)
            {
                rtbState.AppendText($"error: {e.Message}\n");
            }
            finally
            {
                if (myWebResponse!=null)
                {
                    myWebResponse.Close();
                }
                if (fileStream!=null)
                {
                    fileStream.Close();
                }
            }
        }
    }
}
