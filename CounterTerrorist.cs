﻿namespace CSGO_v2;

public class CounterTerrorist : Player
{
    private readonly Random _rnd = new();
    private ViewPrint vp = new ViewPrint();

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
        Match.DefuseStarted = true;
        vp.PrintCenter($"{Name} is defusing the bomb!");
        if (Defusekit)
        {
            await Task.Delay(3000);
            Match.BombDefused = true;
            Match.BombIsPlanted = false;
            vp.PrintCenter("Bomb has been defused!");
            Match.DefuseStarted = false;
        }
        else
            await Task.Delay(5000);
        Match.BombDefused = true;
        Match.BombIsPlanted = false;
        vp.PrintCenter("Bomb has been defused!");
        Match.DefuseStarted = false;
    }

    public void ChooseSite() // sette 2 til å gå B - 3 til å gå A, RAndom hver runde
    {
        var randomSite = _rnd.Next(0, 9);
        chosenSite = randomSite <= 4 ? 'A' : 'B';
    }
    
    public bool CheckTeamEco(int value)
    {
        if (Money > value)
        {
            return true;
        }
        return false;
    }

    public void CheckPlayerEco(int has2K, int has3K)
    {
        if (Weapon != CTweps[0]) return;
        switch (Money)
        {
            case > 4900 when has3K >= 4:
                BuyWep("AWP");
                BuyArmor();
                break;
            case > 2900 when has3K >= 5:
                BuyWep("M4A1");
                BuyArmor();
                
                break;
            case > 2000 when has2K >= 5:
                BuyWep("Deagle");
                BuyArmor();
                
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
        }
        if (Money > 1000 && Armor == 0)
        {
            Armor = 50;
            Health = 100 + Armor;
            Money -= 1000;
        }
    }
}