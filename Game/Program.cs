using Game.Learnings;
using Game.Objects;
using NLog;
using System;
using Game.ConsoleUtils;
using static Game.Players.PlayerManager;

namespace Game
{
    class Program
    {
        public static readonly int BATTLE_NUM = 1000;
        static ILearnQFunction qLearning = new QLearning();

        public static void TrainQFunction()
        {
            qLearning.LearnQFunction();
        }

        public static void LoadQFunction()
        {
            qLearning.LoadQFunction();
        }

        static void Main(string[] args)
        {
            // Debug - Shows every player move
            // Info - Shows winner for each game with finished board state
            // Warn - just simulation result
            NLogConfigurator.Configure(LogLevel.Warn);

            TrainQFunction();
            LoadQFunction();
            TicTacToe.RunGame(PlayerType.QLearning, PlayerType.Random, Board.DEFAULT_SIZE, BATTLE_NUM);
            Console.ReadLine();
        }
    }
}
