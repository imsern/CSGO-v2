using System;
using System.Threading.Tasks;
using CSGO_v2;

var gameEngine = new GameEngine(16, 30);
await gameEngine.Run();