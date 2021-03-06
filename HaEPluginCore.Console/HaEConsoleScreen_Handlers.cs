﻿using System;
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

        public static void RegisterKeys()
        {
            HaEInputHandler.HaEKeyCombination enter = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.Enter, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, HaEConstants.quarterSecTimeOut, HandleEnter);
            HaEPluginCore.HaEInputHandler.AddCombination(enter);

            HaEInputHandler.HaEKeyCombination back = new HaEInputHandler.HaEKeyCombination(VRage.Input.MyKeys.Up, VRage.Input.MyKeys.None, VRage.Input.MyKeys.None, HaEConstants.quarterSecTimeOut, HandlePrevious);
            HaEPluginCore.HaEInputHandler.AddCombination(back);

        }

        public static void HandlePrevious()
        {
            if (HaEConsole.Instance == null || _instance == null)
                return;

            HaEConsole.Instance.PreviousLine();

            _textBox.Text = HaEConsole.Instance.GetLine();
            _textBox.MoveCarriageToEnd();

            _instance.FocusedControl = _textBox;
        }

        public static void HandleEnter()
        {
            if (_instance == null)
                return;

            if (_instance.FocusedControl == _textBox && !_textBox.Text.Equals(""))
            {
                BufferText = "";
                HaEConsole.Instance.ParseCommand(_textBox.Text);
                HaEConsole.Instance.NextLine();
                _displayScreen.Text = HaEConsole.Instance.displayScreen;
                _displayScreen.ScrollbarOffsetV = 1f;
                _textBox.Text = "";
            }
        }

    }
}
