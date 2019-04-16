using Game.Objects;
using Game.Players;
using System;

namespace Game
{
    public static class TicTacToe
    {
        public static void RunGame(PlayerManager.PlayerType player1, PlayerManager.PlayerType player2, int boardSize, int battleNum)
        {
            var b = new Board(boardSize);
            var players = new IPlayer[2] { PlayerManager.GetPlayer(player1), PlayerManager.GetPlayer(player2) };

            var state = b.GetGameState();
            while (state.Item1 == Board.GameState.InProgress)
            {
                Console.WriteLine(b);
                var move = players[b.CurrentPlayer].GetMove(b);
                b.MakeMove(move);
                state = b.GetGameState();
            }

            Console.WriteLine(b);

            if (state.Item1 == Board.GameState.Tie)
            {
                Console.WriteLine($"Tie!");
            }
            else
            {
                Console.WriteLine($"Won player {state.Item2}");
            }
        }
    }
}
