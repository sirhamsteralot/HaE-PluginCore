using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpaceEngineers.Game;

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
    }
}
