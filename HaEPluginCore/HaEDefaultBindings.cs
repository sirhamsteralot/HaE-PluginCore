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
            HaEInputHandler.HaEKeyCombination showCrossHair = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.OemPlus, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, HaEConstants.quarterSecTimeOut, () => {

                if ((int)MySandboxGame.Config.ShowCrosshair + 1 < 4)
                {
                    MySandboxGame.Config.ShowCrosshair++;
                } else
                {
                    MySandboxGame.Config.ShowCrosshair = 0;
                }
            });
            HaEPluginCore.HaEInputHandler.AddCombination(showCrossHair);
        }
    }
}
