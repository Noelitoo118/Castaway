using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class Survivor
    {
        private string _name;
        private int _food;
        private int _water;
        private int _energy;
        private int _wood;
        private int _stone;
        private int _leaf;
        private int _rope;

        public string Name {get; private set;}
        public int Food { get; private set; }
        public int Water { get; private set; }
        public int Energy { get; private set; }
        public int Wood { get; private set; }
        public int Stone { get; private set; }
        public int Leaf { get; private set; }
        public int Rope { get; private set; }

        public Survivor(string name)
        {
            _name = name;
            _food = 8;
            _water = 8;
            _energy = 10;
            _wood = 0;
            _stone = 0;
            _leaf = 0;
            _rope = 0;
        }
        private int LimitStatisztika(int value)
        {
            if (value < 0)
            {
                return 0;
            }
            if (value > 10)
            {
                return 10;
            }
            return value;
        }

        private int LimitAlapAnyag(int value)
        {
            if (value < 0)
            {
                return 0;
            }
            return value;
        }
        public void AddFood(int amount)
        {
            _food = LimitStatisztika(_food + amount);
        }

        public void AddWater(int amount)
        {
            _water = LimitStatisztika(_water + amount);
        }

        public void AddEnergy(int amount)
        {
            _energy = LimitStatisztika(_energy + amount);
        }

        public void AddWood(int amount)
        {
            _wood = LimitAlapAnyag(_wood + amount);
        }

        public void AddStone(int amount)
        {
            _stone = LimitAlapAnyag(_stone + amount);
        }

        public void AddLeaf(int amount)
        {
            _leaf = LimitAlapAnyag(_leaf + amount);
        }

        public void AddRope(int amount)
        {
            _rope = LimitAlapAnyag(_rope + amount);
        }
    }
}
