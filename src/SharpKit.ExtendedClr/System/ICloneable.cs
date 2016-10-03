using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKit.JavaScript;

namespace System
{
    [JsType(Name = "System.ICloneable")]
    public interface JsImplICloneable
    {
        object Clone();
    }
}
