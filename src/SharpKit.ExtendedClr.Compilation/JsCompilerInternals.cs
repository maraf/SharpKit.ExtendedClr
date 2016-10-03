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

    internal class JsCompilerFunction : JsFunction
    {
        public JsType _type;
        public string _name;
        //TODO:?[Obsolete("?")]
        public string name=null;
    }

    internal class JsCompilerObject2 : JsObject
    {
        public string getTypeName()
        {
            throw new NotImplementedException();
        }
    }
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
        protected static BrowserWindow window { get; set; }
    }
    [JsType(JsMode.Prototype, Export = false)]
    public class BrowserWindow
    {
        public void setTimeout(JsAction callback, JsNumber ms)
        {
        }

        internal BrowserWindow open(string url, string p, string p_2)
        {
            throw new NotImplementedException();
        }
    }

}
