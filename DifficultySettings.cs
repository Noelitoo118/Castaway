using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castaway
{
    public class DifficultySettings
    {
        public int StartingFood { get; private set; }
        public int StartingWater { get; private set; }
        public int NightEventChance { get; private set; }
        public int ShipInterval { get; private set; }

        public DifficultySettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    StartingFood = 10;
                    StartingWater = 10;
                    NightEventChance = 15;
                    ShipInterval = 4;
                    break;

                case Difficulty.Normal:
                    StartingFood = 8;
                    StartingWater = 8;
                    NightEventChance = 25;
                    ShipInterval = 5;
                    break;

                case Difficulty.Hard:
                    StartingFood = 6;
                    StartingWater = 6;
                    NightEventChance = 40;
                    ShipInterval = 6;
                    break;
            }
        }
    }
}
