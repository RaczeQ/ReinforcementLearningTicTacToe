using Game.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Objects
{
    public class Board
    {
        public static readonly int DEFAULT_SIZE = 3;
        public static readonly int MAX_BOARD_SIZE = 5;
        public enum Player { X = 0, O = 1 }
        public enum GameState { InProgress = 0, Tie = 1, Finished = 2 }

        public int CurrentPlayer { get; private set; } = 0;
        private int Size { get; set; }
        private Cell[,] Cells { get; set; }

        public Board() : this(DEFAULT_SIZE) { }

        public Board(int size)
        {
            if (size > MAX_BOARD_SIZE)
            {
                Console.WriteLine($"Warning! Setting board size to {MAX_BOARD_SIZE}.");
                size = MAX_BOARD_SIZE;
            }

            Size = size;
            Cells = new Cell[Size, Size];
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Cells[r, c] = new Cell();
                }
            }
        }

        public Board(Board b)
        {
            this.Size = b.Size;
            this.CurrentPlayer = b.CurrentPlayer;
            Cells = new Cell[Size, Size];
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Cells[r, c] = new Cell(b.Cells[r, c].State);
                }
            }
        }

        public Board GetBoardCopy()
        { 
            return new Board(this);
        }

        public Player GetCurrentPlayer()
        {
            return (Player)CurrentPlayer;
        }

        public IList<Tuple<int, int>> GetAvailableMoves()
        {
            var result = new List<Tuple<int, int>>();
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (Cells[r, c].State == Cell.CellState.Empty)
                    {
                        result.Add(new Tuple<int, int>(r, c));
                    }
                }
            }
            return result;
        }

        private void ChangePlayer()
        {
            CurrentPlayer = 1 - CurrentPlayer;
        }

        public void MakeMove(Tuple<int, int> move)
        {
            MakeMove(move.Item1, move.Item2);
        }

        public void MakeMove(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                throw new CoordinatesOutOfRangeException();
            }
            Cells[row, col].SetState((Cell.CellState) CurrentPlayer);
            ChangePlayer();
        }

        public Tuple<GameState, Player?> GetGameState()
        {
            Tuple<GameState, Player?> result;
            var winner = GetWinner();
            if (winner != null)
            {
                result = new Tuple<GameState, Player?>(GameState.Finished, winner);
            }
            else if (Cells.Cast<Cell>().All(c => c.State != Cell.CellState.Empty))
            {
                result = new Tuple<GameState, Player?>(GameState.Tie, null);
            }
            else
            {
                result = new Tuple<GameState, Player?>(GameState.InProgress, null);
            }
            return result;
        }

        private Player? GetWinner()
        {
            // Check columns
            for (var col = 0; col < Size; col++)
            {
                if (AllFieldsTheSame(0, col, 1, 0))
                    return (Player) Cells[0, col].State;
            }

            // Check rows
            for (var row = 0; row < Size; row++)
                if (AllFieldsTheSame(row, 0, 0, 1))
                    return (Player) Cells[row, 0].State;

            // Check diagonals
            if (AllFieldsTheSame(0, 0, 1, 1))
                return (Player) Cells[0, 0].State;

            if (AllFieldsTheSame(Size - 1, 0, -1, 1))
                return (Player) Cells[Size - 1, 0].State;

            return null;
        }

        private bool AllFieldsTheSame(int startRow, int startColumn, int dy, int dx)
        {
            var firstCell = Cells[startRow, startColumn];
            if (firstCell.State == Cell.CellState.Empty)
            {
                return false;
            }

            for (var i = 0; i < Size; i++)
            {
                int r = startRow + dy * i;
                int c = startColumn + dx * i;
                if (!Cells[r,c].Equals(firstCell))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            string result = "";
            for (int r = 0; r < Size; r++)
            {
                var tempRow = new List<string>();
                for (int c = 0; c < Size; c++)
                {
                    tempRow.Add(Cells[r, c].ToString());
                }
                result += string.Join("|", tempRow);
                result += "\n";
                if (r < Size - 1)
                {
                    result += string.Join("+", Enumerable.Repeat('-', Size));
                    result += "\n";
                }
            }
            return result;
        }
    }
}
