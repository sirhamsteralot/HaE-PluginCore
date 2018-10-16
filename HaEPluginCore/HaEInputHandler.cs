using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore
{
    public partial class HaEInputHandler
    {
        private List<HaEKeyCombination> keyCombinations = new List<HaEKeyCombination>();

        public void AddCombination(HaEKeyCombination keyCombination)
        {
            keyCombinations.Add(keyCombination);
        }

        public HaEInputHandler()
        {
            HaEPluginCore.OnUpdate += Update;
        }

        private void Update()
        {
            foreach (HaEKeyCombination keyCombination in keyCombinations)
                keyCombination.CheckCombinationPressed();
        }
    }
}
