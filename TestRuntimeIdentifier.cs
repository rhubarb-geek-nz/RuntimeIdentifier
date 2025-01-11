// Copyright (c) 2024 Roger Brown.
// Licensed under the MIT License.

using System.Management.Automation;

namespace RhubarbGeekNz.RuntimeIdentifier
{
    [Cmdlet(VerbsDiagnostic.Test, "RuntimeIdentifier")]
    [OutputType(typeof(bool))]
    sealed public class TestRuntimeIdentifier : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Runtime Identifier", Position = 0)]
        public string RuntimeIdentifier;

        protected override void ProcessRecord()
        {
            bool result = GetRuntimeIdentifier.ForEachRuntimeIdentifier(s => s.Equals(RuntimeIdentifier));
            WriteObject(result);
        }
    }
}
