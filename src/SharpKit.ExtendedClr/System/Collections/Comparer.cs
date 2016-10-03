using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SharpKit.JavaScript;

namespace System.Collections
{

    [JsType(JsMode.Clr, Name = "System.Collections.Comparer")]
    public abstract class JsImplComparer : IComparer
    {
        public abstract int Compare(object x, object y);

        private static JsImplComparer _default;
        public static JsImplComparer Default
        {
            get
            {
                if (_default == null) _default = new DefaultComparer();
                return _default;
            }
        }
    }


    [JsType(JsMode.Clr)]
    class DefaultComparer : JsImplComparer
    {
        public override int Compare(object x, object y)
        {
            return x.As<IComparable>().CompareTo(y);
        }
    }
}
