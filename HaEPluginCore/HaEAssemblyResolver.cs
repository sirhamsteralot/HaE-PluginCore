using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;


namespace HaEPluginCore
{
    public static class HaEAssemblyResolver
    {
        static readonly Dictionary<string, AssemblyName> AssemblyNames = new Dictionary<string, AssemblyName>();

        public static void ResolveAssembliesIn(DirectoryInfo directory)
        {
            foreach (var dllFileName in directory.EnumerateFiles("*.dll"))
            {
                AssemblyName assemblyName;
                try
                {
                    assemblyName = AssemblyName.GetAssemblyName(dllFileName.FullName);
                }
                catch (BadImageFormatException)
                {
                    // Not a .NET assembly or wrong platform, ignore
                    continue;
                }
                AssemblyNames[assemblyName.FullName] = assemblyName;
            }
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            if (AssemblyNames.TryGetValue(args.Name, out AssemblyName assemblyName))
                return Assembly.Load(assemblyName);
            return null;
        }
    }
}
