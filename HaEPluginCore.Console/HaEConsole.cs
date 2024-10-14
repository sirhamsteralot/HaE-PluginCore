using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Plugins;

namespace HaEPluginCore.Console
{
    public class HaEConsole : IPlugin
    {
        private static HaEConsole _instance;
        public static HaEConsole Instance => _instance;
        private static event Action OnPluginReady;


        public HaEConsoleCommandBinder CommandBinder;

        private StringBuilder _displayScreen;
        public StringBuilder displayScreen => _displayScreen;

        private LinkedList<string> _commandHistory;
        private LinkedListNode<string> _position = null;

        public Dictionary<string, HaEConsoleCommand> commands;

        public void Init(object gameInstance)
        {
            _instance = this;

            HaEInputHandler.HaEKeyCombination enter = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.C, VRage.Input.MyKeys.RightAlt, VRage.Input.MyKeys.None, HaEConstants.quarterSecTimeOut, HaEConsoleScreen.Show);
            HaEPluginCore.HaEInputHandler.AddCombination(enter);

            HaEInputHandler.HaEKeyCombination exit = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.Escape, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, HaEConstants.quarterSecTimeOut, HaEConsoleScreen.Close);
            HaEPluginCore.HaEInputHandler.AddCombination(exit);

            _displayScreen = new StringBuilder();
            _commandHistory = new LinkedList<string>();
            commands = new Dictionary<string, HaEConsoleCommand>();

            CommandBinder = new HaEConsoleCommandBinder();

            HaEConsoleScreen.RegisterKeys();
            HaEConsoleDefaultCommands.RegisterCommands();

            OnPluginReady?.Invoke();
        }

        public static void ExecWhenPluginReady(Action action)
        {
            if (_instance == null)
            {
                OnPluginReady += action;
                return;
            }

            action();
        }

        public static void WriteLine(string line)
        {
            Instance.displayScreen.Append(line).AppendLine();
        }

        public void RegisterCommand(HaEConsoleCommand command)
        {
            if (!commands.ContainsKey(command.Command))
                commands.Add(command.Command, command);
        }

        public void UnregisterCommand(HaEConsoleCommand command)
        {
            if (commands.ContainsKey(command.Command))
                commands.Remove(command.Command);
        }
        public void UnregisterCommand(string command)
        {
            if (commands.ContainsKey(command))
                commands.Remove(command);
        }

        public void ParseCommand(string command)
        {
            if (_position == null)
            {
                _commandHistory.AddLast(command);
            } else
            {
                _commandHistory.AddAfter(_position, command);
                _position = _position.Next;
            }

            _displayScreen.AppendLine(command);
            _displayScreen.Append(HandleCommand(command)).AppendLine();
        }

        public StringBuilder HandleCommand(string command)
        {
            List<string> split = HaEConsoleUtils.SplitArgs(command);

            StringBuilder sb = new StringBuilder();

            if (split.Count <= 0)
            {
                return sb.Append("Error, Empty!");
            }

            string key = split[0];
            HaEConsoleCommand consoleCommand;
            if (!commands.TryGetValue(key, out consoleCommand))
                return sb.Append($"Error, command {key} not found!");

            split.RemoveAt(0);
            string result = "";
            try
            {
                if (consoleCommand.RequireFullArg)
                {
                    string input = command.Substring(key.Length + 1);
                    result = consoleCommand.FullArgAction.Invoke(input);
                }
                else
                {
                    result = consoleCommand.Action.Invoke(split);
                }
            } catch (Exception e)
            {
                sb.Append("Command exception: ").AppendLine(e.Message);
                sb.Append(e.StackTrace);
            }


            if (!string.IsNullOrEmpty(result))
                return sb.Append(result);

            return sb;
        }

        public void NextLine()
        {
            if (_position != null)
            {
                _position = _position.Next;
            }
        }

        public void PreviousLine()
        {
            if (_position == null)
            {
                _position = _commandHistory.Last;
                return;
            }
            if (_position != _commandHistory.First)
            {
                _position = _position.Previous;
            }
        }

        public string GetLine()
        {
            if (_position == null)
            {
                return "";
            }
            return _position.Value;
        }

        public void Clear()
        {
            _displayScreen.Clear();
        }

        public void Update()
        {

        }

        public void Dispose()
        {

        }
    }
}
