using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Steam;
using VRage.Utils;
using VRageMath;
using Sandbox;
using Sandbox.Engine;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using SpaceEngineers.Game;
using SpaceEngineers.Game.GUI;
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
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Exec", "Runs C# script from Script directory in plugins folder, Usage: Exec {filename}", ExecuteScript));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("RemoveBlockInfo", "Removes block info", RemoveBlockInfo));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("ChangeFOV", "Changes FOV, usage: ChangeFOV {fov}", ChangeFOV));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Connect", "Direct Connects to IP, usage: Connect {ip}", Connect));
        }

        public static string ChangeFOV(List<string> arg)
        {
            if (arg.Count < 1)
                return "not enough arguments!";

            float fovSetting;

            if (float.TryParse(arg[0], out fovSetting))
            {
                var currentsettings = MyVideoSettingsManager.CurrentGraphicsSettings;
                currentsettings.FieldOfView = MathHelper.ToRadians(fovSetting);
                MyVideoSettingsManager.Apply(currentsettings);
                MyVideoSettingsManager.SaveCurrentSettings();
                return $"Set FOV to: {fovSetting}";
            }

            return "Invalid input!";
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

        public static string ExecuteScript(List<string> args)
        {
            if (args.Count < 1)
                return "not enough arguments!";

            if (Directory.Exists($"{HaEConstants.pluginFolder}\\{HaEConstants.ScriptFolder}"))
            {
                try
                {
                    using (var reader = new StreamReader($"{HaEConstants.pluginFolder}\\{HaEConstants.ScriptFolder}\\{args[0]}"))
                    {
                        return HandleAsync(reader.ReadToEnd());
                    }
                }
                catch (FileNotFoundException e)
                {
                    return $"Could not find script to run: {args[0]}";
                }
            }

            return "ScriptDirectory was not found!";
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

        #region Connect
        private static MyGuiScreenProgress m_progressScreen;
        public static string Connect(List<string> args)
        {
            if (args.Count < 1)
                return "Not enough arguments!";

            try
            {
                string[] array = args[0].Trim().Split(new char[]
                {
                    ':'
                });
                ushort num;
                if (array.Length < 2)
                {
                    num = 27016;
                }
                else
                {
                    num = ushort.Parse(array[1]);
                }
                IPAddress[] hostAddresses = Dns.GetHostAddresses(array[0]);

                StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
                m_progressScreen = new MyGuiScreenProgress(text, new MyStringId?(MyCommonTexts.Cancel), false, true);
                MyGuiSandbox.AddScreen(m_progressScreen);
                m_progressScreen.ProgressCancelled += delegate
                {
                    CloseHandlers();
                    MySessionLoader.UnloadAndExitToMenu();
                };
                MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(ServerResponded);
                MyGameService.OnPingServerFailedToRespond += new EventHandler(ServerFailedToRespond);
                MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), num);

                MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(ServerResponded);
                MyGameService.OnPingServerFailedToRespond += new EventHandler(ServerFailedToRespond);
                MyGameService.PingServer(hostAddresses[0].ToIPv4NetworkOrder(), num);
            }
            catch (Exception ex)
            {
                MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError, MyMessageBoxStyleEnum.Error);
            }

            return "Attempting to join server: ";
        }
        private static void ServerResponded(object sender, MyGameServerItem serverItem)
        {
            CloseHandlers();
            m_progressScreen.CloseScreen();
            MyJoinGameHelper.JoinGame(serverItem, true);
        }
        private static void ServerFailedToRespond(object sender, object e)
        {
            CloseHandlers();
            m_progressScreen.CloseScreen();
            MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding, default(MyStringId), MyMessageBoxStyleEnum.Error);
        }
        private static void CloseHandlers()
        {
            MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>(ServerResponded);
            MyGameService.OnPingServerFailedToRespond -= new EventHandler(ServerFailedToRespond);
        }
        #endregion

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
                if (!command.hidden)
                    sb.AppendLine(command.Command);
            }

            return sb.ToString();
        }
    }
}
