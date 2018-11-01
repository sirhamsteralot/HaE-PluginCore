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
    public partial class HaEConsoleScreen : MyGuiScreenBase
    {
        private static HaEConsoleScreen _instance;

        private static MyGuiControlMultilineText _displayScreen;
        private static MyGuiControlTextbox _textBox;
        private static MyGuiControlImage _logo;

        private static string BufferText = "";

        private float _screenScale;
        private Vector2 _margin;
        

        public override string GetFriendlyName() {  return "HaE Console";}

        public HaEConsoleScreen() : base(null, null, null, false, null, 0f, 0f)
        {
            this._screenScale = MyGuiManager.GetHudSize().X / MyGuiManager.GetHudSize().Y / HaEConstants.screenScaleConstant;

            this.m_backgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO.Texture;
            this.m_backgroundColor = new Vector4(0, 0, 0, 0.5f);       // Grey, half opacity
            this.m_size = new Vector2(_screenScale, 0.5f);             // Half the screen, drop down from the top
            this.m_position = new Vector2(0.5f, 0.25f);                // HorizontalCenter / Quarter vertical

            _margin = new Vector2(0.06f, 0.04f);                       // Margin

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
            this.m_size = new Vector2(_screenScale, 0.5f);             // Half the screen, drop down from the top
            this.m_position = new Vector2(0.5f, 0.25f);                // HorizontalCenter / Quarter vertical

            base.RecreateControls(constructor);

            Vector4 value = new Vector4(1f, 1f, 1f, 1f);
            float textScale = 1f;

            _textBox = new MyGuiControlTextbox(new Vector2?(new Vector2(0f, 0.25f)), null, 512, new Vector4?(value), 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Default);
            _textBox.Position -= new Vector2(0f, _textBox.Size.Y + _margin.Y / 2f);
            _textBox.Size = new Vector2(_screenScale, _textBox.Size.Y) - 2f * _margin;
            _textBox.ColorMask = new Vector4(0f, 0f, 0f, 0.5f);
            _textBox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
            _textBox.Name = "HaE CMD";

            _displayScreen = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.5f * _screenScale, -0.25f) + _margin), new Vector2?(new Vector2(_screenScale, 0.5f - _textBox.Size.Y) - 2f * _margin), null, "Debug", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, true, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, true, false, null, null);
            _displayScreen.TextColor = Color.White;
            _displayScreen.TextScale = textScale;
            _displayScreen.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
            _displayScreen.Text = HaEConsole.Instance.displayScreen;
            _displayScreen.ColorMask = new Vector4(0f, 0f, 0f, 0.5f);
            _displayScreen.Name = "DisplayScreen";

            _logo = new MyGuiControlImage(new Vector2?(new Vector2(0.46f*_screenScale, -0.24f)), new Vector2?(new Vector2(0.05f, _screenScale * 0.05f)), null, null, new string[]
                {
                    $"{HaEConstants.pluginFolder}\\Assets\\logo.png"
                }, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
            _logo.ColorMask = new Vector4(1f, 1f, 1f, 0.5f);
            _logo.SetToolTip("HaE PluginCore");

            this.Controls.Add(_textBox);
            this.Controls.Add(_displayScreen);
            this.Controls.Add(_logo);
        }


        public static void Close()
        {
            if (_instance != null)
                _instance.CloseScreen();
        }

        public override bool CloseScreen()
        {
            _instance = null;
            return base.CloseScreen();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
        }
    }
}
