using JayLib.JayFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JayLib.JaySerialization
{
    public static class JSerializer
    {
        public static object Deserialize(string filePath)
        {
            if (FileHelper.IsFileCanUse(filePath))
            {
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                object _object = formatter.Deserialize(stream);
                stream.Close();
                return _object;
            }
            else
            {
                return null;
            }
        }

        public static bool Serialize(ISerializable serializableFile, string filePath)
        {
            if (serializableFile == null)
            {
                return false;
            }
            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, serializableFile);
            stream.Close();
            return true;
        }
    }
}
