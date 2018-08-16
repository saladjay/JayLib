using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JayLib.Localization
{
    public abstract class LocalizationManager
    {
        private static string _Language;
        public static string Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
                UpdateLanguage();
            }
        }

        static List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();

        static LocalizationManager()
        {
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }
        }

        /// <summary>
        /// 更换语言包
        /// </summary>
        private static void UpdateLanguage()
        {
            string requestedLanguage = string.Format(@"Language\StringResource.{0}.xaml", Language);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            if (resourceDictionary == null)
            {
                requestedLanguage = @"Language\StringResource.en-US.xaml";
                resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            }
            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }



        public static string GetString(string key)
        {
            if (Application.Current.FindResource(key) is string str)
            {
                return str;
            }
            else
            {
                return null;
            }
        }

        public static string GetString(string key, string language)
        {

            string requestedLanguage = string.Format(@"Language\StringResource.{0}.xaml", language);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            if (resourceDictionary == null)
            {
                return null;
            }
            else
            {
                return resourceDictionary[key].ToString();
            }
        }

        public static bool ChangeString(string key, string value, string language)
        {
            string requestedLanguage = string.Format(@"Language\StringResource.{0}.xaml", language);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            if (resourceDictionary == null)
            {
                return false;
            }
            else
            {
                if (resourceDictionary.Contains(key))
                {
                    resourceDictionary[key] = value;
                    return true;
                }
            }
            return false;
        }

        public static void AddString(string key, string value, string language, bool isOverWrite = true)
        {
            string requestedLanguage = string.Format(@"Language\StringResource.{0}.xaml", language);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            if (resourceDictionary.Contains(key))
            {
                if (isOverWrite)
                {
                    resourceDictionary[key] = value;
                }
            }
            else
            {
                resourceDictionary.Add(key, value);
            }
        }
    }
}
