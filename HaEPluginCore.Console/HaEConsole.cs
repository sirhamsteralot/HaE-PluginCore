using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore.Console
{
    public class HaEConsole
    {
        private HaEInputHandler.HaEKeyCombination keyCombination = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.C, VRage.Input.MyKeys.RightAlt, VRage.Input.MyKeys.None, HaEConsoleScreen.Show);
    }
}
