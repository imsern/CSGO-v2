namespace CSGO_v2;

public class Player : IPlayer
{
    
    public string Name { get; set; }
    public string Team { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public int Armor { get; set; }
    public int Money { get; set; }
    public bool isDead { get; set; }
    public char chosenSite { get; set; }

    public void ChooseSite()
    {
        throw new NotImplementedException();
    }

    public void Shoot(Player target)
    {
        throw new NotImplementedException();
    }

    public bool CheckTeamEco(int value)
    {
        throw new NotImplementedException();
    }

    public void CheckPlayerEco(int has2k, int has3k, string team)
    {
        throw new NotImplementedException();
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