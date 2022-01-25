namespace CSGO_v2;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Cost { get; set; }
    public int Accuracy { get; set; }
    public Weapon(string name, int dmg, int cost, int accuracy)
    {
        Name = name;
        Damage = dmg;
        Cost = cost;
        Accuracy = accuracy;
    }
}