using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Exceptions;
using Game.Learnings;
using Game.Objects;
using Game.Results;
using MathNet.Numerics.LinearAlgebra;
using static Game.Objects.Board;

namespace Game.Learnings
{
    public class QLearning : ILearnQFunction, IUseQFunction
    {

        public static readonly double LEARNING_RATE = 0.9;
        public static readonly double DISCOUNT_FACTOR = 0.95;

        private static readonly double WIN_REWARD_VALUE = 1.0;
        private static readonly double TIE_REWARD_VALUE = 0.5;
        private static readonly double LOSS_REWARD_VALUE = 0.0;

        private static readonly int EPISODES_NUM =1000000;

        public static Matrix<double> QFunctionMatrix { get; set; }

        public void LearnQFunction()
        {
            QFunction.GenerateTabularQFunction();
            
            for (int i=0; i<EPISODES_NUM; i++)
            {
                var board = new Board();
                LearnFromMoves(board);
                Console.WriteLine("Iteration {0} finished", i);
            }

            SaveQFunction();
        }


        private double? LearnFromMoves(Board board)
        {
            if(!(board.GetGameState().Item1 == Board.GameState.InProgress))
            {
                return GetReward(board);
            }
            else
            {      
                var currentBoard = board.GetBoardCopy();

                var currentStateQValue = QFunction.Table[currentBoard.GetHashCode()];
                var index = Array.IndexOf(currentStateQValue, currentStateQValue.Where(x => x.HasValue).OrderBy(y => Guid.NewGuid()).FirstOrDefault());
                //currentStateQValue.Where(x=> x.HasValue)
                board.MakeMove(GetMoveCoordinatesFromQValue(index));
                var nextMoveReward = LearnFromMoves(board);

                QFunction.Table[currentBoard.GetHashCode()][index] = currentStateQValue[index] + LEARNING_RATE
                    * (DISCOUNT_FACTOR * (nextMoveReward) - currentStateQValue[index]);

                return QFunction.Table[currentBoard.GetHashCode()][index];
            }
        }

        private Tuple<int, int> GetMoveCoordinatesFromQValue(int index)
        {
            int x = (int)Math.Floor((double)(index / Board.DEFAULT_SIZE));
            int y = index - (x * Board.DEFAULT_SIZE);
            return new Tuple<int, int>(x, y);
        }



        private double GetReward(Board board)
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


      
        public void SaveQFunction()
        {
            Writer.SaveResult(QFunction.Table, ResultResources.Q_FUNCTION_FILE);
        }

        public Tuple<int, int> GetMoveFromQFunction(Board board)
        {
            var moves = QFunction.Table[board.GetHashCode()];
            return GetMoveCoordinatesFromQValue(moves.ToList().IndexOf(moves.Max()));
        }

        public void LoadQFunction()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/Results";
            var file = String.Format(path + "/{0}.txt", ResultResources.Q_FUNCTION_FILE);

            if (!File.Exists(file))
                throw new QFunctionFileDoesNotExist();

            QFunction.Table = new Dictionary<int, double?[]>();
            using (TextReader sr = new StreamReader(file))              
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    var lineValues = line.Split(';');
                    var key = Int32.Parse(lineValues[0].ToString());
                    double parsedValue;
                    var values = lineValues.Skip(1).Select(x => double.TryParse(x, out parsedValue) ? parsedValue : (double?)null).ToArray();

                    QFunction.Table.Add(key, values);
                }
            }        
        }
    }
}
