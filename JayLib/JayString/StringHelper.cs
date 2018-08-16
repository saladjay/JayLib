using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JayLib.JayString
{
    /// <summary>
    /// a extended class must be modified by static, also the method
    /// the first parameter must the object that needs to be modified, type "this" in front of it
    /// using the extended class in code before using it's head name.
    /// </summary>
    public static  class StringHelper
    {

        /// <summary>
        /// Appends the string str onto the end of this string.
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Append(this string _input, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return _input;
            }
            else
            {
                return new StringBuilder(_input).Append(str).ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PreAppend(this string _input, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return _input;
            }
            else
            {
                _input = new StringBuilder(str).Append(_input).ToString();
                return _input;
            }
        }
        /// <summary>
        /// Appends the char ch onto the end of this string.
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static string Append(this string _input, char ch)
        {
            return new StringBuilder(_input).Append(ch).ToString();
        }

        public static string Append(this string _input, int value)
        {
            return new StringBuilder(_input).Append(value).ToString();
        }

        public static string Left(this string _input, int count)
        {
            StringBuilder stringBuilder = new StringBuilder(_input);
            stringBuilder.Remove(count, stringBuilder.Length - count);
            return stringBuilder.ToString();
        }


        public static string Right(this string _input, int count)
        {
            StringBuilder stringBuilder = new StringBuilder(_input);
            stringBuilder.Remove(0, stringBuilder.Length - count);
            return stringBuilder.ToString();
        }
        /// <summary>
        /// returns a string of size width that contains this string padded by the fill character.
        /// it will return null if the width is less than the length of the string.
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="width"></param>
        /// <param name="fill"></param>
        /// <returns></returns>
        public static string LeftJustified(this string _input, int width, char fill)
        {
            if (_input.Length >= width)
                return _input;
            else
            {
                return new StringBuilder(_input).Append(fill, width - _input.Length).ToString();
            }
        }

        public static string RightJustified(this string _input, int width, char fill)
        {
            if (_input.Length >= width)
                return _input;
            else
            {
                return new StringBuilder(new string(fill, width - _input.Length)).Append(_input).ToString();
            }
        }
        /// <summary>
        /// return byte array of the string
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static byte[] ToAsciiArray(this string _input)
        {
            return System.Text.Encoding.ASCII.GetBytes(_input);
        }
        /// <summary>
        /// returns hex array of the string
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static byte[] ToHexArray(this string _input)
        {
            if (_input == null)
                return null;
            byte[] ByteArray = System.Text.Encoding.ASCII.GetBytes(_input.ToLower());
            for (int i = 0; i < ByteArray.Count(); i++)
            {
                if (ByteArray[i] <= 0x39)
                    ByteArray[i] -= 0x30;
                else
                    ByteArray[i] -= 0x57;
            }
            return ByteArray;
        }

        public static string CStrRemoveNullandEmptyAndReturn(this string _input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] ping = Encoding.UTF8.GetBytes(_input);
            Queue<byte> queue = new Queue<byte>();
            foreach (byte item in ping)
            {
                if (item != 0x00 && item != 0x0a && item != 0x20)
                {
                    queue.Enqueue(item);
                }
            }
            if (queue.Count % 3 == 0)
            {
                byte[] resultPing = new byte[3];
                while (queue.Count != 0)
                {
                    resultPing[0] = queue.Dequeue();
                    resultPing[1] = queue.Dequeue();
                    resultPing[2] = queue.Dequeue();
                    stringBuilder.Append(Encoding.UTF8.GetString(resultPing));
                }
                return stringBuilder.ToString();
            }
            return stringBuilder.ToString();
        }

        /// <summary>  
        /// 将数字1-9转化为汉字  
        /// </summary>  
        /// <param name="num">范围1-9</param>  
        /// <returns></returns>  
        public static string GetStrByInt(int num)
        {
            string strResult = string.Empty;

            switch (num)
            {
                case 1:
                    strResult = "一"; break;
                case 2:
                    strResult = "二"; break;
                case 3:
                    strResult = "三"; break;
                case 4:
                    strResult = "四"; break;
                case 5:
                    strResult = "五"; break;
                case 6:
                    strResult = "六"; break;
                case 7:
                    strResult = "七"; break;
                case 8:
                    strResult = "八"; break;
                case 9:
                    strResult = "九"; break;
                default:
                    strResult = ""; break;
            }

            return strResult;
        }


    }
}
