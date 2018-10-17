using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace HaEPluginCore.Console
{
    public class HaEConsoleUtils
    {
        public static List<string> SplitArgs(string input)
        {
            return input.Split(new char[]
            {
                '"'
            }).Select(delegate (string element, int index)
            {
                if (index % 2 != 0)
                {
                    return new string[]
                    {
                element
                    };
                }
                return element.Split(new char[]
                {
                    ' '
                }, StringSplitOptions.RemoveEmptyEntries);
            }).SelectMany((string[] element) => element).ToList<string>();
        }

        public static class ScriptManager
        {
            static readonly Dictionary<string, AssemblyName> AssemblyNames = new Dictionary<string, AssemblyName>();

            public static async Task<object> ExecuteScript(string code)
            {
                var op = ScriptOptions.Default
                                      .WithReferences("System.Runtime, Version = 4.0.0.0, Culture = neutral, PublicKeyToken = b03f5f7f11d50a3a")
                                      .WithReferences(typeof(List<>).Assembly, typeof(Enumerable).Assembly, typeof(string).Assembly, typeof(HaEPluginCore).Assembly, typeof(StringBuilder).Assembly)
                                      .WithImports("System", "System.Collections.Generic", "System.Timers", "System.Linq", "System.Text", "HaEPluginCore");

                var res = await CSharpScript.EvaluateAsync<object>(code, op);
                GC.Collect();

                return res?.ToString();
            }

            private static IEnumerable<Assembly> SelectAssemblies()
            {
                return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Where(a => !string.IsNullOrWhiteSpace(a.Location));
            }
        }
    }
}
