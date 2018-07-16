using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage.Utils;
using VRageMath;
using HaEPluginCore;

namespace HaEPluginCore.Console
{
    public class HaEConsoleScreen : MyGuiScreenBase
    {
        private static HaEConsoleScreen _instance;

        private MyGuiControlTextbox _textBox;

        private float _screenscale;
        private Vector2 _margin;
        

        public override string GetFriendlyName() {  return "HaE Console";}

        public HaEConsoleScreen() : base(isTopMostScreen: true)
        {
            this._screenscale = MyGuiManager.GetHudSize().X / MyGuiManager.GetHudSize().Y / HaEConstants.screenScaleConstant;

            BackgroundColor = new Vector4(0, 0, 0, 0.5f);
            Size = new Vector2(_screenscale, 0.5f);
            RecreateControls(true);
        }

        public sealed override void RecreateControls(bool constructor)
        {
            Elements.Clear();

        }
    }
}
