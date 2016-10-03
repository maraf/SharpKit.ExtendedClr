using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Client
{
    [JsType(JsMode.Clr)]
    public static class VersionInfo
    {
        internal const string Version = "4.5.0";
        internal const string Preview = "-beta1";

        public static Version GetVersion()
        {
            return new Version(Version);
        }
    }
}
