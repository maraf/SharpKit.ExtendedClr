using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;


namespace System
{
	//[Flags, ComVisible(false)]

	[JsType(Name = "System.StringSplitOptions")]
	internal enum JsImplStringSplitOptions
	{
		None,
		RemoveEmptyEntries
	}

	//[JsType(Name="System.String")]
	//[RunAtClient]
	//public static class JsImplString
	//{
	//  public static string Join(string separator, string[] values)
	//  {
	//    return values.AsJsArray().join(separator);
	//  }
	//}
}
