using Game.Learnings;
using Game.Objects;
using Game.Players;
using Game.Results;
using NLog;
using System;
using System.Collections.Generic;

namespace Game
{
    public static class TicTacToe
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void RunGame(PlayerManager.PlayerType player1, PlayerManager.PlayerType player2, int boardSize, int battleNum)
        {
            var wins = new Dictionary<Board.Player, int> { { Board.Player.X, 0 }, { Board.Player.O, 0 } };
            var ties = 0;
            for (int i = 0; i < battleNum; i++)
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

                if (state.Item1 == Board.GameState.Tie)
                {
                    _logger.Info($"Tie!");
                    ties++;
                }
                else
                {
                    _logger.Info($"Won player {state.Item2}");
                    wins[state.Item2.Value]++;
                    Writer.SaveResult(state.Item2.ToString(), String.Format("{0}_{1}_{2}_{3}_{4}", player1, player2, battleNum, QLearning.LEARNING_RATE, QLearning.DISCOUNT_FACTOR));
                }
            }
            _logger.Warn($"X: {wins[Board.Player.X]}, O: {wins[Board.Player.O]}, Ties: {ties}");
        }
    }
}
