﻿namespace CSGO_v2;

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
        InitPlayers();
    }

    private void InitPlayers()
    {
        for (var i = 0; i < 5; i++)
        {
            CounterTerrorists.Add(new CounterTerrorist(_teamNames2[i]));
            Terrorists.Add(new Terrorist(_teamNames1[i]));
        }
    }

    public void CheckWinConditions()
    {
        // Checks condition if everyone on a team is dead
        if (!BombIsPlanted)
        {
            var ctDead = CounterTerrorists.Count(ct => ct.isDead);
            var tDead = Terrorists.Count(t => t.isDead);
            if (tDead == 5)
            {
                CounterTerroristWins();
            }

            if (ctDead == 5)
            {
                TerroristWins();
            }
        }

        // T = 0, CT = 1
        // Checks condition if a team is to die after bomb is planted
        // If T's die CT wont win until bomb is defused
        if(BombIsPlanted){
            var totalDeaths = CountDownDeathCheck();
            var randomPlayer = PickRandomPlayer(CounterTerrorists);
            if (totalDeaths[0] == 5 && randomPlayer != -1) CounterTerrorists[randomPlayer].DefuseBomb();
            if (totalDeaths[1] == 5) BombTimer = 0;
            if (BombTimer <= 0)
            {
                TerroristWins();
            }
        }
        
        //Checks condition if Bomb is defused
        if (BombDefused)
        {
            CounterTerroristWins();
        }
        
    }

    public void CounterTerroristWins()
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

        if (BombIsPlanted == false)
        {
            RoundEnded = true;
            BombIsPlanted = false;
            BombDefused = false;
            CTscore++;
        }
    }

    public void TerroristWins()
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
        BombDefused = false;
    }

    public async Task ChooseTandPlantBomb()
    {
        var randomPlayer = PickRandomPlayer(Terrorists);
        if (randomPlayer != -1)
        {
            Terrorists[randomPlayer].PlantBomb();
            BombIsPlanted = true;
            ChooseSiteBoth();
        }
    }

    public async Task StartOrResetRounds()
    {
        foreach (var ct in CounterTerrorists)
        {
            if (ct.isDead)
            {
                ct.isDead = false;
                ct.Defusekit = false;
                ct.Weapon = ct.CTweps[0];
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
            if (t.isDead)
            {
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
        BombTimer = 30;

        Console.ForegroundColor = ConsoleColor.White;
        Round++;
        Console.WriteLine($"Runde: {Round}");
        Console.WriteLine($"CT: {CTscore}");
        Console.WriteLine($" T: {Tscore}");
    }

    public void PrintPlayerInfo()
    {
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var ct in CounterTerrorists)
        {
            Console.WriteLine($"{ct.Name} - {ct.Health} - {ct.chosenSite} - {ct.Money} ");
        }

        foreach (var t in Terrorists)
        {
            Console.WriteLine($"{t.Name} - {t.Health} {t.chosenSite} - {t.Money} ");
        }

        if (BombIsPlanted) Console.WriteLine($"Bomb timer: {BombTimer}");
    }

    public void CountDown()
    {
        BombTimer--;
    }

    // public async Task AfterPlantWinCheck()
    // {
    //     if (BombDefused)
    //     {
    //         Console.WriteLine($"Counter-Terrorists win!");
    //         CTscore++;
    //         foreach (var ct in CounterTerrorists)
    //         {
    //             ct.Money += 3500;
    //         }
    //         foreach (var t in Terrorists)
    //         {
    //             t.Money += 2000;
    //         }
    //         RoundEnded = true;
    //         BombIsPlanted = false;
    //     }
    //     // T = 0, CT = 1
    //     var totalDeaths = CountDownDeathCheck();
    //     var randomPlayer = PickRandomPlayer(CounterTerrorists);
    //     if (totalDeaths[0] == 5 && randomPlayer != -1)   CounterTerrorists[randomPlayer].DefuseBomb();
    //     if (totalDeaths[1] == 5) BombTimer = 0;
    //     if (BombTimer <= 0)
    //     {
    //         Console.WriteLine($"Terrorists win!");
    //         Tscore++;
    //         foreach (var t in Terrorists)
    //         {
    //             t.Money += 3500;
    //         }
    //
    //         foreach (var ct in CounterTerrorists)
    //         {
    //             ct.Money += 2000;
    //         }
    //
    //         RoundEnded = true;
    //         BombIsPlanted = false;
    //     }
    // }

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
    }

    public void ChooseTarget(char tSite)
    {
        // FINDING TEAM INDEX
        var teamAliveList = CounterTerrorists.FindAll(x => x.isDead == false).ToList();
        var teamOnSiteList = teamAliveList.FindAll(x => x.chosenSite == tSite).ToList();

        if (teamAliveList.Count <= 0 || teamOnSiteList.Count <= 0) return;

        var teamRandomIndex = random.Next(0, teamOnSiteList.Count);
        var teamIndex = CounterTerrorists.FindIndex(x => x.Name == teamOnSiteList[teamRandomIndex].Name);

        // FINDING ENEMY INDEX
        var enemyAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();

        if (enemyAliveList.Count <= 0) return;

        var enemyRandomAliveIndex = random.Next(0, enemyAliveList.Count);
        var enemyIndex = Terrorists.FindIndex(x => x.Name == enemyAliveList[enemyRandomAliveIndex].Name);

        CounterTerrorists[teamIndex].Shoot(Terrorists[enemyIndex]);
        Terrorists[enemyIndex].Shoot(CounterTerrorists[teamIndex]);
    }

    public void ChooseTarget()
    {
        // FINDING TEAM INDEX
        var teamAliveList = Terrorists.FindAll(x => x.isDead == false).ToList();

        if (teamAliveList.Count <= 0) return;

        var teamRandomAliveIndex = random.Next(0, teamAliveList.Count);
        var teamIndex = Terrorists.FindIndex(x => x.Name == teamAliveList[teamRandomAliveIndex].Name);

        // FINDING ENEMY INDEX
        var enemyAliveList = CounterTerrorists.FindAll(x => x.isDead == false).ToList();
        var enemyOnSiteList = enemyAliveList.FindAll(x => x.chosenSite == Terrorists[0].chosenSite).ToList();

        if (enemyAliveList.Count <= 0 || enemyOnSiteList.Count <= 0) return;

        var enemyRandomIndex = random.Next(0, enemyOnSiteList.Count);
        var enemyIndex = CounterTerrorists.FindIndex(x => x.Name == enemyOnSiteList[enemyRandomIndex].Name);

        Terrorists[teamIndex].Shoot(CounterTerrorists[enemyIndex]);
        CounterTerrorists[enemyIndex].Shoot(Terrorists[teamIndex]);
    }

    public int[] CountDownDeathCheck() // This one is used while Countdown is running
    {
        var totalDead = new int[2];
        foreach (var unused in Terrorists.Where(t => t.isDead))
        {
            totalDead[0]++;
        }

        foreach (var unused in CounterTerrorists.Where(ct => ct.isDead))
        {
            totalDead[1]++;
        }

        return totalDead;
    }

    public bool CheckSiteDeaths() // Is used to check before bomb is planted
    {
        var isAlive = true;
        var ctAsiteDead = 0;
        var ctBsiteDead = 0;

        foreach (var ct in CounterTerrorists)
        {
            if (ct.chosenSite == 'A' && ct.isDead) ctAsiteDead++;
            if (ct.chosenSite == 'B' && ct.isDead) ctBsiteDead++;
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

    public void ChooseSiteBoth()
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
    public void
        EconomyCheck(
            string team) // MÅ NOK STÅ I MATCH FOR Å KUNNE GÅ IGJENNOM ØKONOMI FOR HELE LAGET OG SENDE VIDERE TIL CHECKTEAMECO I HVERT ENKELT LAG
    {
        var has2K = 0;
        var has3K = 0;
        switch (team)
        {
            case "CT":
            {
                foreach (var ct in CounterTerrorists)
                {
                    if (ct.CheckTeamEco(3000))
                    {
                        has3K++;
                    }
                    else if (ct.CheckTeamEco(2000))
                    {
                        has2K++;
                    }
                }

                foreach (var ct in CounterTerrorists)
                {
                    ct.CheckPlayerEco(has2K, has3K);
                }

                break;
            }
            case "T":
            {
                foreach (var t in Terrorists)
                {
                    if (t.CheckTeamEco(3000))
                    {
                        has3K++;
                    }
                    else if (t.CheckTeamEco(2000))
                    {
                        has2K++;
                    }
                }

                foreach (var t in Terrorists)
                {
                    t.CheckPlayerEco(has2K, has3K);
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