using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Objects;
using static Game.Objects.Board;

namespace Game.Learnings
{
    public class QLearning : IQLearning
    {

        public static readonly double LEARNING_RATE = 0.9;
        public static readonly double DISCOUNT_FACTOR = 0.95;

        static List<MoveStates> Qtable = new List<MoveStates>(Board.DEFAULT_SIZE * Board.DEFAULT_SIZE);

        public void LearnQFunction(Board board)
        {


        }

        double CountReward(Board board, MoveStates moveStates, double currentQValue)
        {
            if (board.GetWinner().HasValue)
                return Reward(board);
            else
                return currentQValue + LEARNING_RATE * (DISCOUNT_FACTOR * (moveStates.QValue.Max() - currentQValue)); 
        }



        public int Reward(Board board)
        {
            if (board.GetWinner().HasValue && board.GetWinner() == (Player)board.CurrentPlayer)
                return 1;
            else if (board.GetWinner().HasValue && board.GetWinner() == (Player)(1 - board.CurrentPlayer))
                return -1;
            else
                return 0;
        }
    }
}
