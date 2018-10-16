using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using Sandbox.Game;
using Sandbox.Engine.Utils;
using Sandbox;

namespace HaEPluginCore
{
    public class HaEDefaultBindings
    {
        public static void BindKeys()
        {
            TimeSpan quarterSecTimeOut = TimeSpan.FromMilliseconds(250);

            HaEInputHandler.HaEKeyCombination showCrossHair = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.C, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, quarterSecTimeOut, () => {

                MySandboxGame.Config.ShowCrosshair = !MySandboxGame.Config.ShowCrosshair;
            });
            HaEPluginCore.HaEInputHandler.AddCombination(showCrossHair);
            
        }
    }
}
