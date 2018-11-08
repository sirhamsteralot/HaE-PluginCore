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
            lock (keyCombinations)
            {
                keyCombinations.Add(keyCombination);
            }
        }

        public void RemoveCombination(HaEKeyCombination keyCombination)
        {
            lock (keyCombinations)
            {
                for (int i = 0; i < keyCombinations.Count; i++)
                {
                    if (keyCombinations[i].Equals(keyCombination))
                    {
                        keyCombinations.RemoveAtFast(i);
                        return;
                    }
                }
            }
        }

        public HaEInputHandler()
        {
            HaEPluginCore.OnUpdate += Update;
        }

        private void Update()
        {
            lock (keyCombinations)
            {
                for (int i = 0; i < keyCombinations.Count; i++)
                {
                    if (i < keyCombinations.Count)
                        keyCombinations[i].CheckCombinationPressed();
                }
            }
        }
    }
}
