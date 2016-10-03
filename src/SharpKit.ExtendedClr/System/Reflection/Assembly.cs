using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    [JsType(Name = "System.Reflection.Assembly")]
    internal class JsImplAssembly
    {
        private readonly JsImplAssemblyName name;

        public JsImplAssembly(JsImplAssemblyName name)
        {
            this.name = name;
        }

        internal JsImplAssemblyName GetName()
        {
            return name;
        }
    }
}
