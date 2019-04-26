using System;
using System.Linq;
using Game.Objects;

namespace Game.Players
{
    public class RandomPlayer : IPlayer
    {
        private static Random r = new Random();

        public Tuple<int, int> GetMove(Board board)
        {
            var moves = board.GetAvailableMoves();
            return moves.ElementAt(r.Next(moves.Count));
        }
    }
}
