using System;
using System.Threading.Tasks;

namespace CSGO_v2;

public class CounterTerrorist : Player
{
    private Random rnd = new();

    public bool defusekit { get; set; }
    public readonly List<Weapon> _cTweps = new()
    {
        new Weapon("USP", 15, 250, 90),
        new Weapon("Deagle", 25, 750, 50),
        new Weapon("M4A1", 40, 3000, 55),
        new Weapon("AWP", 150, 4900, 20)
    };
    public CounterTerrorist(string name)
    {
        Name = name;
        Team = "CT";
        Health = 100;
        Armor = 0;
        Money = 800;
        isDead = false;
        Weapon = _cTweps[0];
    }

    public async Task DefuseBomb()
    {
        Console.WriteLine($"{Name} is defusing the bomb!");
        if (defusekit) await Task.Delay(3000);
        else await Task.Delay(5000);
        Match.BombDefused = true;
        Match.CTscore++;
        Console.WriteLine("Bomb has been defused!");
        await Task.Delay(1000);
    }

    public new void ChooseSite() // sette 2 til å gå B - 3 til å gå A, RAndom hver runde
    {
        var randomSite = rnd.Next(0, 9);
        if (randomSite <= 4) chosenSite = 'A';
        else chosenSite = 'B';
    }

    public void Shoot(Terrorist target)
    {
        if (Health <= 0) isDead = true;
        if (isDead || target.isDead) return;
        var hit = rnd.Next(0, 100);
        if (hit <= Weapon.Accuracy)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{Name} hits {target.Name} for {Weapon.Damage} damage.");
            target.Health -= Weapon.Damage;
            if (target.Health <= 0)
            {
                target.isDead = true;
                target.Health = 0;
                Money += 300;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{target.Name} died!");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{Name} missed {target.Name}!");
        }
    }

    public new bool CheckTeamEco(int value)
    {
        if (Money > value)
        {
            return true;
        }
        return false;
    }

    public void CheckPlayerEco(int has2k, int has3k)
    {
        switch (Money)
        {
            case > 4900 when has3k >= 4:
                BuyWep("AWP");
                Console.WriteLine($"{Name} bought {_cTweps[3].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2900 when has3k >= 5:
                BuyWep("M4A1");
                Console.WriteLine($"{Name} bought {_cTweps[2].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2000 when has2k >= 5:
                BuyWep("Deagle");
                Console.WriteLine($"{Name} bought {_cTweps[1].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            default: return;
        }
    }

    public new void BuyWep(string weapon)
    {
        if (weapon == "AWP")
        {
            Weapon = _cTweps[3];
            Money -= _cTweps[3].Cost;
        }
        if (weapon == "M4A1")
        {
            Weapon = _cTweps[2];
            Money -= _cTweps[2].Cost;
        }
        if (weapon == "Deagle")
        {
            Weapon = _cTweps[1];
            Money -= _cTweps[1].Cost;
        }
    }

    public new void BuyArmor()
    {
        if (Money > 450 && defusekit == false)
        {
            defusekit = true;
            Money -= 450;
            Console.WriteLine($"{Name} bought Defuse Kit");
        }
        if (Money > 1000 && Armor == 0)
        {
            Armor = 50;
            Health = 100 + Armor;
            Money -= 1000;
            Console.WriteLine($"{Name} bought Armor");
        }
    }
}