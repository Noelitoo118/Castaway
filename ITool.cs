using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public interface ITool
    {
        string Name { get; }
        int RemainingUses { get; }

        int GetBonus(ToolAction action);

        int GetFishingChanceBonus();

        bool Use();
    }
}
