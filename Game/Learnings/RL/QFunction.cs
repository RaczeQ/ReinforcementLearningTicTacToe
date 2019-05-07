﻿using Game.Objects;
using Game.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings.RL
{
    public class QFunction
    {
        public static double DEAFULT_VALUE = 0.1;
        public static Dictionary<int, double?[]> Table {get; set;} = new Dictionary<int, double?[]>();
        public static int counter = 0;

        public static void GenerateTabularQFunction()
        {
            for (int i = 0; i < 2; i++)
            {
                var b = new Board((Board.Player)i);

                if (!Table.ContainsKey(b.GetHashCode()))
                    Table.Add(b.GetHashCode(), InitializeActions(b.GetAvailableMoves()));

                ExploreStates(b);
            }
        }

        public static void ExploreStates(Board board)
        {   
            if (board.GetGameState().Item1 != Board.GameState.Finished)
            {
                foreach (var move in board.GetAvailableMoves())
                {
                    var copy = board.GetBoardCopy();
                    copy.MakeMove(move);
                    var hash = copy.GetHashCode();
                    
                    if (!Table.ContainsKey(hash))
                    {
                        Table.Add(hash, InitializeActions(copy.GetAvailableMoves()));
                    };
                    
                    ExploreStates(copy);
                }
            }
        }

        static int GetIndexOfQValue(Tuple<int, int> move)
        {
            return move.Item2 + (move.Item1 * Board.DEFAULT_SIZE);
        }

        static double?[] InitializeActions(IList<Tuple<int, int>> moves)
        {
            var actions = new double?[Board.DEFAULT_SIZE * Board.DEFAULT_SIZE];
            foreach(var m in moves)
            {
                var index = GetIndexOfQValue(m);
                actions[index] = DEAFULT_VALUE;
            }
            return actions;
        }
    }
}