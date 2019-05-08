using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Objects;
using Game.Results;
using NLog;

namespace Game.Learnings.SL
{
    public static class DatasetGenerator
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly Random R = new Random();

        private static readonly int PLAYER_VALUE = 1;
        private static readonly int EMPTY_VALUE = 0;
        private static readonly int ENEMY_VALUE = -1;

        private static readonly double PROBABILITY_CHANCE_STEP = 0.04;

        public static IList<BoardStateItem> GenerateDataset(int observationsNumber, bool transformObservation = false)
        {
            var result = new List<BoardStateItem>();
            for (int i = 0; i < observationsNumber; i++)
            {
                double probabilityStep = 0.04 + R.NextDouble() * 0.06;
                double baseProbability = probabilityStep;
                bool generated = false;

                var startingPlayer = (Board.Player)R.Next(2);
                var b = new Board(startingPlayer);

                while (!generated)
                {
                    var currentPlayer = b.GetCurrentPlayer();
                    var moves = b.GetAvailableMoves();
                    if (moves.Any())
                    {
                        var nextMove = moves.ElementAt(R.Next(moves.Count));
                        b.MakeMove(nextMove);
                    }

                    if (R.NextDouble() < baseProbability || !moves.Any())
                    {
                        var gameResult = GetGameResult(b.GetBoardCopy(), currentPlayer);
                        result.Add(GetObservationFromBoardState(b, gameResult, currentPlayer));
                        if (transformObservation)
                        {
                            var transformedStates = b.GetBoardTransformations();

                            foreach (var state in transformedStates)
                            {
                                result.Add(GetObservationFromBoardState(state, gameResult, currentPlayer));
                            }
                        }
                        generated = true;
                    }
                    else
                    {
                        baseProbability += probabilityStep;
                    }
                }
            }
            return result;
        }

        public static void SaveDataset(IList<BoardStateItem> dataset)
        {
            Writer.SaveSupervisedLearningDataset(dataset);
        } 

        private static GameResult GetGameResult(Board b, Board.Player consideredPlayer)
        {
            while (b.GetGameState().Item1 == Board.GameState.InProgress)
            {
                var moves = b.GetAvailableMoves();
                var nextMove = moves.ElementAt(R.Next(moves.Count));
                b.MakeMove(nextMove);
            }

            var (gameState, winner) = b.GetGameState();
            if (gameState == Board.GameState.Tie)
            {
                return GameResult.Tie;
            }
            else
            {
                return winner == consideredPlayer ? GameResult.Win : GameResult.Loss;
            }
        }

        public static int[] GetParsedCells(IList<Cell> cellsList, Board.Player consideredPlayer)
        {
            var result = new int[cellsList.Count];
            for (int i = 0; i < cellsList.Count; i++)
            {
                var cell = cellsList[i];
                if (cell.State == Cell.CellState.Empty)
                {
                    result[i] = EMPTY_VALUE;
                }
                else
                {
                    result[i] =
                        cell.State == (Cell.CellState)consideredPlayer ? PLAYER_VALUE : ENEMY_VALUE;
                }
            }
            return result;
        }
        private static BoardStateItem GetObservationFromBoardState(Board b, GameResult gameResult, Board.Player consideredPlayer)
        {
            var result = new BoardStateItem();
            result.GameResult = gameResult;
            var cellsList = b.Cells.Cast<Cell>().ToList();
            result.BoardFields = GetParsedCells(cellsList, consideredPlayer);
            return result;
        }
    }
}
