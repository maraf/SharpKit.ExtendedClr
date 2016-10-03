﻿using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Client.System
{
    [JsType(Name = "System.IComparable")]
    internal interface JsImplIComparable
    {
        int CompareTo(object obj);
    }
}
