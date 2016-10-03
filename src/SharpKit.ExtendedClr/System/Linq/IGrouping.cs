using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SharpKit.JavaScript;

namespace System.Linq
{

    [JsType(JsMode.Clr, Name = "System.Linq.IGrouping$2")]
    public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable
    {
        TKey Key { get; }
    }
}
