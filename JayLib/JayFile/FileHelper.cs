using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JayLib.JayFile
{
    public class FileHelper
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly static IntPtr HFILE_ERROR = new IntPtr(-1);
        public static bool IsFileCanUse(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }
            if (!File.Exists(filePath))
            {
                return false;
            }
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return false;
            }
            CloseHandle(vHandle);
            return true;
        }
    }
}
