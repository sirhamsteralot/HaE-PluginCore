using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore
{
    public partial class HaEInputHandler
    {
        private static readonly HaEInputHandler _instance = new HaEInputHandler();
        public static HaEInputHandler Instance => _instance;

        private List<HaEKeyCombination> keyCombinations;

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
