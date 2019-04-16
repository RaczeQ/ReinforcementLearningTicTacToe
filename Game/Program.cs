using Game.Objects;
using NLog;
using System;
using static Game.Players.PlayerManager;

namespace Game
{
    class Program
    {
        public static readonly int BATTLE_NUM = 100;
        static void Main(string[] args)
        {
            NLogConfigurator.Configure(LogLevel.Info);
            //TicTacToe.RunGame(PlayerType.QLearning, PlayerType.Random, Board.DEFAULT_SIZE, BATTLE_NUM);
            TicTacToe.RunGame(PlayerType.Player, PlayerType.Random, Board.DEFAULT_SIZE, 1);
            Console.ReadLine();
        }
    }
}
