using Castaway;
using System;
using System.Text;

public class Game
{
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
        while (running)
        {
            ShowStatus();
            ShowMenu();
            int choice = ReadNumber(1, 7);
            Console.WriteLine();

            if (choice >= 1 && choice <= 4)
            {
                if (DoAction(choice))
                {
                    AdvanceTime();
                }
            }
            else if (choice == 5)
            {
                if (Craft())
                {
                    AdvanceTime();
                }
            }
            else if (choice == 6)
            {
                ShowInventory();
            }
            else
            {
                running = false;
            }
            Console.WriteLine();
        }

        Console.WriteLine("Viszlát, " + _survivor.Name + "!");
        Console.WriteLine("Nyomj Entert a kilépéshez.");
        Console.ReadLine();
    }
    private bool DoAction(int choice)
    {
        if (choice != 4 && _survivor.Energy < ActionCost)
        {
            Console.WriteLine("Túl fáradt vagy, ehhez legalább " + ActionCost
                + " energia kell. Most csak pihenni tudsz.");
            return false;
        }

        if (choice == 4)
        {
            int gain = RestGain;

            if (_buildings.Contains("Menedék"))
            {
                gain = 7;
            }

            _survivor.AddEnergy(gain);

            Console.WriteLine(RandomText(Texts.RestTexts) + " +" + gain + " energia.");

            return true;
        }

        _survivor.DeleteEnergy(ActionCost);

        if (choice == 1)
        {
            int gain = _random.Next(2, 5);
            _survivor.AddFood(gain);
            Console.WriteLine(RandomText(Texts.GatherTexts) + " +" + gain + " élelem.");
        }
        else if (choice == 2)
        {
            int gain = _random.Next(2, 5);

            if (_buildings.Contains("Tűzrakó hely"))
            {
                gain += 2;
            }

            _survivor.AddWater(gain);

            Console.WriteLine(RandomText(Texts.WaterTexts) + " +" + gain + " víz.");
        }
        else
        {
            int wood = _random.Next(1, 4);
            int stone = _random.Next(0, 3);
            int leaf = _random.Next(1, 4);
            _survivor.AddWood(wood);
            _survivor.AddStone(stone);
            _survivor.AddLeaf(leaf);
            Console.WriteLine(RandomText(Texts.MaterialTexts)
                + " +" + wood + " fa, +" + stone + " kő, +" + leaf + " levél.");
        }
        return true;
    }

    private void AdvanceTime()
    {
        _timeOfDay++;
        if (_timeOfDay > 2)
        {
            _timeOfDay = 0;
            _day++;
            Console.WriteLine("Leszállt az éj, majd újra felkelt a nap. Új nap kezdődik.");
        }
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
        Console.WriteLine("[6] Leltár");
        Console.WriteLine("[7] Kilépés");
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

}
