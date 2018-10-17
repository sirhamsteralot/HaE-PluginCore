﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using VRage;
using VRage.Steam;

namespace HaEPluginCore.Console
{
    public class HaEConsoleDefaultCommands
    {
        public static void RegisterCommands()
        {
            // Core commands
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Help", "Lists all available commands", Help));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Hello", "Just a test command", HelloWorld));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Exit", "Exits the game", x => { Environment.Exit(0); return $"Exiting!"; }));

            // Utilities
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OpenContainer", "Opens a competitive container", x => { MySteamService.Static.TriggerPersonalContainer(); return "Crate opened!"; }));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("OpenCompContainer", "Opens a competitive container", x => { MySteamService.Static.TriggerCompetitiveContainer(); return $"Crate opened!"; }));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Eval", "Runs C# script", Eval));

        }

        public static string Eval(List<string> args)
        {
            string combined = "";
            foreach(string arg in args)
            {
                combined += " " + arg;
            }

            try
            {
                var result = HaEConsoleUtils.ScriptManager.ExecuteScript(combined);
                return result.ToString();

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
