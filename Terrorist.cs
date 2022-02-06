﻿namespace CSGO_v2;

public class Terrorist : Player
{
    private readonly Random _rnd = new();


    public readonly List<Weapon> Tweps = new()
    {
        new Weapon("Glock", 15, 250, 90),
        new Weapon("Deagle", 25, 750, 50),
        new Weapon("AK", 40, 3000, 55),
        new Weapon("AWP", 150, 4900, 30)
    };
    public Terrorist(string name)
    {
        Name = name;
        Team = "T";
        Health = 100;
        Armor = 0;
        Money = 800;
        isDead = false;
        Weapon = Tweps[0];
    }

    public async Task PlantBomb()
    {
        Console.WriteLine($"{Name} is Planting the bomb!");
        await  Task.Delay(1000);
        Console.WriteLine($".");
        await  Task.Delay(1000);
        Console.WriteLine($"..");
        await  Task.Delay(1000);
        Console.WriteLine($"...");
        await  Task.Delay(1000);
        Console.WriteLine($"....");
        await  Task.Delay(1000);
        Console.WriteLine($"Bomb has been planted!");
    }
    
    public void ChooseSite(int site) // velger om Terror går A eller B
    {
        chosenSite = site <= 4 ? 'A' : 'B';
    }

    public void Shoot(CounterTerrorist target)
    {
        if (Health <= 0) isDead = true;
        if (isDead || target.isDead) return;
        var hit = _rnd.Next(0, 100);
        if (hit <= Weapon.Accuracy)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
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
            Console.WriteLine($"{Name} missed {target.Name}! ");
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

    public void CheckPlayerEco(int has2K, int has3K)
    {
        switch (Money)
        {
            case > 4900 when has3K >= 4:
                BuyWep("AWP");
                Console.WriteLine($"{Name} bought {Tweps[3].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2900 when has3K >= 5:
                BuyWep("AK");
                Console.WriteLine($"{Name} bought {Tweps[2].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2000 when has2K >= 5:
                BuyWep("Deagle");
                Console.WriteLine($"{Name} bought {Tweps[1].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            default: return;
        }
    }

    private new void BuyWep(string weapon)
    {
        switch (weapon)
        {
            case "AWP":
                Weapon = Tweps[3];
                Money -= Tweps[3].Cost;
                break;
            case "AK":
                Weapon = Tweps[2];
                Money -= Tweps[2].Cost;
                break;
            case "Deagle":
                Weapon = Tweps[1];
                Money -= Tweps[1].Cost;
                break;
        }
    }

    private new void BuyArmor()
    {
        if (Money > 1000 && Armor == 0)
        {
            Armor = 50;
            Health = 100 + Armor;
            Money -= 1000;
            Console.WriteLine($"{Name} bought Armor");
        }
    }
}