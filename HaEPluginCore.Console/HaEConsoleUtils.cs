using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore.Console
{
    public static class HaEConsoleUtils
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
