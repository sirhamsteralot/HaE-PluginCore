using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaEPluginCore;
using VRage.Input;

namespace HaEPluginCore.Console
{
    public class HaEConsoleCommandBinder
    {
        public static WriteConfig configurationWriter = new WriteConfig(null, "HaEConsoleCommandBinderConfig.bin");

        public HaEConsoleCommandBinder()
        {
            DeSerialize();
            RegisterCommands();
        }

        public void RegisterCommands()
        {
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("AddBinding", "Binds Console Command to key, Usage: AddBinding {name} {key} {modifier} {modifier2} \"{command}\"", AddBinding));
            HaEConsole.Instance.RegisterCommand(new HaEConsoleCommand("RemoveBinding", "Unbinds Console Command from key", RemoveBound));
        }

        public string AddBinding(List<string> args)
        {
            if (args.Count < 5)
                return "not enough arugmens!";

            List<BoundCommand> commands = ((Configuration)configurationWriter.Configuration).BoundCommands;
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
            return "Binding Added!";
        }

        public string RemoveBound(List<string> bindingName)
        {
            List<BoundCommand> commands = ((Configuration)configurationWriter.Configuration).BoundCommands;

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
            HaEPluginConfigurationHandler.Serialize(configurationWriter);
        }


        public class WriteConfig : HaEPluginConfigurationHandler.IHaESerializable
        {
            public string fileName { get; set; }

            public object Configuration { get; set; }

            public WriteConfig(object Configuration, string fileName)
            {
                this.fileName = fileName;
                this.Configuration = Configuration;
            }
        }

        public void DeSerialize()
        {
            HaEPluginConfigurationHandler.AddSerializable(configurationWriter);
            HaEPluginConfigurationHandler.DeSerialize(configurationWriter);

            foreach (var command in ((Configuration)configurationWriter.Configuration).BoundCommands)
            {
                BindFromBoundCommand(command);
            }
        }

        public void BindFromBoundCommand(BoundCommand command)
        {
            HaEInputHandler.HaEKeyCombination keyCombination = new HaEInputHandler.HaEKeyCombination(command._key, command._modifier, command._modifier2, command.Execute);
            HaEPluginCore.HaEInputHandler.AddCombination(keyCombination);

            command.keyCombo = keyCombination;
        }

        [Serializable]
        public class Configuration
        {
            public List<BoundCommand> BoundCommands = new List<BoundCommand>();
        }

        [Serializable]
        public class BoundCommand
        {
            public string bindingName;
            public string Command;

            public MyKeys _key;
            public MyKeys _modifier;
            public MyKeys _modifier2;

            public HaEInputHandler.HaEKeyCombination keyCombo;

            public void Execute()
            {
                HaEConsole.Instance.HandleCommand(Command);
            }
        }

    }
}
