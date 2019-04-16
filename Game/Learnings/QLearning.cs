using System;
using System.Collections.Generic;
using System.Linq;
using Game.Exceptions;
using Game.Learnings;
using Game.Objects;
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

        

        public void Train()
        {
            QFunction.GenerateStates();
            var temp = QFunction.QTable;
            var t = 5;
        }


        public Tuple<int, int> MakeMove(Board board)
        {
            throw new NotImplementedException();
        }


        public Tuple<int, int> LearnQFunction(Board board)
        {
            return Learn(board);  
        }

        Tuple<int, int> Learn(Board board)
        {
            return null;
        }

        //void InitializeQtable(QValue moveStates, IList<Tuple<int, int>> availableMoves)
        //{
        //    foreach(var move in availableMoves)
        //    {
        //        moveStates.Actions[GetIndexOfQValue(move)] = 0.6;
        //    }
        //}
        //void UpdateQTable(QValue moveStates)
        //{
        //    for( int i =0; i<moveStates.Actions.Length; i++)
        //    {
        //        if(moveStates.QValue[i].HasValue)
        //            moveStates.Actions[i] = Reward(moveStates.State);
        //    }         
        //}

       
        Tuple<int, int> GetMoveCoordinatesFromQValue(int index)
        {
            int y = (int) Math.Floor((double)(index / Board.DEFAULT_SIZE));
            int x = index - (y * Board.DEFAULT_SIZE);
            return new Tuple<int, int>(x, y);
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
                return TIE_REWARD_VALUE;
            }
            else
            {
                return state.Item2 == board.GetCurrentPlayer() ? WIN_REWARD_VALUE : LOSS_REWARD_VALUE;
            }
        }

      
    }
}
