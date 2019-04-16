using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Exceptions;
using Game.Objects;
using static Game.Objects.Board;

namespace Game.Learnings
{
    public class QLearning : IQLearning
    {

        public static readonly double LEARNING_RATE = 0.9;
        public static readonly double DISCOUNT_FACTOR = 0.95;

        static List<MoveStates> Qtable = new List<MoveStates>(); //Board.DEFAULT_SIZE * Board.DEFAULT_SIZE

        public void LearnQFunction(Board board)
        {
            UpdateQtable(board);
           
        }

        void UpdateQtable(Board board)
        {
            foreach(var move in board.GetAvailableMoves())
            {
                var stateCopy = board;
                stateCopy.MakeMove(move);
                if (Qtable.Where(x => x.State == stateCopy).FirstOrDefault() == null)
                    Qtable.Add(new MoveStates { State = stateCopy });      
            }
        }

        double CountReward(Board board, MoveStates moveStates, double currentQValue)
        {
            var state = board.GetGameState();
            if (state.Item1 != GameState.InProgress)
                return Reward(board);
            else
                return currentQValue + LEARNING_RATE * (DISCOUNT_FACTOR * (moveStates.QValue.Max() - currentQValue)); 
        }

        public double Reward(Board board)
        {
            var state = board.GetGameState();
            if (state.Item1 == GameState.InProgress)
            {
                throw new QLearningRewardReturnException();
            }
            else if (state.Item1 == GameState.Tie)
            {
                return 0.5;
            }
            else
            {
                return state.Item2 == board.GetCurrentPlayer() ? 1 : 0;
            }
        }
    }
}
