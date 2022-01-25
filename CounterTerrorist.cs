using System;
using System.Threading.Tasks;

namespace CSGO_v2;

public class CounterTerrorist : Player
{
    private Random rnd = new();

    public bool defusekit { get; set; }
    private readonly List<Weapon> _cTweps = new()
    {
        new Weapon("USP", 10, 250, 70),
        new Weapon("Deagle", 20, 750, 50),
        new Weapon("M4A1", 40, 3000, 55),
        new Weapon("AWP", 150, 4900, 20)
    };
    public CounterTerrorist(string name)
    {
        Name = name;
        Team = "CT";
        Health = 100;
        Armor = 0;
        Money = 4500;
        isDead = false;
        Weapon = _cTweps[0];
    }

    //public async Task FindRemaindingCT(List<CounterTerrorist> playerList)
    //{
    //    var teamMemberAliveList = playerList.FindAll(x => x.isDead == false).ToList();
    //    foreach (var ct in teamMemberAliveList)
    //    {
    //        Console.WriteLine($"{ct.Name} swapping from site: {ct.chosenSite}");
    //        if (chosenSite == 'A') chosenSite = 'B';
    //        else if (chosenSite == 'B') chosenSite = 'A';
    //    }
    //    Console.WriteLine("Remaining CT is going to the bomb");
        //await GoToBomb(teamMemberAliveList);
    //}

    //public async Task GoToBomb(List<CounterTerrorist> teamMemberAliveList)
    //{
    //    foreach (var ct in teamMemberAliveList)
    //    {
    //        if (chosenSite == 'A') chosenSite = 'B';
    //        if (chosenSite == 'B') chosenSite = 'A';
    //    }

    //    Console.WriteLine("Remaining CT is going to the bomb");
    //}

    public async Task ChooseSite() // sette 2 til å gå B - 3 til å gå A, RAndom hver runde
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
            //Console.WriteLine($"Counter-Terrorist: {Name} hits {target.Name} for {Weapon.Damage} damage.");
            target.Health -= Weapon.Damage;
            if (target.Health <= 0)
            {
                target.isDead = true;
                Money += 300;
                Console.WriteLine($"{target.Name} died!");
            }
        }
        //else Console.WriteLine($"Counter-Terrorist: {Name} missed {target.Name}! ");
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

    public void BuyWep(string weapon)
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

    public void BuyArmor()
    {
        if (Money > 450)
        {
            defusekit = true;
            Money -= 450;
            Console.WriteLine($"{Name} bought Defuse Kit");
        }
        if (Money > 1000)
        {
            Armor = 50;
            Health = 100 + Armor;
            Money -= 1000;
            Console.WriteLine($"{Name} bought Armor");
        }
    }
}