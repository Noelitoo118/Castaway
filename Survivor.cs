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


        public string Name { get { return _name; } }
        public int Food { get { return _food; } }
        public int Water { get { return _water; } }
        public int Energy { get { return _energy; } }
        public int Wood { get { return _wood; } }
        public int Stone { get { return _stone; } }
        public int Leaf { get { return _leaf; } }
        public int Rope { get { return _rope; } }
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
        }public Survivor(string name, int food, int water, int energy, int wood, int stone, int leaf, int rope)
        {
            _name = name;
            _food = LimitStatisztika(food);
            _water = LimitStatisztika(water);
            _energy = LimitStatisztika(energy);
            _wood = LimitAlapAnyag(wood);
            _stone = LimitAlapAnyag(stone);
            _leaf = LimitAlapAnyag(leaf);
            _rope = LimitAlapAnyag(rope);
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
        public void DeleteEnergy(int amount)
        {
            _energy = LimitStatisztika(_energy - amount);
        }
        public void DeleteLeaf(int amount)
        {
            _leaf = LimitAlapAnyag(_leaf - amount);
        }
        public void DeleteRope(int amount)
        {
            _rope = LimitAlapAnyag(_rope - amount);
        }
        public void DeleteWood(int amount)
        {
            _wood = LimitAlapAnyag(_wood - amount);
        }
        public void DeleteStone(int amount)
        {
            _stone = LimitAlapAnyag(_stone - amount);
        }
        public void DeleteFood(int amount)
        {
            _food = LimitStatisztika(_food - amount);
        }
        public void DeleteWater(int amount)
        {
            _water = LimitStatisztika(_water - amount);
        }
        public void LoseHalfMaterials()
        {
            _wood /= 2;
            _stone /= 2;
            _leaf /= 2;
            _rope /= 2;
        }
    }
}
