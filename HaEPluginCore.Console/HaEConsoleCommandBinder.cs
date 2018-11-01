﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static string StoragePath => Path.GetDirectoryName(typeof(HaEConsoleCommandBinder).Assembly.Location);

        public HaEConsoleCommandBinder()
        {
            configurationWriter = new WriteConfig("HaEConsoleCommandBinderConfig.bin");
            DeSerialize();
            RegisterCommands();
        }

        public void RegisterCommands()
        {
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
                sb.AppendLine($"{command.bindingName}: \"{command.Command}\"");
            }

            return sb.ToString();
        }

        public string AddBinding(List<string> args)
        {
            if (args.Count < 5)
                return "not enough arugmens!";

            List<BoundCommand> commands = configurationWriter.BoundCommands;
            BoundCommand boundCommand = new BoundCommand();

            boundCommand.bindingName = args[0];
            boundCommand.Command = args[4];

            MyKeys key;
            if (!Enum.TryParse(args[1], out key))
                return "Could not parse key!";
            boundCommand._key = key;

            MyKeys modifier;
            if (!Enum.TryParse(args[2], out modifier))
                return "Could not parse modifier!";
            boundCommand._modifier = modifier;

            MyKeys modifier2;
            if (!Enum.TryParse(args[3], out modifier2))
                return "Could not parse modifier2!";
            boundCommand._modifier2 = modifier2;

            commands.Add(boundCommand);
            BindFromBoundCommand(boundCommand);
            Save();
            return "Binding Added!";
        }

        public string RemoveBound(List<string> bindingName)
        {
            List<BoundCommand> commands = configurationWriter.BoundCommands;

            for (int i = 0; i < commands.Count; i++)
            {
                if (commands[i].bindingName.Equals(bindingName[0]))
                {
                    HaEPluginCore.HaEInputHandler.RemoveCombination(commands[i].keyCombo);
                    commands.RemoveAtFast(i);
                    return "Removed Binding!";
                }
            }

            return "Could not find Binding to remove!";
        }

        public void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream($"{StoragePath}\\{configurationWriter.fileName}",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, configurationWriter);
            stream.Close();
        }


        public void DeSerialize()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream($"{StoragePath}\\{configurationWriter.fileName}",
                                            FileMode.Open,
                                            FileAccess.Read, FileShare.Read);
                configurationWriter = (WriteConfig)formatter.Deserialize(stream);
                stream.Close();
            } catch (FileNotFoundException e)
            {
                //nom
            }

            foreach (var command in configurationWriter.BoundCommands)
            {
                BindFromBoundCommand(command);
            }
        }

        public void BindFromBoundCommand(BoundCommand command)
        {
            HaEInputHandler.HaEKeyCombination keyCombination = new HaEInputHandler.HaEKeyCombination(command._key, command._modifier, command._modifier2, HaEConstants.quarterSecTimeOut, command.Execute);
            HaEPluginCore.HaEInputHandler.AddCombination(keyCombination);

            command.keyCombo = keyCombination;
        }

        [Serializable]
        public class WriteConfig
        {
            public string fileName { get; set; }

            public List<BoundCommand> BoundCommands = new List<BoundCommand>();

            public WriteConfig(string fileName)
            {
                this.fileName = fileName;
            }
        }

        [Serializable]
        public class BoundCommand
        {
            public string bindingName;
            public string Command;

            public MyKeys _key;
            public MyKeys _modifier;
            public MyKeys _modifier2;

            [NonSerialized]
            public HaEInputHandler.HaEKeyCombination keyCombo;

            public void Execute()
            {
                HaEConsole.Instance.ParseCommand(Command);
            }
        }

    }
}
