using System.Runtime.CompilerServices;
using System;
using System.Threading.Tasks;

namespace CSGO_v2;

public class Match
{
    private Random random = new();
    public int Round { get; set; }
    public int MatchRound { get; set; }
    public int MaxRounds { get; set; }
    public bool BombIsPlanted { get; set; }
    public bool BombDefused { get; set; }
    //public int BombTimer { get; set; }
    public bool RoundEnded { get; set; }
    public bool GameEnded { get; set; }
    public int Tscore { get; set; }
    public int CTscore { get; set; }
    public static List<CounterTerrorist> CounterTerrorists = new();
    public static List<Terrorist> Terrorists = new();
    private readonly List<string> _teamNames1 = new()
    {
        "GeT_g00d Terje",
        "GeT_g00d BJ",
        "GeT_g00d Marie",
        "GeT_g00d Schrodern",
        "GeT_g00d Linn"
    };
    private readonly List<string> _teamNames2 = new()
    {
        "Modu13 Krist-Offer",
        "Modu13 StighY",
        "Modu13 Johnny-E",
        "Modu13 Kenny",
        "Modu13 Stian"
    };


    public Match(int matchround, int maxrounds)
    {
        Tscore = 0;
        CTscore = 0;
        Round = 0;
        MatchRound = matchround;
        MaxRounds = maxrounds;
        BombIsPlanted = false;
        GameEnded = false;
        initPlayers();
    }
    private void initPlayers()
    {
        for (int i = 0; i < 5; i++)
        {
            CounterTerrorists.Add(new CounterTerrorist(_teamNames2[i]));
            Terrorists.Add(new Terrorist(_teamNames1[i]));
        }
    }
    public void Startmatch()
    {
        Introduction();
    }

    public async Task StartRound()
    {
        while (!GameEnded)
        {
            RoundEnded = false;
            Round++;
            Console.WriteLine($"Runde: {Round}");
            EconomyCheck("CT");
            EconomyCheck("T");
            await chooseSiteBoth();
            PrintPlayerInfo();
            await Task.Delay(1000);
            await Fight();
            if (!BombIsPlanted) await Terrorists[PickRandomPlayer(Terrorists)].PlantBomb();
            BombIsPlanted = true;
            await chooseSiteBoth();
            PrintPlayerInfo();
            await AfterPlant();
            //if (BombIsPlanted)
            await Task.Delay(1000);
            Console.Clear();
        }
    }

    private async Task AfterPlant()
    {
        //Fight();
        //await CountDown();
        Parallel.Invoke(() => Fight(), () => CountDown());
    }

    private void PrintPlayerInfo()
    {
        foreach (var ct in CounterTerrorists)
        {
            Console.WriteLine($"{ct.Name} hp: {ct.Health} money: {ct.Money} site: {ct.chosenSite}");
        }
        foreach (var t in Terrorists)
        {
            Console.WriteLine($"{t.Name} hp: {t.Health} money: {t.Money} site: {t.chosenSite}");
        }
    }

    public async Task CountDown()
    {
        var index = 15;
        while (!BombDefused || RoundEnded)
        {
            if (BombDefused)
            {
                Console.WriteLine($"Counter-Terrorists win!");
                RoundEnded = true;
                BombIsPlanted = false;
                break;
            }
            Console.WriteLine($"Timer: {index}");
            await Task.Delay(1000);
            index--;
            if (index <= 0)
            {
                Console.WriteLine($"Terrorists win!");
                RoundEnded = true;
                BombIsPlanted = false;
                break;
            }
        }
        //while (!BombDefused || !RoundEnded)
        //{
        //    for (int i = 15; i > 0; i--)
        //    {
        //        if (BombDefused)
        //        {
        //            Console.WriteLine($"Counter-Terrorists win!");
        //            RoundEnded = true;
        //            break;
        //        }
        //        Console.WriteLine($"Timer: {i}");
        //        await Task.Delay(1000);
        //        if (i <= 0)
        //        {
        //            Console.WriteLine($"Terrorists win!");
        //            RoundEnded = true;
        //            break;
        //        }
        //    }
        //    break;
        //}
    }



    private int PickRandomPlayer(List<Terrorist> playerList)
    {
        var teamMemberAliveList = playerList.FindAll(x => x.isDead == false).ToList();
        int index = random.Next(0, teamMemberAliveList.Count);
        var playerPlanting = 0;
        if (teamMemberAliveList.Count > 0)
            playerPlanting = playerList.FindIndex(x => x.Name == teamMemberAliveList[index].Name);
        return playerPlanting;
    }

    public async Task Fight()
    {
        var isAlive = true;
        while (isAlive)
        {
            var tSite = Terrorists[0].chosenSite; // DETTE ER DER TERROR VELGER Å GÅ PER RUNDE
            var whoShootsFirst = random.Next(0, 9);

            if (whoShootsFirst <= 4) // CT SKYTER FØRST
            {
                ChooseTarget(tSite);
            }
            if (whoShootsFirst >= 5) // T SKYTER FØRST
            {
                ChooseTarget();
            }
            isAlive = checkSiteDeaths(isAlive);
        }

    }

    private void ChooseTarget(char tSite)
    {
        var teamAliveList = CounterTerrorists.FindAll(x => x.isDead == false).ToList();

        if (teamAliveList == null) return;

        var teamRandomAliveIndex = random.Next(0, teamAliveList.Count);
        var teamIndex = CounterTerrorists.FindIndex(x => x.Name == teamAliveList[teamRandomAliveIndex].Name);

        var enemyAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();
        var enemyRandomAliveIndex = random.Next(0, enemyAliveList.Count);
        var enemyIndex = Terrorists.FindIndex(x => x.Name == enemyAliveList[enemyRandomAliveIndex].Name);
        if (tSite != Terrorists[teamIndex].chosenSite)
        {
            return;
        }
        Terrorists[enemyIndex].Shoot(CounterTerrorists[teamIndex]);
        CounterTerrorists[teamIndex].Shoot(Terrorists[enemyIndex]);
    }
    private void ChooseTarget()
    {
        var teamAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();
        var teamRandomAliveIndex = random.Next(0, teamAliveList.Count);
        var teamIndex = Terrorists.FindIndex(x => x.Name == teamAliveList[teamRandomAliveIndex].Name);

        var enemyAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();
        var enemyRandomAliveIndex = random.Next(0, enemyAliveList.Count);
        var enemyIndex = Terrorists.FindIndex(x => x.Name == enemyAliveList[enemyRandomAliveIndex].Name);

        CounterTerrorists[enemyIndex].Shoot(Terrorists[teamIndex]);
        Terrorists[teamIndex].Shoot(CounterTerrorists[enemyIndex]);
    }

    private bool checkSiteDeaths(bool isAlive)
    {
        var ctAsiteDead = 0;
        var ctBsiteDead = 0;
        var ctDead = 0;
        var tDead = 0;
        foreach (var ct in CounterTerrorists)
        {
            if (ct.chosenSite == 'A' && ct.isDead == true) ctAsiteDead++;
            if (ct.chosenSite == 'B' && ct.isDead == true) ctBsiteDead++;
            if (ct.isDead == true) ctDead++;
        }
        foreach (var t in Terrorists)
        {
            if (t.isDead == true) tDead++;
        }

        if (tDead == 5)
        {
            Console.WriteLine($"Counter-Terrorists win!");
            isAlive = false;
            RoundEnded = true;
            if (!isAlive && BombIsPlanted == false) GameEnded = true;
        }

        if (ctDead == 5)
        {
            Console.WriteLine($"Terrorists win!");
            isAlive = false;
            RoundEnded = true;
        }

        var siteCountA = 0;
        var siteCountB = 0;
        foreach (var ct in CounterTerrorists)
        {
            if (ct.chosenSite == 'A') siteCountA++;
            if (ct.chosenSite == 'B') siteCountB++;
        }

        if (ctAsiteDead == siteCountA || ctBsiteDead == siteCountB) isAlive = false;
        return isAlive;
    }

    public async Task chooseSiteBoth()
    {
        switch (BombIsPlanted)
        {
            case false:
                {
                    foreach (var ct in CounterTerrorists)
                    {
                        await ct.ChooseSite();
                    }

                    var randomSite = random.Next(0, 9);
                    foreach (var t in Terrorists)
                    {
                        await t.ChooseSite(randomSite);
                    }

                    break;
                }
            case true:
                {
                    foreach (var ct in CounterTerrorists)
                    {
                        if (Terrorists[0].chosenSite == 'A') ct.chosenSite = 'A';
                        else if (Terrorists[0].chosenSite == 'B') ct.chosenSite = 'B';
                    }

                    break;
                }
        }
    }

    // må sjekke for 2k, 3k og 5k
    public void EconomyCheck(string team) // MÅ NOK STÅ I MATCH FOR Å KUNNE GÅ IGJENNOM ØKONOMI FOR HELE LAGET OG SENDE VIDERE TIL CHECKTEAMECO I HVERT ENKELT LAG
    {
        var has2k = 0;
        var has3k = 0;
        switch (team)
        {
            case "CT":
                {
                    foreach (var ct in CounterTerrorists)
                    {
                        if (ct.CheckTeamEco(3000))
                        {
                            has3k++;
                        }
                        else if (ct.CheckTeamEco(2000))
                        {
                            has2k++;
                        }
                    }

                    foreach (var ct in CounterTerrorists)
                    {
                        ct.CheckPlayerEco(has2k, has3k);
                    }

                    break;
                }
            case "T":
                {
                    foreach (var t in Terrorists)
                    {
                        if (t.CheckTeamEco(3000))
                        {
                            has3k++;
                        }
                        else if (t.CheckTeamEco(2000))
                        {
                            has2k++;
                        }
                    }

                    foreach (var t in Terrorists)
                    {
                        t.CheckPlayerEco(has2k, has3k);
                    }

                    break;
                }
        }
    }

    private void Introduction()
    {
        Console.WriteLine($"Welcome to Counter-Stroik: Ginger Offensive!");
        Console.WriteLine($"Match is set to MR {MatchRound} with max rounds: {MaxRounds}");
        Console.WriteLine($"First team to reach {MatchRound} rounds win!");
        Console.WriteLine($"Whenever you're ready type - Start");
    }
}