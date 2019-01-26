using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowTest
{
    public class SaveDataIntoExe
    {
        public long AlloctionLength { get; private set; }
        public long ExeFileInfoLengthkey { get; private set; }
        public byte[] Data { get; private set; }

        public SaveDataIntoExe(long allocationBuffLength, long exeFileInfoLength = 0)
        {
            AlloctionLength = allocationBuffLength;
            ExeFileInfoLengthkey = exeFileInfoLength;
        }

        public bool SaveData(byte[] data, string newExeFilePath = null)
        {
            if (data.LongLength > AlloctionLength)
            {
                return false;
            }
            else
            {
                var executableName = Process.GetCurrentProcess().MainModule.FileName;
                // rename executable file
                var newExecutableName = string.IsNullOrWhiteSpace(newExeFilePath) ? System.AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "_.exe") : newExeFilePath;
                FileInfo fi = new FileInfo(executableName);
                fi.MoveTo(newExecutableName);
                // make copy of executable file to original name
                File.Copy(newExecutableName, executableName, true);

                // write data end of new file
                var bytes2 = new byte[AlloctionLength];
                Array.Copy(data, bytes2, data.Length);
                using (FileStream file = File.OpenWrite(executableName))
                {
                    Class1 class1 = new Class1();
                    if (Debugger.IsAttached)
                    {
                        ExeFileInfoLengthkey = file.Seek(0, SeekOrigin.End);
                    }
                    if (class1.UpdateRunCount() == 1)
                    {
                        //Properties.Settings.Default[ExeFileInfoLengthkey] = file.Seek(0, SeekOrigin.End);
                    }
                    {
                        file.Seek(ExeFileInfoLengthkey, SeekOrigin.Begin);
                    }
                    file.Write(bytes2, 0, (int)AlloctionLength);
                }
                return true;
            }
        }

        public void DeleteFile(string filePath)
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                           filePath;
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
        }

        public byte[] ReadData()
        {
            var executableName = Process.GetCurrentProcess().MainModule.FileName;
            if (ExeFileInfoLengthkey != 0)
            {
                long length = ExeFileInfoLengthkey;
                using (FileStream file = File.OpenRead(executableName))
                {
                    if (file.Seek(0, SeekOrigin.End) > length)
                    {
                        try
                        {
                            file.Seek(length, SeekOrigin.Begin);
                            var bytes = new byte[AlloctionLength];
                            file.Read(bytes, 0, bytes.Length);
                            Data = bytes;
                            return bytes;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            return null;
        }
    }
}
