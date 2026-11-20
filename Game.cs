using Castaway;
using System;
using System.Text;

public class Game
{
    private Survivor _survivor;

    public void Run()
    {
        Console.OutputEncoding = Encoding.UTF8; // hogy az ékezetes betűk jól jelenjenek meg
        Console.WriteLine(Texts.Title);
        Console.WriteLine();
        Console.WriteLine(Texts.Intro);
        Console.WriteLine();

        // K1: névbekérés
        string name = AskName();
        if (name == null)
        {
            Console.WriteLine("Nincs több bemenet, a játék véget ér.");
            return;
        }
        _survivor = new Survivor(name);

        Console.WriteLine($"Üdv a szigeten, {_survivor.Name}!");
        Console.WriteLine();

        // TODO: a játék főciklusa (K1-K6)

        Console.WriteLine("Nyomj meg egy billentyűt a kilépéshez.");
        Console.ReadKey(true);
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
