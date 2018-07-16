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
        

        public override string GetFriendlyName() {  return "HaE Console";}

        public HaEConsoleScreen() : base(null, null, null, false, null, 0f, 0f)
        {
            this._screenscale = MyGuiManager.GetHudSize().X / MyGuiManager.GetHudSize().Y / HaEConstants.screenScaleConstant;

            this.m_backgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO.Texture;
            this.m_backgroundColor = new Vector4(0, 0, 0, 0.5f);       // Grey, half opacity
            this.m_size = new Vector2(_screenscale, 0.5f);             // Half the screen, drop down from the top
            this.m_position = new Vector2(0.5f, 0.25f);                // HorizontalCenter / Quarter vertical


            RecreateControls(true);
        }

        public static void Show()
        {
            _instance = new HaEConsoleScreen();
            _instance.RecreateControls(true);
            MyGuiSandbox.AddScreen(_instance);
        }

        public sealed override void RecreateControls(bool constructor)
        {
            Elements.Clear();

            this.m_backgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO.Texture;
            this.m_backgroundColor = new Vector4(0, 0, 0, 0.5f);       // Grey, half opacity
            this.m_size = new Vector2(_screenscale, 0.5f);             // Half the screen, drop down from the top
            this.m_position = new Vector2(0.5f, 0.25f);                // HorizontalCenter / Quarter vertical
        }
    }
}
