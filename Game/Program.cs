using Game.Objects;
using System;
using static Game.Players.PlayerManager;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe.RunGame(PlayerType.Player, PlayerType.Random, Board.DEFAULT_SIZE);
            Console.ReadLine();
        }
    }
}
