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
    private string[] _timeNames = { "reggel", "dél", "délután", "este" };
    private List<string> _buildings = new List<string>();

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
                Console.WriteLine("Barkácsolás: ez később lesz kész.");
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
            _survivor.AddEnergy(RestGain);
            Console.WriteLine(RandomText(Texts.RestTexts) + " +" + RestGain + " energia.");
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

}
