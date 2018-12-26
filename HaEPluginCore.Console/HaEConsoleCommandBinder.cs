using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using HaEPluginCore;
using VRage.Input;

namespace HaEPluginCore.Console
{
    public class HaEConsoleCommandBinder
    {
        public static WriteConfig configurationWriter;

        public HaEConsoleCommandBinder()
        {
            configurationWriter = new WriteConfig();
            DeSerialize();
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("Alias", "Binds Console Command to an alias name, Usage: AddBinding {name} \"{command}\"", Alias));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("AddBinding", "Binds Console Command to key, Usage: AddBinding {name} {key} {modifier} {modifier2} \"{command}\"", AddBinding));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("RemoveBinding", "Unbinds Console Command from key", RemoveBound));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("ListBound", "Lists all bound commands", ListBound));
        }

        public string ListBound(List<string> args)
        {
            StringBuilder sb = new StringBuilder();
            List<BoundCommand> commands = configurationWriter.BoundCommands;

            sb.AppendLine("Listing bound commands: ");

            foreach (var command in commands)
            {
                if (command.shortKey)
                    sb.AppendLine($"ShortKey: {command.bindingName}: \"{command.Command}\" {command.key}+{command.modifier}+{command.modifier2}");
                else
                    sb.AppendLine($"Alias: {command.bindingName}: \"{command.Command}\"");
            }

            return sb.ToString();
        }

        public string Alias(List<string> args)
        {
            if (args.Count < 2)
                return "not enough arguments!";

            List<BoundCommand> commands = configurationWriter.BoundCommands;
            BoundCommand boundCommand = new BoundCommand();

            boundCommand.bindingName = args[0];
            boundCommand.Command = args[1];
            boundCommand.shortKey = false;

            commands.Add(boundCommand);
            BindFromBoundCommand(boundCommand);
            Save();
            return "Alias Added!";
        }

        public string AddBinding(List<string> args)
        {
            if (args.Count < 5)
                return "not enough arguments!";

            List<BoundCommand> commands = configurationWriter.BoundCommands;
            BoundCommand boundCommand = new BoundCommand();

            boundCommand.bindingName = args[0];
            boundCommand.Command = args[4];
            boundCommand.shortKey = true;

            MyKeys key;
            if (!Enum.TryParse(args[1], out key))
                return "Could not parse key!";
            boundCommand.key = key;

            MyKeys modifier;
            if (!Enum.TryParse(args[2], out modifier))
                return "Could not parse modifier!";
            boundCommand.modifier = modifier;

            MyKeys modifier2;
            if (!Enum.TryParse(args[3], out modifier2))
                return "Could not parse modifier2!";
            boundCommand.modifier2 = modifier2;

            commands.Add(boundCommand);
            BindFromBoundCommand(boundCommand);
            Save();
            return "Binding Added!";
        }

        public string RemoveBound(List<string> bindingName)
        {
            if (bindingName.Count < 1)
                return "not enough arguments!";

            List<BoundCommand> commands = configurationWriter.BoundCommands;

            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].bindingName.Equals(bindingName[0]))
                {
                    if (commands[i].shortKey)
                    {
                        HaEPluginCore.HaEInputHandler.RemoveCombination(commands[i].keyCombo);
                        commands.RemoveAtFast(i);
                        return "Removed Binding!";
                    } else
                    {
                        HaEConsole.Instance.UnregisterCommand(commands[i].bindingName);
                        commands.RemoveAtFast(i);
                        return "Removed Alias!";
                    }
                }
            }

            return "Could not find Binding to remove!";
        }

        public void Save()
        {
            if (!Directory.Exists($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}"))
                Directory.CreateDirectory($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}");

            using (var writer = new StreamWriter($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}\\{configurationWriter.fileName}"))
            {
                var x = new XmlSerializer(typeof(WriteConfig));
                x.Serialize(writer, configurationWriter);
                writer.Close();
            }
        }


        public void DeSerialize()
        {
            if (Directory.Exists($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}"))
            {
                try
                {
                    using (var writer = new StreamReader($"{HaEConstants.pluginFolder}\\{HaEConstants.StorageFolder}\\{configurationWriter.fileName}"))
                    {
                        var x = new XmlSerializer(typeof(WriteConfig));
                        configurationWriter = (WriteConfig)x.Deserialize(writer);
                        writer.Close();
                    }
                }
                catch (FileNotFoundException e)
                {
                    //nom
                }
            }

            foreach (var command in configurationWriter.BoundCommands)
            {
                BindFromBoundCommand(command);
            }
        }

        public void BindFromBoundCommand(BoundCommand command)
        {
            if (command.shortKey)
            {
                HaEInputHandler.HaEKeyCombination keyCombination = new HaEInputHandler.HaEKeyCombination(command.key, command.modifier, command.modifier2, HaEConstants.quarterSecTimeOut, command.Execute);
                HaEPluginCore.HaEInputHandler.AddCombination(keyCombination);

                command.keyCombo = keyCombination;
            } else
            {
                HaEConsoleCommand consoleCommand = new HaEConsoleCommand(command.bindingName, x => { command.Execute(); return ""; });
                HaEConsole.Instance.RegisterCommand(consoleCommand);
            }
        }

        [Serializable]
        public class WriteConfig
        {
            [XmlIgnore]
            public string fileName => "HaEConsoleBinder.cfg";

            public List<BoundCommand> BoundCommands = new List<BoundCommand>();

            public WriteConfig()
            {
            }
        }

        [Serializable]
        public class BoundCommand
        {
            public string bindingName;
            public string Command;

            public MyKeys key;
            public MyKeys modifier;
            public MyKeys modifier2;

            public bool shortKey;

            public BoundCommand()
            {
            }

            [XmlIgnore]
            public HaEInputHandler.HaEKeyCombination keyCombo;

            public void Execute()
            {
                HaEConsole.Instance.ParseCommand(Command);
            }
        }

    }
}
