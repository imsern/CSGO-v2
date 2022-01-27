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
    public static bool BombDefused { get; set; }
    public bool RoundEnded { get; set; }
    public bool GameEnded { get; set; }
    public static int Tscore { get; set; }
    public static int CTscore { get; set; }
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
            StartOrResetRounds();
            EconomyCheck("CT");
            EconomyCheck("T");
            await chooseSiteBoth();
            PrintPlayerInfo();
            await Task.Delay(1000);
            var isAlive = true;
            while (isAlive) // Loops through the first fight of the round, until one of the teams on the same site is dead.
            {
                Fight();
                isAlive = checkSiteDeaths(isAlive);
                await Task.Delay(1000);
            }
            if (!BombIsPlanted) // if CT dies on the site, we go further to the plantbomb method, and CT will move towards the bomb. Then the fight will happen again
            {
                var randomPlayer = PickRandomPlayer(Terrorists);
                if (randomPlayer != -1)
                {
                    await Terrorists[randomPlayer].PlantBomb();
                    BombIsPlanted = true;
                    await chooseSiteBoth();
                    await CountDown();
                }
            }
            await Task.Delay(3000);
            Console.Clear();
        }
    }

    private void StartOrResetRounds()
    {
        foreach (var ct in CounterTerrorists)
        {
            if(ct.isDead){
                ct.isDead = false;
                ct.defusekit = false;
                ct.Weapon = ct._cTweps[0];
                ct.Health = 100;
                ct.Armor = 0;
            }

            if (ct.Health < 100)
            {
                ct.Health = 100;
                ct.Armor = 0;
            }
            ct.Health = 100 + ct.Armor;
        }
        foreach (var t in Terrorists)
        {
            if(t.isDead){
                t.isDead = false;
                t.Weapon = t._tweps[0];
                t.Health = 100;
                t.Armor = 0;
            }

            if (t.Health < 100)
            {
                t.Health = 100;
                t.Armor = 0;
            }
            t.Health = 100 + t.Armor;
        }
        RoundEnded = false;
        BombDefused = false;
        BombIsPlanted = false;

        Console.ForegroundColor = ConsoleColor.White;
        Round++;
        Console.WriteLine($"Runde: {Round}");
        Console.WriteLine($"CT: {CTscore}");
        Console.WriteLine($" T: {Tscore}");
    }

    private void PrintPlayerInfo()
    {
        Console.WriteLine($"Terrorists are going: {Terrorists[0].chosenSite}");
        Console.Write($"CT is going: ");
        foreach (var ct in CounterTerrorists)
        {
            Console.Write($"{ct.chosenSite}, ");
        }
        Console.Write("\n");
    }

    public async Task CountDown()
    {
        var index = 30;
        
        while (!BombDefused || !RoundEnded)
        {
            if (BombDefused)
            {
                Console.WriteLine($"Counter-Terrorists win!");
                CTscore++;
                foreach (var ct in CounterTerrorists)
                {
                    ct.Money += 3500;
                }
                foreach (var t in Terrorists)
                {
                    t.Money += 2000;
                }
                RoundEnded = true;
                BombIsPlanted = false;
                break;
            }
            Console.WriteLine($"Timer: {index}");
            await Task.Delay(500);
            Fight();
            await Task.Delay(500);
            // T = 0, CT = 1
            var totalDeaths = checkSiteDeaths();
            var randomPlayer = PickRandomPlayer(CounterTerrorists);
            if (totalDeaths[0] == 5 && randomPlayer != -1) CounterTerrorists[randomPlayer].DefuseBomb();
            if (totalDeaths[1] == 5) index = 0;
            index--;
            if (index <= 0)
            {
                Console.WriteLine($"Terrorists win!");
                Tscore++;
                foreach (var t in Terrorists)
                {
                    t.Money += 3500;
                }
                foreach (var ct in CounterTerrorists)
                {
                    ct.Money += 2000;
                }
                RoundEnded = true;
                BombIsPlanted = false;
                break;
            }
        }
    }
    private int PickRandomPlayer(List<Terrorist> playerList)
    {
        var teamMemberAliveList = playerList.FindAll(x => x.isDead == false).ToList();
        int index = random.Next(0, teamMemberAliveList.Count);
        var playerPlanting = -1; // returns -1 if everyone is dead on the team to stop the plant from happening.
        if (teamMemberAliveList.Count > 0)
            playerPlanting = playerList.FindIndex(x => x.Name == teamMemberAliveList[index].Name);
        return playerPlanting;
    }
    private int PickRandomPlayer(List<CounterTerrorist> playerList)
    {
        var teamMemberAliveList = playerList.FindAll(x => x.isDead == false).ToList();
        int index = random.Next(0, teamMemberAliveList.Count);
        var playerPlanting = -1; // returns -1 if everyone is dead on the team to stop the plant from happening.
        if (teamMemberAliveList.Count > 0)
            playerPlanting = playerList.FindIndex(x => x.Name == teamMemberAliveList[index].Name);
        return playerPlanting;
    }

    public void Fight()
    {
        var tSite = Terrorists[0].chosenSite;   // DETTE ER DER TERROR VELGER Å GÅ PER RUNDE
            var whoShootsFirst = random.Next(0, 9);

        if (whoShootsFirst <= 4)  // CT SKYTER FØRST
        {
            ChooseTarget(tSite);
        }
        if (whoShootsFirst >= 5) // T SKYTER FØRST
        {
            ChooseTarget();
        }


    }

    private void ChooseTarget(char tSite)
    {
        var teamAliveList = CounterTerrorists.FindAll(x => x.isDead == false).ToList();

        if (teamAliveList.Count <=0) return;

        var teamRandomAliveIndex = random.Next(0, teamAliveList.Count);
        var teamIndex = CounterTerrorists.FindIndex(x => x.Name == teamAliveList[teamRandomAliveIndex].Name);

        var enemyAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();

        if (enemyAliveList.Count <= 0) return;

        var enemyRandomAliveIndex = random.Next(0, enemyAliveList.Count);
        var enemyIndex = Terrorists.FindIndex(x => x.Name == enemyAliveList[enemyRandomAliveIndex].Name);
        if (tSite != CounterTerrorists[teamIndex].chosenSite)
        {
            return;
        }
        CounterTerrorists[teamIndex].Shoot(Terrorists[enemyIndex]);
        Terrorists[enemyIndex].Shoot(CounterTerrorists[teamIndex]);
        Console.Write("\n");
    }
    private void ChooseTarget()
    {
        var teamAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();

        if (teamAliveList.Count <= 0) return;

        var teamRandomAliveIndex = random.Next(0, teamAliveList.Count);
        var teamIndex = Terrorists.FindIndex(x => x.Name == teamAliveList[teamRandomAliveIndex].Name);

        var enemyAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();
        
        if (enemyAliveList.Count <= 0) return;

        var enemyRandomAliveIndex = random.Next(0, enemyAliveList.Count);
        var enemyIndex = Terrorists.FindIndex(x => x.Name == enemyAliveList[enemyRandomAliveIndex].Name);
        if (Terrorists[teamIndex].chosenSite != CounterTerrorists[teamIndex].chosenSite)
        {
            return;
        }
        Terrorists[teamIndex].Shoot(CounterTerrorists[enemyIndex]);
        CounterTerrorists[enemyIndex].Shoot(Terrorists[teamIndex]);
        Console.Write("\n");
    }

    private int[] checkSiteDeaths()
    {
        var totalDead = new int[2];
        foreach (var t in Terrorists)
        {
            if (t.isDead == true) totalDead[0]++;
        }
        foreach (var ct in CounterTerrorists)
        {
            if (ct.isDead == true) totalDead[1]++;
        }

        return totalDead;
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
            foreach (var ct in CounterTerrorists)
            {
                ct.Money += 3500;
            }
            foreach (var t in Terrorists)
            {
                t.Money += 2000;
            }
            isAlive = false;
            if (!isAlive && BombIsPlanted == false)
            {
                RoundEnded = true;
                CTscore++;
            }
        }

        if (ctDead == 5)
        {
            Console.WriteLine($"Terrorists win!");
            foreach (var t in Terrorists)
            {
                t.Money += 3500;
            }
            foreach (var ct in CounterTerrorists)
            {
                ct.Money += 3500;
            }
            Tscore++;
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

        var currentSite = Terrorists[0].chosenSite;
        if ((
                (ctAsiteDead == siteCountA && siteCountA != 0) ||
             (ctBsiteDead == siteCountB && siteCountB != 0)) ||
            (currentSite == 'A' && siteCountA == 0 || 
             currentSite == 'B' && siteCountB == 0))
            isAlive = false;
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