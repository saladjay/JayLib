using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JayLib.JayString
{
    public class StringRegex
    {
        /// <summary>
        /// replace "{0}" with attribute's name
        /// </summary>
        public static string AttributeContent = @"\s+{0}\s*=\s*""\w*\s*""\s*";


        /// <summary>
        /// TODO:提取节点的正则表达式，例如：<run></run>
        /// </summary>
        public static string NodeContent = "";

        /// <summary>
        /// TODO:提取没有内容的节点的正则表达式，例如：<run/>
        /// </summary>
        public static string NodeWithoutContent = "";
    }
}
