using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        [JsMethod(Export = false, ExtensionImplementedInInstance = true, NativeOverloads = true)]
        public static bool startsWith(this JsString s, JsString s2)
        {
            return false;
        }
        [JsMethod(Export = false, ExtensionImplementedInInstance = true, NativeOverloads = true)]
        public static bool endsWith(this JsString s, JsString s2)
        {
            return false;
        }

        [JsMethod(Code = "if(s==null || s.length==0) return defaultValue; return s;")]
        public static string GetValueOrDefaultIfNullOrEmpty(this string s, string defaultValue)
        {
            if (s == null || s.Length == 0)
                return defaultValue;
            return s;
        }

        [JsMethod(Export = false)]
        public static bool IsNullOrEmpty(this string s)
        {
            return s == null || s.Length == 0;
        }

        [JsMethod(Export = false)]
        public static bool IsNotNullOrEmpty(this string s)
        {
            return s != null && s.Length > 0;
        }

        public static bool IsNullOrEmpty(this JsString s)
        {
            return s == null || s.length == 0;
        }

        public static bool IsNotNullOrEmpty(this JsString s)
        {
            return s != null && s.length > 0;
        }

        public static string HtmlEscape(this string s)
        {
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
        }

        [JsMethod(Code = "return s.ReplaceFirst(search, replace);")]
        public static string ReplaceFirst(this string s, string search, string replace)
        {
            return ReplaceFirst(s, search, replace, StringComparison.CurrentCulture);
        }
        public static string ReplaceFirst(this string s, string search, string replace, StringComparison comparisonType)
        {
            int index = s.IndexOf(search, comparisonType);
            if (index != -1)
            {
                string finalStr = String.Concat(s.Substring(0, index), replace, s.Substring(search.Length + index));
                return finalStr;
            }
            return s;
        }

        public static string FixCamelCasing(this string s)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (char c in s)
            {
                if (Char.IsUpper(c) && !first)
                {
                    sb.Append(' ');
                }
                sb.Append(c);
                first = false;
            }
            return sb.ToString();
        }

        [JsMethod(Code = "return s.substr(s, s.length-count);")]
        public static string RemoveLast(this string s, int count)
        {
            return s.Substring(0, s.Length - count);
        }

        public static string TrimEnd(this string s, string trimText)
        {
            if (s.EndsWith(trimText))
                return RemoveLast(s, trimText.Length);
            return s;
        }



        public static bool EqualsIgnoreCase(this string s1, string s2)
        {
            return String.Compare(s1, s2, true) == 0;
        }
    }
}
