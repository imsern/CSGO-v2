namespace CSGO_v2;

public interface IPlayer
{
    public void Shoot(Player target, string player);
    public void BuyWep(string weapon);
    public void BuyArmor();
}