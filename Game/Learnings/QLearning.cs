using System;
using System.Collections.Generic;
using System.Linq;
using Game.Exceptions;
using Game.Learnings;
using Game.Objects;
using MathNet.Numerics.LinearAlgebra;
using static Game.Objects.Board;

namespace Game.Learnings
{
    public class QLearning : IQLearning
    {

        public static readonly double LEARNING_RATE = 0.9;
        public static readonly double DISCOUNT_FACTOR = 0.95;

        private static readonly double WIN_REWARD_VALUE = 1.0;
        private static readonly double TIE_REWARD_VALUE = 0.5;
        private static readonly double LOSS_REWARD_VALUE = 0.0;

        private static readonly int EPISODES_NUM =5;

        Random r = new Random();
        public static Matrix<double> QFunctionMatrix { get; set; }

        public void Train()
        {
            QFunction.GenerateTabularQFunction();
            
            for (int i=0; i<EPISODES_NUM; i++)
            {
                var board = new Board();
                LearnFromMoves(board);
            }
                
        }


        public double? LearnFromMoves(Board board)
        {
            if(!(board.GetGameState().Item1 == Board.GameState.InProgress))
            {
                return GetReward(board);
            }
            else
            {      
                var currentBoard = board.GetBoardCopy();
                var currentStateQValue = QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault();
                var index = currentStateQValue.Actions.ToList().IndexOf(currentStateQValue.Actions.Max());
                board.MakeMove(GetMoveCoordinatesFromQValue(index));
                var nextMoveReward  = LearnFromMoves(board);

                QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault().Actions[index]
                    = currentStateQValue.Actions[index] + LEARNING_RATE
                    * (DISCOUNT_FACTOR * (nextMoveReward) - currentStateQValue.Actions[index]);

                return QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault().Actions[index];
            }              
        }

        Tuple<int, int> GetMoveCoordinatesFromQValue(int index)
        {
            int x = (int)Math.Floor((double)(index / Board.DEFAULT_SIZE));
            int y = index - (x * Board.DEFAULT_SIZE);
            return new Tuple<int, int>(x, y);
        }

     

        public double GetReward(Board board)
        {
            var state = board.GetGameState();
            if (state.Item1 == GameState.InProgress)
            {
                throw new QLearningRewardReturnException();
            }
            else if (state.Item1 == GameState.Tie)
            {
                return TIE_REWARD_VALUE;
            }
            else
            {
                return state.Item2 == board.GetCurrentPlayer() ? WIN_REWARD_VALUE : LOSS_REWARD_VALUE;
            }
        }

      
    }
}
