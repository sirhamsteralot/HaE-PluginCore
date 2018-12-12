using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using Harmony;

namespace HaEPluginCore
{
    public class HaEPatcher
    {
        public static void ReplaceMethod(MethodInfo methodToReplace, MethodInfo methodToInject)
        {
            var harmony = HarmonyInstance.Create("com.HaE.HaECore.PluginCore");
            harmony.Patch(methodToReplace, new HarmonyMethod(methodToInject));
        }
    }
}
