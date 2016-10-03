using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKit.JavaScript;

namespace System.Collections.Generic
{
    [JsType(Name = "System.Collections.Generic.JsArrayEnumerator$1")]
    class JsArrayEnumerator<T> : JsImplIEnumerator<T>
    {
        public JsArrayEnumerator(JsArray<T> list)
        {
            List = list;
            Index = -1;
            ListCount = list.length;
        }
        JsArray<T> List;
        int Index;
        int ListCount;
        #region IEnumerator<T> Members

        public T Current
        {
            get
            {
                return List[Index];
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            List = null;
        }

        #endregion

        #region IEnumerator Members

        public bool MoveNext()
        {
            Index++;
            return Index < ListCount;
        }

        public void Reset()
        {
            Index = -1;
        }

        #endregion


    }

}
