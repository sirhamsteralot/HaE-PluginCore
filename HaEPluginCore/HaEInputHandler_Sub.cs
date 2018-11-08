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
        public class HaEKeyCombination : IEquatable<HaEKeyCombination>
        {
            private readonly MyKeys _key;
            private readonly MyKeys _modifier;
            private readonly MyKeys _modifier2;

            private readonly TimeSpan _timeout;

            private readonly Action _callback;

            private DateTime _lastPressed;
            private bool _active = true;

            public HaEKeyCombination(MyKeys key, MyKeys modifier, MyKeys modifier2, TimeSpan timeOut, Action callback)
            {
                _key = key;
                _modifier = modifier;
                _modifier2 = modifier2;
                _timeout = timeOut;

                _callback = callback;
            }

            public HaEKeyCombination(MyKeys key, MyKeys modifier, MyKeys modifier2, Action callback)
            {
                _key = key;
                _modifier = modifier;
                _modifier2 = modifier2;
                _timeout = TimeSpan.Zero;

                _callback = callback;
            }

            public void CheckCombinationPressed()
            {
                if (!_active)
                    return;

                if ((MyInput.Static.IsKeyPress(_key) && 
                   (MyInput.Static.IsKeyPress(_modifier) || _modifier == MyKeys.None) && 
                   (MyInput.Static.IsKeyPress(_modifier2) || _modifier2 == MyKeys.None)) &&
                   ((DateTime.Now - _lastPressed) > _timeout))
                {
                    _lastPressed = DateTime.Now;

                    _callback?.Invoke();
                }
            }

            public void SetActive(bool val)
            {
                _active = val;
            }

            public bool Equals(HaEKeyCombination other)
            {
                return ((other._key == _key) && (other._modifier == _modifier) && (other._modifier2 == _modifier2)) && (other._callback == _callback);
            }
        }
    }
}
