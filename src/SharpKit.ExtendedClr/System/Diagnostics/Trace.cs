using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;


namespace System.Diagnostics
{

    [JsType(Name = "System.Diagnostics.Trace")]
	internal static class JsImplTrace
	{
		static List<string> Warnings;
		static bool Enabled=false; //TODO:
		public static void TraceWarning(string msg)
		{
			if (!Enabled)
				return;
			if (Warnings == null)
				Warnings = new List<string>();
			Warnings.Add(msg);
		}

		public static void TraceWarning(string format, params object[] args)
		{
			if (!Enabled)
				return;
			TraceWarning(String.Format(format, args));
		}

	}
}
