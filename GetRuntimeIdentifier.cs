// Copyright (c) 2024 Roger Brown.
// Licensed under the MIT License.

using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace RhubarbGeekNz.RuntimeIdentifier
{
    [Cmdlet(VerbsCommon.Get, "RuntimeIdentifier")]
    [OutputType(typeof(string))]
    sealed public class GetRuntimeIdentifier : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            ForEachRuntimeIdentifier(s => { WriteObject(s); return false; });
        }

        internal delegate bool RuntimeDelegate(string s);

        internal static bool ForEachRuntimeIdentifier(RuntimeDelegate runtimeDelegate)
        {
            string rid = RuntimeInformation.RuntimeIdentifier;
            bool result = runtimeDelegate(rid);

            if (!result)
            {
                bool found = false;

                char dsc = Path.DirectorySeparatorChar;

                try
                {
                    var exe = Assembly.GetEntryAssembly().Location;
                    int dot = exe.LastIndexOf('.');
                    int slash = exe.LastIndexOf(dsc);

                    if (dot > slash)
                    {
                        exe = exe.Substring(0, dot);
                    }

                    string exeJson = exe + ".deps.json";

                    using (var stream = File.OpenRead(exeJson))
                    {
                        var doc = JsonDocument.Parse(stream);

                        if (doc.RootElement.TryGetProperty("runtimes", out var runtimes))
                        {
                            if (runtimes.TryGetProperty(rid, out var runtimeList))
                            {
                                foreach (var runtime in runtimeList.EnumerateArray())
                                {
                                    result = runtimeDelegate(runtime.ToString());

                                    if (result)
                                    {
                                        break;
                                    }
                                }
                            }

                            found = true;
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                }

                if (!found)
                {
                    var exe = typeof(string).Assembly.Location;
                    var path = exe.Split(dsc);
                    int i = path.Length - 1;

                    while (i > 0)
                    {
                        string file = path[--i];
                        path[path.Length - 1] = file + ".deps.json";
                        file = string.Join(dsc, path);

                        try
                        {
                            using (var stream = File.OpenRead(file))
                            {
                                var doc = JsonDocument.Parse(stream);

                                if (doc.RootElement.TryGetProperty("runtimes", out var runtimes))
                                {
                                    if (runtimes.TryGetProperty(rid, out var runtimeList))
                                    {
                                        foreach (var runtime in runtimeList.EnumerateArray())
                                        {
                                            result = runtimeDelegate(runtime.ToString());

                                            if (result)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        }
                        catch (FileNotFoundException)
                        {
                        }
                    }
                }
            }

            return result;
        }
    }
}
