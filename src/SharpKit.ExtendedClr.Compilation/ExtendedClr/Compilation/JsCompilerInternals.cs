using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKit.JavaScript;

namespace SharpKit.ExtendedClr.Compilation
{
    internal static class Extensions
    {
        [JsMethod(ExtensionImplementedInInstance = true)]
        public static JsArray from(this JsArguments args, int index)
        {
            throw new NotImplementedException();
        }
    }


    [JsType(JsMode.Json)]
    class JsPrimitive
    {
        public JsFunc<JsNumber> valueOf { get; set; }

    }

    [JsType(Export = false)]
    internal class JsCompilerFunction : JsFunction
    {
        public JsType _type;
        public string _name;
        //TODO:?[Obsolete("?")]
        public string name=null;
    }

    [JsType(Export = false)]
    internal class JsCompilerObject2 : JsObject
    {
        public string getTypeName()
        {
            throw new NotImplementedException();
        }
    }

    [JsType(Export = false)]
    internal class JsCompilerPrototype : JsObject
    {
        public new JsCompilerFunction toString;
    }

    [JsType(JsMode.Prototype, Name = "Object", Export = false)]
    internal class JsCompilerObject : JsObject
    {
        public static JsCompilerPrototype prototype { get; set; }
        public new JsCompilerFunction toString { get; set; }
        public JsCompilerFunction getTypeName { get; set; }
        public JsString _hashKey { get; set; }
        public JsObject<JsDelegateFunction> __delegateCache { get; set; }
    }

    [JsType(JsMode.Global, Export = false)]
    public class BrowserContext : JsContextBase
    {
        protected static JsObject window { get; set; }
    }
}
