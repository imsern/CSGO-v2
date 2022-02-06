﻿namespace CSGO_v2;

public class CounterTerrorist : Player
{
    private readonly Random _rnd = new();

    public bool Defusekit { get; set; }
    public readonly List<Weapon> CTweps = new()
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
        Weapon = CTweps[0];
    }

    public async Task DefuseBomb()
    {
        Console.WriteLine($"{Name} is defusing the bomb!");
        if (Defusekit)
        {
            Match.BombDefused = true;
            Match.BombIsPlanted = false;
            Console.WriteLine("Bomb has been defused!");
            await Task.Delay(3000);
        }
        else
            await Task.Delay(5000);
        Match.BombDefused = true;
        Match.BombIsPlanted = false;
        Console.WriteLine("Bomb has been defused!");
    }

    public new void ChooseSite() // sette 2 til å gå B - 3 til å gå A, RAndom hver runde
    {
        var randomSite = _rnd.Next(0, 9);
        chosenSite = randomSite <= 4 ? 'A' : 'B';
    }

    public void Shoot(Terrorist target)
    {
        if (Health <= 0) isDead = true;
        if (isDead || target.isDead) return;
        var hit = _rnd.Next(0, 100);
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

    public void CheckPlayerEco(int has2K, int has3K)
    {
        switch (Money)
        {
            case > 4900 when has3K >= 4:
                BuyWep("AWP");
                Console.WriteLine($"{Name} bought {CTweps[3].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2900 when has3K >= 5:
                BuyWep("M4A1");
                Console.WriteLine($"{Name} bought {CTweps[2].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2000 when has2K >= 5:
                BuyWep("Deagle");
                Console.WriteLine($"{Name} bought {CTweps[1].Name}");
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
                Weapon = CTweps[3];
                Money -= CTweps[3].Cost;
                break;
            case "M4A1":
                Weapon = CTweps[2];
                Money -= CTweps[2].Cost;
                break;
            case "Deagle":
                Weapon = CTweps[1];
                Money -= CTweps[1].Cost;
                break;
        }
    }

    private new void BuyArmor()
    {
        if (Money > 450 && Defusekit == false)
        {
            Defusekit = true;
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