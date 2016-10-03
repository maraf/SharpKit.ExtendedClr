using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    internal static class PropertyInfoExtensions
    {
        [JsMethod(Code = "return pi._IsStatic;")]
        public static bool IsStatic(this PropertyInfo pi)
        {
            return pi.GetAccessors()[0].IsStatic;
        }
        [JsMethod(Code = "throw new Error('Not Implemented');")]
        public static bool IsPublic(this PropertyInfo pi)
        {
            return (pi.GetAccessors()[0].Attributes & MethodAttributes.Public) == MethodAttributes.Public;
        }
    }
}
