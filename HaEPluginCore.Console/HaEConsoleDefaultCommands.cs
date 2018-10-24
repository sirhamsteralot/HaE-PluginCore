using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using VRage;
using VRage.Steam;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System.Reflection;

namespace HaEPluginCore.Console
{
    public class HaEConsoleDefaultCommands
    {
        public static void RegisterCommands()
        {
            // Core commands
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Help", "Lists all available commands", Help));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Hello", "Just a test command", HelloWorld));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Clear", "Clears the console", (List<string> x) => { HaEConsole.Instance.Clear(); return ""; }));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Exit", "Exits the game", (List<string> x) => { Environment.Exit(0); return $"Exiting!"; }));

            // Utilities
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OpenContainer", "Opens a competitive container", (List<string> x) => { MySteamService.Static.TriggerPersonalContainer(); return "Crate opened!"; }));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OpenCompContainer", "Opens a competitive container", (List<string> x) => { MySteamService.Static.TriggerCompetitiveContainer(); return $"Crate opened!"; }));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Eval", "Runs C# script", HandleAsync));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("RemoveBlockInfo", "Removes block info", RemoveBlockInfo));

        }

        public static string RemoveBlockInfo(List<string> arg)
        {
            FieldInfo field = typeof(MyGuiScreenHudSpace).GetField("m_contextHelp", BindingFlags.NonPublic | BindingFlags.Instance);
            MyGuiControlContextHelp control = field.GetValue(MyGuiScreenHudSpace.Static) as MyGuiControlContextHelp;

            if (control == null)
                return "blockInfo is null!";

            control.Alpha = 0;

            return $"Removed!";
        }

        public static string HandleAsync(string arg)
        {
            return Eval(arg).GetAwaiter().GetResult();
        }
        public async static Task<string> Eval(string arg)
        {
            try
            {
                var result = await HaEConsoleUtils.ScriptManager.ExecuteScript(arg);
                return result?.ToString();

            } catch (CompilationErrorException ex)
            {
                return  $"Error executing script!\n" +
                        $"{ex.Message}";
            }
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
