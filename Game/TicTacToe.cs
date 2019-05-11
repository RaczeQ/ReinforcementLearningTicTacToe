using Game.Learnings;
using Game.Objects;
using Game.Players;
using Game.Results;
using NLog;
using System;
using System.Collections.Generic;
using Game.Learnings.RL;
using System.Threading.Tasks;

namespace Game
{
    public static class TicTacToe
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static object _lock = new object();
        private static object _lock_2 = new object();

        private static IDictionary<Board.Player, int> _wins = new Dictionary<Board.Player, int> { { Board.Player.X, 0 }, { Board.Player.O, 0 } };

        private static int _ties = 0;

        public static void RunGame(PlayerManager.PlayerType player1, PlayerManager.PlayerType player2, int boardSize, int battleNum, int? iter=null)
        {
            _wins = new Dictionary<Board.Player, int> { { Board.Player.X, 0 }, { Board.Player.O, 0 } };
            _ties = 0;

            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Parallel.For(0, battleNum, options, i =>
            {
                PlayerManager.PlayerType p1 = default(PlayerManager.PlayerType);
                PlayerManager.PlayerType p2 = default(PlayerManager.PlayerType);
                int bSize = 0;
                lock (_lock_2)
                {
                    p1 = player1;
                    p2 = player2;
                    bSize = boardSize;
                }
                RunSingleGame(p1, p2, bSize);
            });
            //for (int i = 0; i< battleNum; i++)
            //{
            //    RunSingleGame(player1, player2, boardSize);
            //}
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            

            _logger.Warn($"X: {_wins[Board.Player.X]}, O: {_wins[Board.Player.O]}, Ties: {_ties}");

            Writer.SaveQLearningResults(String.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}; {8}", "X wins", "O wins", "ties", "learning rate", "discount factor", "default value", "episode num", "X player", "Iter"),
                String.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}; {8}", _wins[Board.Player.X], _wins[Board.Player.O], _ties, QLearning.LEARNING_RATE, QLearning.DISCOUNT_FACTOR, QFunction.DEAFULT_VALUE, QLearning.EPISODES_NUM, player1, iter));
            
        }

        private static void RunSingleGame(PlayerManager.PlayerType player1, PlayerManager.PlayerType player2, int boardSize)
        {
            var b = new Board(boardSize);
            var players = new IPlayer[2] { PlayerManager.GetPlayer(player1), PlayerManager.GetPlayer(player2) };

            var state = b.GetGameState();
            while (state.Item1 == Board.GameState.InProgress)
            {
                _logger.Debug(b);
                var move = players[b.CurrentPlayer].GetMove(b);
                b.MakeMove(move);
                state = b.GetGameState();
            }

            _logger.Info(b);
            lock (_lock)
            {
                if (state.Item1 == Board.GameState.Tie)
                {
                    _logger.Info($"Tie!");
                    _ties++;
                }
                else
                {
                    _logger.Info($"Won player {state.Item2}");
                    _wins[state.Item2.Value]++;
                }

                if (i % (int)(battleNum / 10) == 0)
                {
                    _logger.Warn($"{i}/{battleNum}");
                }
            }
        }
    }
}
