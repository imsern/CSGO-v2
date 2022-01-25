using System;
using System.Threading.Tasks;
using CSGO_v2;

var match = new Match(16, 30);

match.Startmatch();
var txt = Console.ReadLine().ToLower();
if (txt == "start") await match.StartRound();