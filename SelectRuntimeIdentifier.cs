// Copyright (c) 2024 Roger Brown.
// Licensed under the MIT License.

using System.Management.Automation;

namespace RhubarbGeekNz.RuntimeIdentifier
{
    [Cmdlet(VerbsCommon.Select, "RuntimeIdentifier")]
    [OutputType(typeof(string))]
    sealed public class SelectRuntimeIdentifier : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Runtime Identifier", Position = 0)]
        public string[] RuntimeIdentifier;

        protected override void ProcessRecord()
        {
            GetRuntimeIdentifier.ForEachRuntimeIdentifier(s => { Select(s); return false; });
        }

        private void Select(string s)
        {
            if (RuntimeIdentifier != null)
            {
                foreach (var i in RuntimeIdentifier)
                {
                    if (s.Equals(i))
                    {
                        WriteObject(s);
                    }
                }
            }
        }
    }
}
