//namespace CSGO_v2;

//public class GameEngine
//{
//    private Match match = new Match(16, 30);




//    public async Task StartRound()
//    {
//        while (!match.GameEnded)
//        {
//            match.Round++;
//            Console.WriteLine($"Runde: {match.Round}");
//            match.EconomyCheck("CT");
//            match.EconomyCheck("T");
//            await match.chooseSiteBoth();
//            await Task.Delay(100);
//            await match.Fight();
//            await Task.Delay(1000);
//        }
//        while (match.BombIsPlanted)
//        {

//        }
//    }
//}