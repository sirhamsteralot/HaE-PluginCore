using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HaEPluginCore
{
    public class HaEILInjector
    {
        public static void Replace<T1,T2>(int funcNum, string targetMethod, string injectionMethod)
        {
            MethodInfo methodToReplace = typeof(T1).GetMethod(targetMethod + funcNum, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeof(T2).GetMethod(injectionMethod + funcNum, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;

                    *tar = *inj;
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;

                    *tar = *inj;
                }
            }
        }
    }
}
