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
        if (txt == "start") Console.Clear();
            while (!match.GameEnded)
            {                               // This section will be used to reset all values to normal before a round restarts
                await match.StartOrResetRounds();
                match.EconomyCheck("CT");
                await Task.Delay(2500);
                match.EconomyCheck("T");
                await Task.Delay(2500);
                match.ChooseSiteBoth();
                Console.Clear();
                while (!match.RoundEnded) // Everything happening while a bomb isnt planted
                {
                    match.PrintMatchInfo();
                    match.PlayerInfo();
                    match.Fight();
                    if (!match.CheckSiteDeaths() && !Match.BombIsPlanted) await match.ChooseTandPlantBomb();
                    if (!match.RoundEnded)
                    {
                        if (Match.BombIsPlanted)
                        {
                            var totalDeaths = match.CountDownDeathCheck();
                            var randomPlayer = match.PickRandomPlayer(match.CounterTerrorists);
                            if (totalDeaths[0] == 5 && randomPlayer != -1 && !Match.DefuseStarted) match.CounterTerrorists[randomPlayer].DefuseBomb();
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