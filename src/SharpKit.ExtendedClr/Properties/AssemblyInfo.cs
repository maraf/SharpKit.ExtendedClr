using SharpKit.JavaScript;
using System.Client;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Neptuo.Client.Clr;
using SharpKit.JavaScript;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Neptuo.System.Client")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Neptuo.System.Client")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d1e2d5d1-2de4-4f19-bbce-30e6e5a132bf")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(VersionInfo.Version)]
[assembly: AssemblyInformationalVersion(VersionInfo.Version)]
[assembly: AssemblyFileVersion(VersionInfo.Version)]

[assembly: JsExport(DefaultFilename = "Core.js", UseStrict = true)]
[assembly: JsType(JsMode.Clr)]

[assembly: JsMergedFile(Filename = "SharpKit.ExtendedClr.js", Sources = new string[]
{
	"Core.js",
	"CoreEx.js",
})]
