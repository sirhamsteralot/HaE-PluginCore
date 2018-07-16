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

            private readonly Action _callback;


            public void CheckCombinationPressed()
            {
                if (MyInput.Static.IsKeyPress(_key) && MyInput.Static.IsKeyPress(_modifier))
                    _callback?.Invoke();
            }
        }
    }
}
