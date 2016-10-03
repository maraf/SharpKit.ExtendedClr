using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ArrayExtensions
    {
        [JsMethod(Code = "return array==null || array.length==0;")]
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }

        [JsMethod(Code = "return array!=null && array.length>0;")]
        public static bool IsNotNullOrEmpty(this Array array)
        {
            return array != null && array.Length > 0;
        }
    }
}
