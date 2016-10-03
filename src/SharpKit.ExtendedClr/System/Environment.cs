using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	[JsType(Name = "System.Environment")]
	class JsImplEnvironment
	{
		internal static string GetResourceString(string p)
		{
			return p;
		}

        private const string newLine = "\r\n";
        public static string NewLine { get { return newLine; } }

        public static int TickCount { get { return new JsDate().valueOf(); } }

	}

}
