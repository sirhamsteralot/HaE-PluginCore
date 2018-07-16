using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Input;

namespace HaEPluginCore
{
    public partial class HaEInputHandler
    {
        public class HaEKeyCombination
        {
            private readonly MyKeys _key;
            private readonly MyKeys _modifier;
            private readonly MyKeys _modifier2;

            private readonly Action _callback;

            public HaEKeyCombination(MyKeys key, MyKeys modifier, MyKeys modifier2, Action callback)
            {
                _key = key;
                _modifier = modifier;
                _modifier2 = modifier2;

                _callback = callback;
            }

            public void CheckCombinationPressed()
            {
                if (MyInput.Static.IsKeyPress(_key) && 
                   (MyInput.Static.IsKeyPress(_modifier) || _modifier == MyKeys.None) && 
                   (MyInput.Static.IsKeyPress(_modifier2) || _modifier2 == MyKeys.None))
                {
                    _callback?.Invoke();
                }
            }
        }
    }
}
