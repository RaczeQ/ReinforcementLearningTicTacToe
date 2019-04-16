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

            var t = QFunction.TabluarQFunction;
            var tt = 5;
                                           
        }

        //CHANGE TO MATRIX AND REFACTOR

        public double? LearnFromMoves(Board board)
        {
            if(board.GetGameState().Item1 == Board.GameState.Finished)
            {
                return GetReward(board);
            }
            else
            {
                var currentBoard = board.GetBoardCopy();
                var currentStateQValue = QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault();
                var index = currentStateQValue.Actions.ToList().IndexOf(currentStateQValue.Actions.Max());
                var t = GetMoveCoordinatesFromQValue(index);

                var temp = currentBoard.GetAvailableMoves();
                board.MakeMove(GetMoveCoordinatesFromQValue(index));
                var nextMoveReward  = LearnFromMoves(board);

                QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault().Actions[index]
                    = currentStateQValue.Actions[index] + LEARNING_RATE
                    * (DISCOUNT_FACTOR * (nextMoveReward) - currentStateQValue.Actions[index]);

                return QFunction.TabluarQFunction.Where(s => s.State == currentBoard.GetHashCode()).FirstOrDefault().Actions[index];
            }
               
        }


        //TO REFACTOR
        private void IniitializeQFunctionMatrix()
        { 
            QFunctionMatrix = Matrix<double>.Build.Dense(QFunction.TabluarQFunction.Count, (1 + Board.DEFAULT_SIZE * Board.DEFAULT_SIZE), -1);
            for (int i = 0; i < QFunction.TabluarQFunction.Count; i++)
            {
                QFunctionMatrix[i, 0] = QFunction.TabluarQFunction.ElementAt(i).State;
                for(int j=0; j<(1 + Board.DEFAULT_SIZE * Board.DEFAULT_SIZE); j++)
                {
                    QFunctionMatrix[i, j] = QFunction.TabluarQFunction.ElementAt(i).Actions[j - 1] ?? -1;
                }
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
