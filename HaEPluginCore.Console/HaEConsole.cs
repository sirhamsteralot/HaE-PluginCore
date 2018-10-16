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


        public void Init(object gameInstance)
        {
            _instance = this;

            HaEInputHandler.HaEKeyCombination enter = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.C, VRage.Input.MyKeys.RightAlt, VRage.Input.MyKeys.None, HaEConsoleScreen.Show);
            HaEInputHandler.Instance.AddCombination(enter);

            HaEInputHandler.HaEKeyCombination exit = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.Escape, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, HaEConsoleScreen.Close);
            HaEInputHandler.Instance.AddCombination(exit);
        }

        public void Update()
        {

        }

        public void Dispose()
        {

        }
    }
}
