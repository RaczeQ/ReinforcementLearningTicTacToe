using Game.Objects;
using System;
using static Game.Players.PlayerManager;

namespace Game
{
    class Program
    {
        public static readonly int BATTLE_NUM = 100;
        static void Main(string[] args)
        {
            TicTacToe.RunGame(PlayerType.QLearning, PlayerType.Random, Board.DEFAULT_SIZE, BATTLE_NUM);
            Console.ReadLine();
        }
    }
}
