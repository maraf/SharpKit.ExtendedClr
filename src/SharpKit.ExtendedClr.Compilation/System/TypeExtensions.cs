using SharpKit.ExtendedClr.Compilation;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {
        [JsMethod(Code = "return type._JsType;")]
        public static JsType GetJsType(this Type type)
        {
            return null;
        }
    }
}
