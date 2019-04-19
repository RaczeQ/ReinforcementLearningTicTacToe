using Game.Learnings;
using Game.Objects;
using NLog;
using System;
using Game.ConsoleUtils;
using System.Collections.Generic;
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
        public static void AnalyzeQLearningParameters()
        {
            var learning_rate = new List<double> { 0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95 };
            var discount_factor = new List<double> { 0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95 };
            var default_value = new List<double> { 0.1, 0.2, 0.3, 0.4, .5, 0.6, 0.7, 0.8, 0.9 };

            foreach (var dv in default_value)
            {
                foreach (var l in learning_rate)
                {
                    foreach (var d in discount_factor)
                    {
                        QLearning.LEARNING_RATE = l;
                        QLearning.DISCOUNT_FACTOR = d;
                        QFunction.DEAFULT_VALUE = dv;
                        TrainQFunction();
                        LoadQFunction();
                        TicTacToe.RunGame(PlayerType.QLearning, PlayerType.Random, Board.DEFAULT_SIZE, BATTLE_NUM);
                        TicTacToe.RunGame(PlayerType.Random, PlayerType.QLearning, Board.DEFAULT_SIZE, BATTLE_NUM);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // Debug - Shows every player move
            // Info - Shows winner for each game with finished board state
            // Warn - just simulation result
            NLogConfigurator.Configure(LogLevel.Warn);

            AnalyzeQLearningParameters();
            Console.ReadLine();
        }
    }
}
