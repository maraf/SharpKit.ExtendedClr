using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Client.Clr.System.ComponentModel
{
    [JsType(Name = "System.ComponentModel.TypeDescriptor")]
    internal class JsImplTypeDescriptor
    {
        [JsMethod(Name = "GetConverter$$Type")]
        public static TypeConverter GetConverter(Type type)
        {
            return null;
        }
    }
}
