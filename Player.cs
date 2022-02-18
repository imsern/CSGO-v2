namespace CSGO_v2;

public class Player : IPlayer
{
    private readonly Random _rnd = new();
    public string Name { get; protected init; }
    protected string Team { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public int Armor { get; set; }
    public int Money { get; set; }
    public bool isDead { get; set; }
    public char chosenSite { get; set; }
    private readonly ViewPrint _vp = new ViewPrint();
    
    public void Shoot(Player target, string player)
    {
        if (Health <= 0) isDead = true;
        if (isDead || target.isDead) return;
        var hit = _rnd.Next(0, 100);
        if (hit <= Weapon.Accuracy)
        {
            if(player == "CT") Console.ForegroundColor = ConsoleColor.Cyan;
            if(player == "T") Console.ForegroundColor = ConsoleColor.Yellow;
            _vp.PrintCenter($"{Name} hits {target.Name} for {Weapon.Damage} damage.");
            target.Health -= Weapon.Damage;
            if (target.Health <= 0)
            {
                target.isDead = true;
                target.Health = 0;
                Money += 300;
                Console.ForegroundColor = ConsoleColor.Red;
                _vp.PrintCenter($"{target.Name} died!");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            _vp.PrintCenter($"{Name} missed {target.Name}!");
        }
    }
    
    public void BuyWep(string weapon)
    {
        throw new NotImplementedException();
    }

    public void BuyArmor()
    {
        throw new NotImplementedException();
    }
}