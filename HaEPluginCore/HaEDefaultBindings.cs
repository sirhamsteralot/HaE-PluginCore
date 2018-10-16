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
        public void BindKeys()
        {
            HaEInputHandler.HaEKeyCombination showCrossHair = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.C, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, () => {

                typeof(MyHudCrosshair).GetProperty("Visible").SetValue(MyHud.Crosshair, !MyHud.Crosshair.Visible);
            });
            HaEPluginCore.HaEInputHandler.AddCombination(showCrossHair);
            
        }
    }
}
