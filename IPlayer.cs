namespace CSGO_v2;

public interface IPlayer
{
    public string Name { get; set; }
    public string Team { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public int Armor { get; set; }
    public int Money { get; set; }
    public bool isDead { get; set; }
    public char chosenSite { get; set; }


    
    

    public void ChooseSite();
    public void Shoot(Player target);

    public bool CheckTeamEco(int value);

    public void CheckPlayerEco(int has2k, int has3k, string team);
    public void BuyWep(string weapon);
    public void BuyArmor();
}