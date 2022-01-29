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
    public static bool BombIsPlanted { get; set; }
    public int BombTimer { get; set; }
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
        BombTimer = 30;
        MatchRound = matchround;
        MaxRounds = maxrounds;
        BombIsPlanted = false;
        GameEnded = false;
        RoundEnded = false;
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

    public async Task ChanceToPlant()
    {
        var toPlantOrNotToPlant = random.Next(0, 7) == 3;
        if (toPlantOrNotToPlant)
        {
            ChooseTandPlantBomb();
        }
    }

    public async Task ChooseTandPlantBomb()
    {
        var randomPlayer = PickRandomPlayer(Terrorists);
        if (randomPlayer != -1)
        {
            Terrorists[randomPlayer].PlantBomb();
            BombIsPlanted = true;
        }
    }

    public void StartOrResetRounds()
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

    public void PrintPlayerInfo()
    {
        Console.WriteLine($"Terrorists are going: {Terrorists[0].chosenSite}");
        Console.Write($"CT is going: ");
        foreach (var ct in CounterTerrorists)
        {
            Console.Write($"{ct.chosenSite}, ");
        }
        Console.Write("\n");
    }

    public void CountDown()
    {
        while (!BombDefused || !RoundEnded)
        {
            BombTimer--;
        }
    }

    public async Task AfterPlantWinCheck()
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
        }
        // T = 0, CT = 1
        var totalDeaths = CountDownDeathCheck();
        var randomPlayer = PickRandomPlayer(CounterTerrorists);
        if (totalDeaths[0] == 5 && randomPlayer != -1) CounterTerrorists[randomPlayer].DefuseBomb();
        if (totalDeaths[1] == 5) BombTimer = 0;
        if (BombTimer <= 0)
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
        }
    }

    public int PickRandomPlayer(List<Terrorist> playerList)
    {
        var teamMemberAliveList = playerList.FindAll(x => x.isDead == false).ToList();
        int index = random.Next(0, teamMemberAliveList.Count);
        var playerPlanting = -1; // returns -1 if everyone is dead on the team to stop the plant from happening.
        if (teamMemberAliveList.Count > 0)
            playerPlanting = playerList.FindIndex(x => x.Name == teamMemberAliveList[index].Name);
        return playerPlanting;
    }
    public int PickRandomPlayer(List<CounterTerrorist> playerList)
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

    public void ChooseTarget(char tSite)
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
    }
    public void ChooseTarget()
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

    public int[] CountDownDeathCheck() // This one is used while Countdown is running
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
    public bool checkSiteDeaths() // Is used to check before bomb is planted
    {
        var isAlive = true;
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
                        ct.ChooseSite();
                    }

                    var randomSite = random.Next(0, 9);
                    foreach (var t in Terrorists)
                    {
                        t.ChooseSite(randomSite);
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

    public void Introduction()
    {
        Console.WriteLine($"Welcome to Counter-Stroik: Ginger Offensive!");
        Console.WriteLine($"Match is set to MR {MatchRound} with max rounds: {MaxRounds}");
        Console.WriteLine($"First team to reach {MatchRound} rounds win!");
        Console.WriteLine($"Whenever you're ready type - Start");
    }
}