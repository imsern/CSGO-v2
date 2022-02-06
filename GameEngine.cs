namespace CSGO_v2;

public class GameEngine
{
    private int Matchrounds { get; set; }
    private int Maxround { get; set; }

    public GameEngine(int matchrounds, int maxround)
    {
        Matchrounds = matchrounds;
        Maxround = maxround;
    }
    public async Task Run()
    {
        var match = new Match(Matchrounds, Maxround);
        match.Introduction();
        var txt = Console.ReadLine()!.ToLower();
        if (txt == "start")
            while (!match.GameEnded)
            {                               // This section will be used to reset all values to normal before a round restarts
                await match.StartOrResetRounds();
                match.EconomyCheck("CT");
                match.EconomyCheck("T");
                match.ChooseSiteBoth();
                while (!match.RoundEnded) // Everything happening while a bomb isnt planted
                {
                    match.PrintPlayerInfo();
                    match.Fight();
                    if (!match.CheckSiteDeaths() && !Match.BombIsPlanted) match.ChooseTandPlantBomb();
                    if (!match.RoundEnded)
                    {
                        if (Match.BombIsPlanted)
                        {
                            match.CountDown();
                        }
                    } 
                    match.CheckWinConditions();
                    await Task.Delay(1000);
                    Console.Clear();
                }
            }
    }
}