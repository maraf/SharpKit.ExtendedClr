using System;
using System.Collections.Generic;

using System.Text;

using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security.Cryptography;
using System.IO;
using SharpKit.JavaScript;

namespace System.IO
{
    class IOGlobal
    {
        //TODO: HACK: see Issue #43
        [JsMethod(Code = @"AfterCompilation(function()
{
	System.IO.Path.ctor();
});
", GlobalCode = true)]
        static void Global()
        {

        }
    }

    [JsType(Name = "System.IO.Path")]
    internal static class JsImplPath
    {
        // Fields
        public static readonly char AltDirectorySeparatorChar = '/';
        public static readonly char DirectorySeparatorChar = '\\';
        private static readonly char[] InvalidFileNameChars = new char[] { 
        '"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', 
        '\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001a', '\x001b', 
        '\x001c', '\x001d', '\x001e', '\x001f', ':', '*', '?', '\\', '/'
     };
        [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public static readonly char[] InvalidPathChars = new char[] { 
        '"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', 
        '\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001a', '\x001b', 
        '\x001c', '\x001d', '\x001e', '\x001f'
     };
        internal const int MAX_DIRECTORY_PATH = 0xf8;
        internal const int MAX_PATH = 260;
        internal static readonly int MaxPath = 260;
        public static readonly char PathSeparator = ';';
        private static readonly char[] RealInvalidPathChars = new char[] { 
        '"', '<', '>', '|', '\0', '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\a', '\b', '\t', '\n', '\v', 
        '\f', '\r', '\x000e', '\x000f', '\x0010', '\x0011', '\x0012', '\x0013', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019', '\x001a', '\x001b', 
        '\x001c', '\x001d', '\x001e', '\x001f'
     };
        public static readonly char VolumeSeparatorChar = ':';

        public static string ChangeExtension(string path, string extension)
        {
            if (path == null)
            {
                return null;
            }
            CheckInvalidPathChars(path);
            string str = path;
            int length = path.Length;
            while (--length >= 0)
            {
                char ch = path[length];
                if (ch == '.')
                {
                    str = path.Substring(0, length);
                    break;
                }
                if (((ch == DirectorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == VolumeSeparatorChar))
                {
                    break;
                }
            }
            if ((extension == null) || (path.Length == 0))
            {
                return str;
            }
            if ((extension.Length == 0) || (extension[0] != '.'))
            {
                str = str + ".";
            }
            return (str + extension);
        }


        private static bool CharArrayStartsWithOrdinal(char[] array, int numChars, string compareTo, bool ignoreCase)
        {
            if (numChars < compareTo.Length)
            {
                return false;
            }
            if (ignoreCase)
            {
                string str = new string(array, 0, compareTo.Length);
                return compareTo.Equals(str, StringComparison.OrdinalIgnoreCase);
            }
            for (int i = 0; i < compareTo.Length; i++)
            {
                if (array[i] != compareTo[i])
                {
                    return false;
                }
            }
            return true;
        }

        internal static void CheckInvalidPathChars(string path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                int num2 = path.As<JsString>().charCodeAt(i);
                if (((num2 == 0x22) || (num2 == 60)) || (((num2 == 0x3e) || (num2 == 0x7c)) || (num2 < 0x20)))
                {
                    throw new ArgumentException(JsImplEnvironment.GetResourceString("Argument_InvalidPathChars"));
                }
            }
        }

        internal static void CheckSearchPattern(string searchPattern)
        {
            int num;
            while ((num = searchPattern.IndexOf("..", StringComparison.Ordinal)) != -1)
            {
                if ((num + 2) == searchPattern.Length)
                {
                    throw new ArgumentException(JsImplEnvironment.GetResourceString("Arg_InvalidSearchPattern"));
                }
                if ((searchPattern[num + 2] == DirectorySeparatorChar) || (searchPattern[num + 2] == AltDirectorySeparatorChar))
                {
                    throw new ArgumentException(JsImplEnvironment.GetResourceString("Arg_InvalidSearchPattern"));
                }
                searchPattern = searchPattern.Substring(num + 2);
            }
        }

        public static string Combine(string path1, string path2)
        {
            if ((path1 == null) || (path2 == null))
            {
                throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
            }
            CheckInvalidPathChars(path1);
            CheckInvalidPathChars(path2);
            if (path2.Length == 0)
            {
                return path1;
            }
            if (path1.Length == 0)
            {
                return path2;
            }
            if (IsPathRooted(path2))
            {
                return path2;
            }
            char ch = path1[path1.Length - 1];
            if (((ch != DirectorySeparatorChar) && (ch != AltDirectorySeparatorChar)) && (ch != VolumeSeparatorChar))
            {
                return (path1 + DirectorySeparatorChar + path2);
            }
            return (path1 + path2);
        }
        
        public static string GetDirectoryName(string path)
        {
            if (path != null)
            {
                CheckInvalidPathChars(path);
                int rootLength = GetRootLength(path);
                if (path.Length > rootLength)
                {
                    int length = path.Length;
                    if (length == rootLength)
                    {
                        return null;
                    }
                    while (((length > rootLength) && (path[--length] != DirectorySeparatorChar)) && (path[length] != AltDirectorySeparatorChar))
                    {
                    }
                    return path.Substring(0, length);
                }
            }
            return null;
        }

        public static string GetExtension(string path)
        {
            if (path == null)
            {
                return null;
            }
            CheckInvalidPathChars(path);
            int length = path.Length;
            int startIndex = length;
            while (--startIndex >= 0)
            {
                char ch = path[startIndex];
                if (ch == '.')
                {
                    if (startIndex != (length - 1))
                    {
                        return path.Substring(startIndex, length - startIndex);
                    }
                    return string.Empty;
                }
                if (((ch == DirectorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == VolumeSeparatorChar))
                {
                    break;
                }
            }
            return String.Empty;
        }

        public static string GetFileName(string path)
        {
            if (path != null)
            {
                CheckInvalidPathChars(path);
                int length = path.Length;
                int num2 = length;
                while (--num2 >= 0)
                {
                    char ch = path[num2];
                    if (((ch == DirectorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == VolumeSeparatorChar))
                    {
                        return path.Substring(num2 + 1, (length - num2) - 1);
                    }
                }
            }
            return path;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            path = GetFileName(path);
            if (path == null)
            {
                return null;
            }
            int length = path.LastIndexOf('.');
            if (length == -1)
            {
                return path;
            }
            return path.Substring(0, length);
        }

        public static string GetFullPath(string path)
        {
            return path;
        }

        public static char[] GetInvalidFileNameChars()
        {
            return InvalidFileNameChars;
        }

        public static char[] GetInvalidPathChars()
        {
            return RealInvalidPathChars;
        }

        public static string GetPathRoot(string path)
        {
            if (path == null)
            {
                return null;
            }
            return path.Substring(0, GetRootLength(path));
        }

        internal static int GetRootLength(string path)
        {
            CheckInvalidPathChars(path);
            int num = 0;
            int length = path.Length;
            if ((length >= 1) && IsDirectorySeparator(path[0]))
            {
                num = 1;
                if ((length >= 2) && IsDirectorySeparator(path[1]))
                {
                    num = 2;
                    int num3 = 2;
                    while ((num < length) && (((path[num] != DirectorySeparatorChar) && (path[num] != AltDirectorySeparatorChar)) || (--num3 > 0)))
                    {
                        num++;
                    }
                }
                return num;
            }
            if ((length >= 2) && (path[1] == VolumeSeparatorChar))
            {
                num = 2;
                if ((length >= 3) && IsDirectorySeparator(path[2]))
                {
                    num++;
                }
            }
            return num;
        }

        public static string GetTempFileName()
        {
            throw new NotSupportedException();
        }

        public static string GetTempPath()
        {
            throw new NotSupportedException();
        }

        public static bool HasExtension(string path)
        {
            if (path != null)
            {
                CheckInvalidPathChars(path);
                int length = path.Length;
                while (--length >= 0)
                {
                    char ch = path[length];
                    if (ch == '.')
                    {
                        return (length != (path.Length - 1));
                    }
                    if (((ch == DirectorySeparatorChar) || (ch == AltDirectorySeparatorChar)) || (ch == VolumeSeparatorChar))
                    {
                        break;
                    }
                }
            }
            return false;
        }

        internal static string InternalCombine(string path1, string path2)
        {
            if ((path1 == null) || (path2 == null))
            {
                throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
            }
            CheckInvalidPathChars(path1);
            CheckInvalidPathChars(path2);
            if (path2.Length == 0)
            {
                throw new ArgumentException(JsImplEnvironment.GetResourceString("Argument_PathEmpty"), "path2");
            }
            if (IsPathRooted(path2))
            {
                throw new ArgumentException(JsImplEnvironment.GetResourceString("Arg_Path2IsRooted"), "path2");
            }
            int length = path1.Length;
            if (length == 0)
            {
                return path2;
            }
            char ch = path1[length - 1];
            if (((ch != DirectorySeparatorChar) && (ch != AltDirectorySeparatorChar)) && (ch != VolumeSeparatorChar))
            {
                return (path1 + DirectorySeparatorChar + path2);
            }
            return (path1 + path2);
        }

        internal static bool IsDirectorySeparator(char c)
        {
            if (c != DirectorySeparatorChar)
            {
                return (c == AltDirectorySeparatorChar);
            }
            return true;
        }

        public static bool IsPathRooted(string path)
        {
            if (path != null)
            {
                CheckInvalidPathChars(path);
                int length = path.Length;
                if (((length >= 1) && ((path[0] == DirectorySeparatorChar) || (path[0] == AltDirectorySeparatorChar))) || ((length >= 2) && (path[1] == VolumeSeparatorChar)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
