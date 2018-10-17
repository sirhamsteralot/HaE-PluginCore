using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore.Console
{
    public class HaEConsoleCommand
    {
        public string Command;
        public string Description;
        public Func<List<string>, string> Action;

        public HaEConsoleCommand(string command, string description, Func<List<string>, string> action)
        {
            Command = command;
            Description = description;
            Action = action;
        }

        public HaEConsoleCommand(string command, Func<List<string>, string> action)
        {
            Command = command;
            Description = "";
            Action = action;
        }
    }
}
