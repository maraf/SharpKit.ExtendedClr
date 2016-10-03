using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKit.JavaScript
{
    [JsType(JsMode.Prototype, Name = "Function", Export = false)]
    public class JsDelegateFunction : JsFunction
    {
        public JsFunction func { get; set; }
        public object target { get; set; }
        public bool isDelegate { get; set; }

        public JsArray<JsDelegateFunction> delegates { get; set; }
        public bool isMulticastDelegate { get; set; }
    }
}
