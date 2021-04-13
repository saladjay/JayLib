using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex regex = new Regex(string.Format(JayLib.JayString.StringRegex.AttributeContent, "Name"));
            Dictionary<string, int> nameDict = new Dictionary<string, int>();
            var inputString = @"<RegexTest m_strCaseName=""></RegexTest>
<RegexTest m_strCaseName=""content""></RegexTest>
< RegexTest m_strCaseName = ""content"" ></ RegexTest >
< RegexTest Name =""content"" ></ RegexTest >
 < RegexTest Name = """" ></ RegexTest >
  < RegexTest Name = ""image"" ></ RegexTest >
   < RegexTest Name ></ RegexTest >
    < RegexTest Name=""image"" />
< RegexTest Name=""添加到源代码管理"" />
     < RegexTest Name = ""image"" /> ";
            var resStringBuilder = new StringBuilder(inputString);

            Regex regex1 = new Regex(@"\s*""\w*""");
            Regex regex2 = new Regex("[\\u4E00-\\u9FA5A-Za-z0-9_]+");
            var removeCount = 0;
            foreach (Match mch in Regex.Matches(inputString, string.Format(@"\s+{0}\s*=\s*""\w*""\s*", "Name"), RegexOptions.Multiline))
            {
                string x = mch.Value;
                var mch1 = regex1.Match(x).Value;
                var mch2 = regex2.Match(mch1).Value;
                Console.WriteLine(mch2);
                if (string.IsNullOrWhiteSpace(mch2)|| nameDict.ContainsKey(mch2))
                {
                    inputString = inputString.Remove(mch.Index - removeCount, mch.Length);
                    removeCount += mch.Length;
                }
                else
                {
                    nameDict[mch2] = 1;
                }
            }
            Console.WriteLine(inputString);
            Console.ReadKey();
        }
    }
}
