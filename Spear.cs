using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class Spear : ITool
    {
        public string Name => "Lándzsa";
        public int RemainingUses { get; private set; } = 10;

        public int GetBonus(ToolAction action)
        {
            if (action == ToolAction.FoodGathering)
                return 2;

            return 0;
        }

        public bool Use()
        {
            RemainingUses--;
            return RemainingUses <= 0;
        }
        public int GetFishingChanceBonus()
        {
            return 25;
        }
    }

}
