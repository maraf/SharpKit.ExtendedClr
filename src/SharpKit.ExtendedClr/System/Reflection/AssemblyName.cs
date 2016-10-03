using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    [JsType(Name = "System.Reflection.AssemblyName")]
    internal class JsImplAssemblyName
    {
        public string Name { get; set; }
    }
}
