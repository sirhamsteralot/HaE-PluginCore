using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore.Console
{
    public class HaEConsoleDefaultCommands
    {
        public static void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Hello", "just a test command", HelloWorld));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Help", "Lists all available commands", Help));
        }

        public static string HelloWorld(List<string> args)
        {
            return "Hello World!";
        }

        public static string Help(List<string> args)
        {
            StringBuilder sb = new StringBuilder();

            if (args.Count > 0)
            {
                HaEConsoleCommand command;
                
                if (HaEConsole.Instance.commands.TryGetValue(args[0], out command))
                {
                    sb.Append("Showing help for command: ").AppendLine(command.Command);
                    sb.Append(command.Description);
                    return sb.ToString();
                } else
                {
                    sb.Append("Could not find help for command: ").Append(args[0]);
                    return sb.ToString();
                }
            }

            sb.AppendLine("Showing all commands: ");
            sb.AppendLine("for more details use ``Help {command}``");
            foreach (var command in HaEConsole.Instance.commands.Values)
            {
                sb.AppendLine(command.Command);
            }

            return sb.ToString();
        }
    }
}
