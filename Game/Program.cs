using Game.Learnings;
using Game.Objects;
using NLog;
using System;
using static Game.Players.PlayerManager;

namespace Game
{
    class Program
    {
        public static readonly int BATTLE_NUM = 1;

        public static void LearnAgent()
        {
            QLearning qLearning = new QLearning();
            qLearning.Train();
        }

        static void Main(string[] args)
        {
            // Debug - Shows every player move
            // Info - Shows winner for each game with finished board state
            // Warn - just simulation result
            NLogConfigurator.Configure(LogLevel.Warn);

            LearnAgent();
            //TicTacToe.RunGame(PlayerType.QLearning, PlayerType.Random, Board.DEFAULT_SIZE, BATTLE_NUM);
            Console.ReadLine();
        }
    }
}
