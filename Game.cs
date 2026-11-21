using Castaway;
using System;
using System.Text;

public class Game
{
    private List<Island> _allLocations = new List<Island>
{
    new Island(
        "Forrás", 0,
        0, 4, 0, 0, 0, 0,
        0, 2, 0, 0),

    new Island(
        "Banánliget", 1,
        4, 0, 0, 0, 0, 0,
        2, 0, 0, 0),

    new Island(
        "Kőfejtő",2,
        0, 0, 0, 4, 0, 0,
        0, 0, 1, 0),

    new Island(
        "Elhagyott tábor", 3,
        0, 0, 2, 0, 0, 2,
        0, 0, 1, 0),

    new Island(
        "Barlang", 4,
        0, 0, 0, 2, 3, 0,
        0, 0, 0, 2)
};

    private List<Island> _discoveredLocations = new List<Island>();

    private int _gatherLevel = 0;
    private int _gatherUses = 0;

    private int _waterLevel = 0;
    private int _waterUses = 0;

    private int _materialLevel = 0;
    private int _materialUses = 0;

    private const int NightFoodCost = 2;
    private const int NightWaterCost = 2;
    private const int NightEnergyGain = 3;

    private bool _gameEnded = false;
    private string _endReason = "";
    private int _warningShownDay = 0;

    private const int MaxStat = 10;
    private const int ActionCost = 2;
    private const int RestGain = 5;

    private readonly Random _random = new Random();
    private Survivor _survivor;
    private int _day = 1;
    private int _timeOfDay = 0;
    private string[] _timeNames = { "délelőtt", "délután", "este" };
    private List<string> _buildings = new List<string>();

    private List<Craft> _recipes = new List<Craft>
{
    new Craft("Kötél", 0, 0, 3, 0,
        "Alapanyag, többször is elkészíthető.", false),

    new Craft("Tűzrakó hely", 3, 2, 0, 0,
        "A vízkeresés +2 vizet ad.", true),

    new Craft("Menedék", 6, 0, 4, 1,
        "A pihenés 5 helyett 7 energiát ad.", true),

    new Craft("Jelzőtűz", 10, 5, 0, 2,
        "Egy arra járó hajó észrevehet.", true)
};
    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8; // hogy az ékezetes betűk jól jelenjenek meg
        Console.WriteLine(Texts.Title);
        Console.WriteLine();
        Console.WriteLine(Texts.Intro);
        Console.WriteLine();

        string name = AskName();
        if (name == null)
        {
            Console.WriteLine("Nincs több bemenet, a játék véget ér.");
            return;
        }
        _survivor = new Survivor(name);

        Console.WriteLine($"Üdv a szigeten, {_survivor.Name}!");
        Console.WriteLine();
        bool running = true;
        while (running && !_gameEnded)
        {
            CheckShipWarning();

            ShowStatus();
            ShowMenu();

            int choice = ReadNumber(1, 9);

            Console.WriteLine();

            if (_timeOfDay == 2 &&
            (choice == 1 || choice == 2 || choice == 3 || choice == 5) || choice == 9)
            {
                Console.WriteLine("Már sötét van! Este csak pihenni vagy barkácsolni lehet.");
            }
            else if (choice >= 1 && choice <= 4)
            {
                DoAction(choice);
                AdvanceTime();
            }
            else if (choice == 5)
            {
                Fishing();
                AdvanceTime();
            }
            else if (choice == 6)
            {
                if (Craft())
                {
                    AdvanceTime();
                }
            }
            else if (choice == 7)
            {
                ShowInventory();
            }
            else if (choice == 8)
            {
                if (Explore())
                {
                    AdvanceTime();
                }
            }
            else if (choice == 9)
            {
                running = false;
                _endReason = "Kilépés";
            }

            Console.WriteLine();
        }
        ShowSummary();

        Console.WriteLine();
        Console.WriteLine("Viszlát, " + _survivor.Name + "!");
    }
    private void DoAction(int action)
    {
        if (action == 4)
        {
            int restGain = RestGain;

            if (_buildings.Contains("Menedék"))
            {
                restGain += 2;
            }

            restGain += GetRestBonus();

            _survivor.AddEnergy(restGain);

            Console.WriteLine("Pihentél. +" + restGain + " energia.");
            return;
        }

        if (_survivor.Energy < ActionCost)
        {
            Console.WriteLine("Nincs elég energiád ehhez az akcióhoz!");
            return;
        }

        _survivor.DeleteEnergy(ActionCost);

        if (action == 1)
        {
            int amount = _random.Next(2, 5) + _gatherLevel + GetGatherBonus();
            _survivor.AddFood(amount);

            Console.WriteLine("Élelmet gyűjtöttél. +" + amount + " élelem.");

            IncreaseSkill("Élelemgyűjtés", ref _gatherLevel, ref _gatherUses);
        }
        else if (action == 2)
        {
            int amount = _random.Next(2, 5) + _waterLevel + GetWaterBonus();

            if (_buildings.Contains("Tűzrakó hely"))
            {
                amount += 2;
            }

            _survivor.AddWater(amount);

            Console.WriteLine("Vizet gyűjtöttél. +" + amount + " víz.");

            IncreaseSkill("Vízkeresés", ref _waterLevel, ref _waterUses);
        }
        else if (action == 3)
        {
            int wood = _random.Next(1, 4) + _materialLevel + GetMaterialBonus();
            int stone = _random.Next(0, 3) + _materialLevel + GetMaterialBonus();
            int leaf = _random.Next(1, 4) + _materialLevel + GetMaterialBonus();

            _survivor.AddWood(wood);
            _survivor.AddStone(stone);
            _survivor.AddLeaf(leaf);

            Console.WriteLine("Anyagokat gyűjtöttél:");
            Console.WriteLine("Fa: +" + wood);
            Console.WriteLine("Kő: +" + stone);
            Console.WriteLine("Levél: +" + leaf);

            IncreaseSkill("Anyaggyűjtés", ref _materialLevel, ref _materialUses);
        }
    }

    private void AdvanceTime()
    {
        _timeOfDay++;

        if (_timeOfDay > 2)
        {
            _timeOfDay = 0;

            Console.WriteLine();
            Console.WriteLine("--- Leszállt az éj (" + _day + ". nap) ---");

            if (_day % 5 == 0)
            {
                if (CheckShip())
                {
                    return;
                }
            }

            Night();

            if (!_gameEnded)
            {
                _day++;
            }
        }
    }
    private void Night()
    {
        _survivor.DeleteFood(NightFoodCost);
        _survivor.DeleteWater(NightWaterCost);

        Console.WriteLine("Éjszakai fogyasztás:");
        Console.WriteLine("-2 élelem");
        Console.WriteLine("-2 víz");

        NightEvent nightEvent;

        if (_random.Next(100) < 25)
        {
            NightEvent[] events =
            {
            NightEvent.StrongWind,
            NightEvent.WildAnimal
        };

            nightEvent = events[_random.Next(events.Length)];
        }
        else
        {
            nightEvent = NightEvent.QuietNight;
        }

        switch (nightEvent)
        {
            case NightEvent.QuietNight:
                Console.WriteLine(RandomText(Texts.QuietNights));
                break;

            case NightEvent.StrongWind:
                Console.WriteLine(RandomText(Texts.WindTexts));
                _survivor.DeleteWood(2);
                Console.WriteLine("2 fa elveszett.");
                break;

            case NightEvent.WildAnimal:
                Console.WriteLine(RandomText(Texts.AnimalTexts));
                _survivor.DeleteFood(2);
                Console.WriteLine("2 élelem elveszett.");
                break;
        }

        if (_survivor.Food == 0 || _survivor.Water == 0)
        {
            GameOver();
            return;
        }

        _survivor.AddEnergy(NightEnergyGain);
        if (!_buildings.Contains("Menedék") && _random.Next(100) < 30)
        {
            _survivor.DeleteEnergy(3);
            Console.WriteLine("Rosszul aludtál a szabad ég alatt! -3 energiát vesztettél.");
        }
        Console.WriteLine("Átaludtad az éjszakát. +3 energia.");
    }

    private string RandomText(string[] texts)
    {
        return texts[_random.Next(texts.Length)];
    }

    private void ShowStatus()
    {
        Console.WriteLine(_survivor.Name + " | " + _day + ". nap, " + _timeNames[_timeOfDay]
            + " | Élelem " + _survivor.Food + "/10"
            + " | Víz " + _survivor.Water + "/10"
            + " | Energia " + _survivor.Energy + "/10");
        Console.WriteLine("Fa " + _survivor.Wood
            + " | Kő " + _survivor.Stone
            + " | Levél " + _survivor.Leaf
            + " | Kötél " + _survivor.Rope);
        Console.WriteLine();
    }
    private bool CanCraft(Craft recipe)
    {
        if (_survivor.Wood < recipe.Wood)
            return false;

        if (_survivor.Stone < recipe.Stone)
            return false;

        if (_survivor.Leaf < recipe.Leaf)
            return false;

        if (_survivor.Rope < recipe.Rope)
            return false;

        if (recipe.IsBuilding && _buildings.Contains(recipe.Name))
            return false;

        return true;
    }
    private void UseMaterials(Craft recipe)
    {
        _survivor.DeleteWood(recipe.Wood);
        _survivor.DeleteStone(recipe.Stone);
        _survivor.DeleteLeaf(recipe.Leaf);
        _survivor.DeleteRope(recipe.Rope);
    }
    public void ShowCrafts()
    {
        Console.WriteLine("--- Barkácsolás ---");

        Console.WriteLine("[0] Visszalépés");

        for (int i = 0; i < _recipes.Count; i++)
        {
            Craft recipe = _recipes[i];

            Console.WriteLine(
                "[" + (i + 1) + "] " + recipe.Name
                + " | Fa: " + recipe.Wood
                + " | Kő: " + recipe.Stone
                + " | Levél: " + recipe.Leaf
                + " | Kötél: " + recipe.Rope);

            Console.WriteLine("Hatás: " + recipe.Effect);

            if (CanCraft(recipe))
            {
                Console.WriteLine("Elkészíthető: Igen");
            }
            else
            {
                Console.WriteLine("Elkészíthető: Nem");
            }

            Console.WriteLine();
        }
    }
    private void ShowMenu()
    {
        Console.WriteLine("[1] Gyűjtögetés");
        Console.WriteLine("[2] Vízkeresés");
        Console.WriteLine("[3] Anyaggyűjtés");
        Console.WriteLine("[4] Pihenés");
        Console.WriteLine("[5] Barkácsolás");
        Console.WriteLine("[6] Halászat");
        Console.WriteLine("[7] Leltár");
        Console.WriteLine("[8] Felfedezés");
        Console.WriteLine("[9] Kilépés");
    }
    private void ShowInventory()
    {
        Console.WriteLine("--- Leltár ---");
        Console.WriteLine($"Név: {_survivor.Name}");
        Console.WriteLine($"Élelem: {_survivor.Food}/10");
        Console.WriteLine($"Víz: {_survivor.Water}/10");
        Console.WriteLine($"Energia: {_survivor.Energy}/10");
        Console.WriteLine($"Fa: {_survivor.Wood}");
        Console.WriteLine($"Kő: {_survivor.Stone}");
        Console.WriteLine($"Levél: {_survivor.Leaf}");
        Console.WriteLine($"Kötél: {_survivor.Rope}");
        Console.WriteLine();
        Console.WriteLine("--- Gyakorlati szintek ---");
        Console.WriteLine("Élelemgyűjtés: " + _gatherLevel +". szint | Használatok: " + _gatherUses );
        Console.WriteLine( "Vízkeresés: " + _waterLevel +". szint | Használatok: " + _waterUses);
        Console.WriteLine("Anyaggyűjtés: " + _materialLevel +". szint | Használatok: " + _materialUses);
        if (_buildings.Count == 0)
        {
            Console.WriteLine("Építmények: még nincs egy sem.");
        }
        else
        {
            Console.WriteLine("Építmények:");
            foreach (string building in _buildings)
            {
                Console.WriteLine("- " + building);
            }
        }
        Console.WriteLine();
        Console.WriteLine("--- Felfedezett helyszínek ---");

        if (_discoveredLocations.Count == 0)
        {
            Console.WriteLine("Még nem fedeztél fel helyszínt.");
        }
        else
        {
            foreach (Island location in _discoveredLocations)
            {
                Console.WriteLine(Texts.LocationTexts[location.TextIndex]);

                if (location.GatherBonus > 0)
                    Console.WriteLine("  Gyűjtögetés bónusz: +" + location.GatherBonus);

                if (location.WaterBonus > 0)
                    Console.WriteLine("  Vízkeresés bónusz: +" + location.WaterBonus);

                if (location.MaterialBonus > 0)
                    Console.WriteLine("  Anyaggyűjtés bónusz: +" + location.MaterialBonus);

                if (location.RestBonus > 0)
                    Console.WriteLine("  Pihenés bónusz: +" + location.RestBonus);
            }
        }
    }
    private int ReadNumber(int min, int max)
    {
        while (true)
        {
            Console.Write($"Választás ({min}-{max}): ");
            string input = Console.ReadLine();

            if (input == null)
            {
                return max;
            }

            int number;
            if (int.TryParse(input, out number) && number >= min && number <= max)
            {
                return number;
            }

            Console.WriteLine($"Hibás érték! Egy {min} és {max} közötti számot adj meg.");
        }
    }

    private string AskName()
    {
        Console.Write("Mi a neved? ");
        string input = Console.ReadLine();

        if (input == null)
        {
            return null;
        }

        if (input == "")
        {
            Console.WriteLine("A név nem lehet üres, add meg újra!");
            return AskName();
        }

        return input;
    }
    private bool Craft()
    {
        ShowCrafts();

        int choice = ReadNumber(0, _recipes.Count);

        if (choice == 0)
        {
            return false;
        }

        Craft recipe = _recipes[choice - 1];

        if (!CanCraft(recipe))
        {
            Console.WriteLine("Nincs elegendő alapanyagod, vagy ez az építmény már elkészült.");
            return false;
        }

        if (_survivor.Energy < ActionCost)
        {
            Console.WriteLine("Túl fáradt vagy! Legalább 2 energia szükséges.");
            return false;
        }

        UseMaterials(recipe);

        _survivor.DeleteEnergy(ActionCost);

        if (recipe.Name == "Kötél")
        {
            _survivor.AddRope(1);
        }
        else
        {
            _buildings.Add(recipe.Name);
        }

        Console.WriteLine("Sikeresen elkészítetted: " + recipe.Name + "!");
        Console.WriteLine(recipe.Effect);

        return true;
    }
    private bool CheckShip()
    {
        Console.WriteLine();
        Console.WriteLine("Egy hajó halad el a sziget mellett!");

        if (_buildings.Contains("Jelzőtűz"))
        {
            Console.WriteLine("A hajó észrevette a jelzőtüzedet!");
            Console.WriteLine("MEGMENEKÜLTÉL!");

            _gameEnded = true;
            _endReason = "Megmenekülés";

            return true;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine(RandomText(Texts.ShipMissed));

            return false;
        }
    }
    private void GameOver()
    {
        Console.WriteLine();
        Console.WriteLine("========== GAME OVER ==========");

        if (_survivor.Food == 0)
        {
            Console.WriteLine("Elfogyott az élelmed!");
        }

        if (_survivor.Water == 0)
        {
            Console.WriteLine("Elfogyott a vized!");
        }

        Console.WriteLine("Nem sikerült túlélned az éjszakát.");

        _gameEnded = true;
        _endReason = "Game Over";
    }
    private void CheckShipWarning()
    {
        if (_day % 5 == 0 && _warningShownDay != _day)
        {
            Console.WriteLine();
            Console.WriteLine("FIGYELEM! Ma este egy hajó haladhat el a sziget mellett!");
            Console.WriteLine("Ha van jelzőtüzed, észrevehetnek!");

            _warningShownDay = _day;
        }
    }
    private void ShowSummary()
    {
        Console.WriteLine();
        Console.WriteLine("========== JÁTÉK ÖSSZEGZÉSE ==========");

        Console.WriteLine("Játékos: " + _survivor.Name);
        Console.WriteLine("A játék vége: " + _endReason);
        Console.WriteLine("A szigeten töltött napok: " + _day);

        Console.WriteLine();
        Console.WriteLine("Megépített építmények:");

        if (_buildings.Count == 0)
        {
            Console.WriteLine("Nem építettél semmit.");
        }
        else
        {
            foreach (string building in _buildings)
            {
                Console.WriteLine("- " + building);
            }
        }

        Console.WriteLine("======================================");
    }
    private void Fishing()
    {
        if (_survivor.Energy < ActionCost)
        {
            Console.WriteLine("Nincs elég energiád a halászathoz!");
            return;
        }

        _survivor.DeleteEnergy(ActionCost);

        int chance = _buildings.Contains("Lándzsa") ? 75 : 50;

        if (_random.Next(100) < chance)
        {
            _survivor.AddFood(5);
            Console.WriteLine(RandomText(Texts.FishingSuccess));
            Console.WriteLine("+5 élelem!");
        }
        else
        {
            Console.WriteLine(RandomText(Texts.FishingFail));
        }
    }
    private void IncreaseSkill(string skillName, ref int level, ref int uses)
    {
        uses++;

        if (uses % 5 == 0 && level < 3)
        {
            level++;
            Console.WriteLine(
                "Egyre ügyesebb vagy! " + skillName + ": " + level + ". szint."
            );
        }
    }
    private bool Explore()
    {
        if (_discoveredLocations.Count == _allLocations.Count)
        {
            Console.WriteLine("Már minden helyszínt felfedeztél!");
            return false;
        }

        if (_survivor.Energy < ActionCost)
        {
            Console.WriteLine("Nincs elég energiád a felfedezéshez!");
            return false;
        }

        List<Island> undiscovered =
            new List<Island>();

        foreach (Island location in _allLocations)
        {
            if (!_discoveredLocations.Contains(location))
            {
                undiscovered.Add(location);
            }
        }

        Island found =
            undiscovered[_random.Next(undiscovered.Count)];

        _survivor.DeleteEnergy(ActionCost);

        _discoveredLocations.Add(found);

        Console.WriteLine();
        Console.WriteLine("Új helyszínt fedeztél fel!");
        Console.WriteLine(Texts.LocationTexts[found.TextIndex]);

        _survivor.AddFood(found.Food);
        _survivor.AddWater(found.Water);
        _survivor.AddWood(found.Wood);
        _survivor.AddStone(found.Stone);
        _survivor.AddLeaf(found.Leaf);
        _survivor.AddRope(found.Rope);

        Console.WriteLine("Megtalált jutalmak:");

        if (found.Food > 0)
            Console.WriteLine("Élelem: +" + found.Food);

        if (found.Water > 0)
            Console.WriteLine("Víz: +" + found.Water);

        if (found.Wood > 0)
            Console.WriteLine("Fa: +" + found.Wood);

        if (found.Stone > 0)
            Console.WriteLine("Kő: +" + found.Stone);

        if (found.Leaf > 0)
            Console.WriteLine("Levél: +" + found.Leaf);

        if (found.Rope > 0)
            Console.WriteLine("Kötél: +" + found.Rope);

        return true;
    }
    private int GetGatherBonus()
    {
        int bonus = 0;

        foreach (Island location in _discoveredLocations)
        {
            bonus += location.GatherBonus;
        }

        return bonus;
    }

    private int GetWaterBonus()
    {
        int bonus = 0;

        foreach (Island location in _discoveredLocations)
        {
            bonus += location.WaterBonus;
        }

        return bonus;
    }

    private int GetMaterialBonus()
    {
        int bonus = 0;

        foreach (Island location in _discoveredLocations)
        {
            bonus += location.MaterialBonus;
        }

        return bonus;
    }

    private int GetRestBonus()
    {
        int bonus = 0;

        foreach (Island location in _discoveredLocations)
        {
            bonus += location.RestBonus;
        }

        return bonus;
    }
}
