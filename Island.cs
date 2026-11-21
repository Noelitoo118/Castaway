using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class Island
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public int Food { get; private set; }
        public int Water { get; private set; }
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Leaf { get; private set; }
        public int Rope { get; private set; }

        public int GatherBonus { get; private set; }
        public int WaterBonus { get; private set; }
        public int MaterialBonus { get; private set; }
        public int RestBonus { get; private set; }

        public Island(
            string name,
            string description,
            int food, int water,
            int wood, int stone, int leaf, int rope,
            int gatherBonus, int waterBonus,
            int materialBonus, int restBonus)
        {
            Name = name;
            Description = description;
            Food = food;
            Water = water;
            Wood = wood;
            Stone = stone;
            Leaf = leaf;
            Rope = rope;
            GatherBonus = gatherBonus;
            WaterBonus = waterBonus;
            MaterialBonus = materialBonus;
            RestBonus = restBonus;
        }
    }
}
