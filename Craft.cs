using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class Craft
    {
        public string Name { get; }
        public int Wood { get; }
        public int Stone { get; }
        public int Leaves { get; }
        public int Rope { get; }
        public string Effect { get; }
        public bool IsStructure { get; }

        public Craft(string name, int wood, int stone, int leaves, int rope,
                      string effect, bool isStructure)
        {
            Name = name;
            Wood = wood;
            Stone = stone;
            Leaves = leaves;
            Rope = rope;
            Effect = effect;
            IsStructure = isStructure;
        }
        public string CostText()
        {
            var parts = new List<string>();
            if (Wood > 0) parts.Add($"{Wood} fa");
            if (Stone > 0) parts.Add($"{Stone} kő");
            if (Leaves > 0) parts.Add($"{Leaves} levél");
            if (Rope > 0) parts.Add($"{Rope} kötél");
            return string.Join(", ", parts);
        }
    }
}

