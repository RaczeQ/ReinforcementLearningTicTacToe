using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Learnings.SL;
using Game.Objects;
using Microsoft.ML;
using NLog;
using ReinforcementLearningTicTacToeML.Model.DataModels;

namespace Game.Players
{
    class MLPlayer : IPlayer
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static PredictionEngine<ModelInput, ModelOutput> predEngine;

        public MLPlayer()
        {
            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load("MLModel.zip", out var modelInputSchema);
            predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        public Tuple<int, int> GetMove(Board board)
        {
            // Use the code below to add input data
            var input = new ModelInput();
            var currentPlayer = board.GetCurrentPlayer();
            var availableMoves = board.GetAvailableMoves();
            float bestScore = -1;
            Tuple<int, int> bestMove = null;
            foreach (var move in availableMoves)
            {
                var b_copy = board.GetBoardCopy();
                b_copy.MakeMove(move);
                var cells = board.Cells.Cast<Cell>().ToList();
                var parsedCells = DatasetGenerator.GetParsedCells(cells, currentPlayer);
                input.X0 = parsedCells[0];
                input.X1 = parsedCells[1];
                input.X2 = parsedCells[2];
                input.X3 = parsedCells[3];
                input.X4 = parsedCells[4];
                input.X5 = parsedCells[5];
                input.X6 = parsedCells[6];
                input.X7 = parsedCells[7];
                input.X8 = parsedCells[8];

                // Try model on sample data
                ModelOutput result = predEngine.Predict(input);

                if (bestMove == null || bestScore < result.Score)
                {
                    bestMove = move;
                    bestScore = result.Score;
                }
            }

            _logger.Debug(bestMove);
            _logger.Debug(bestScore);


            return bestMove;
        }
    }
}
