using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CSGO_v2;

public class Terrorist : Player
{
    private Random rnd = new();


    private List<Weapon> _tweps = new()
    {
        new Weapon("Glock", 10, 150, 70),
        new Weapon("Deagle", 20, 750, 50),
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
        Weapon = _tweps[0];
    }

    public async Task PlantBomb()
    {
        Console.WriteLine($"{Name} is Planting the bomb!");
        await Task.Delay(1000);
        Console.WriteLine($".");
        await Task.Delay(1000);
        Console.WriteLine($"..");
        await Task.Delay(1000);
        Console.WriteLine($"...");
        await Task.Delay(1000);
        Console.WriteLine($"....");
        await Task.Delay(1000);
        Console.WriteLine($"Bomb has been planted!");
        
    }
    
    public async Task ChooseSite(int site) // velger om Terror går A eller B
    {
        
            if (site <= 4)
            {
                chosenSite = 'A';
            }
            else
            {
                chosenSite = 'B';
            }
        
    }

    public void Shoot(CounterTerrorist target)
    {
        if (Health <= 0) isDead = true;
        if (isDead || target.isDead) return;
        var hit = rnd.Next(0, 100);
        if (hit <= Weapon.Accuracy)
        {
            //Console.WriteLine($"Terrorist: {Name} hits {target.Name} for {Weapon.Damage} damage.");
            target.Health -= Weapon.Damage;
            if (target.Health <= 0)
            {
                target.isDead = true;
                Money += 300;
                Console.WriteLine($"{target.Name} died!");

            }
        }
        //else Console.WriteLine($"Terrorist: {Name} missed {target.Name}! ");
    }

    public bool CheckTeamEco(int value)
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
                Console.WriteLine($"{Name} bought {_tweps[3].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2900 when has3k >= 5:
                BuyWep("AK");
                Console.WriteLine($"{Name} bought {_tweps[2].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            case > 2000 when has2k >= 5:
                BuyWep("Deagle");
                Console.WriteLine($"{Name} bought {_tweps[1].Name}");
                BuyArmor();
                //Console.WriteLine($"{Name} has ${Money} left");
                break;
            default: return;
        }
    }

    public void BuyWep(string weapon)
    {
        if (weapon == "AWP")
        {
            Weapon = _tweps[3];
            Money -= _tweps[3].Cost;
        }
        if (weapon == "AK")
        {
            Weapon = _tweps[2];
            Money -= _tweps[2].Cost;
        }
        if (weapon == "Deagle")
        {
            Weapon = _tweps[1];
            Money -= _tweps[1].Cost;
        }
    }

    public void BuyArmor()
    {
        if (Money > 1000)
        {
            Armor = 50;
            Health = 100 + Armor;
            Money -= 1000;
            Console.WriteLine($"{Name} bought Armor");
        }
    }
}