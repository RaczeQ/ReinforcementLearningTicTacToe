using Game.Learnings;
using Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Players
{
    class QLearningPlayer : IPlayer
    {
        IUseQFunction qLearning = new QLearning();
        Random r = new Random();

        public Tuple<int, int> GetMove(Board board)
        {
            var nextMove = qLearning.GetMoveFromQFunction(board);

            var moves = board.GetAvailableMoves();
            return nextMove;
           // moves.ElementAt(r.Next(moves.Count));
        }
    }
}
