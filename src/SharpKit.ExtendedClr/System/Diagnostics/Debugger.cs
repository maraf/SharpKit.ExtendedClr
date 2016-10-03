using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Diagnostics
{
    [JsType(Name = "System.Diagnostics.Debugger")]
	public class JsImplDebugger
	{
		[JsMethod(Code = "debugger;")]
		public static void Break()
		{
		}
	}
}
