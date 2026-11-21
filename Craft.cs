using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class Craft
    {
        public string Name { get; private set; }
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Leaf { get; private set; }
        public int Rope { get; private set; }
        public string Effect { get; private set; }
        public bool IsBuilding { get; private set; }

        public Craft(string name, int wood, int stone,
            int leaf, int rope, string effect, bool isBuilding)
        {
            Name = name;
            Wood = wood;
            Stone = stone;
            Leaf = leaf;
            Rope = rope;
            Effect = effect;
            IsBuilding = isBuilding;
        }
    
        public bool IsRopeCraftable(int leafamount)
        {
            if (Leaf < leafamount || Leaf == leafamount)
            {
                return true;
            }
            return false;
        }
        public bool IsFirePlaceCraftable(int woodamount, int stoneamount)
        {
            if (Wood < woodamount || Wood == woodamount)
            {
                if (Stone < stoneamount || Stone == stoneamount)
                {
                    return true;
                }
            }
            return false;

        }
        public bool IsRefugeCraftable(int woodamount, int leafamount, int ropeamount)
        {
            if (Wood < woodamount || Wood == woodamount)
            {
                if (Leaf < leafamount || Leaf == leafamount)
                {
                    if (Rope < ropeamount || Rope == ropeamount)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsSignalFireCraftable(int woodamount, int stoneamount, int ropeamount)
        {
            if (Wood < woodamount || Wood == woodamount)
            {
                if (Stone < stoneamount || Stone == stoneamount)
                {
                    if (Rope < ropeamount || Rope == ropeamount)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    } 
}

