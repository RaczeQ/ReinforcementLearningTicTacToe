using Game.Exceptions;

namespace Game.Objects
{
    public class Cell
    {
        public enum CellState { Empty = -1, X = 0, O = 1 }
        public CellState State { get; private set; } = CellState.Empty;

        public Cell() :
            this(CellState.Empty)
        { }

        public Cell(CellState state)
        {
            State = state;
        }

        public void SetState(CellState newState)
        {
            if (newState == CellState.Empty)
            {
                throw new EmptyGameStateException();
            }

            if (State != CellState.Empty)
            {
                throw new CellAlreadyOccupiedException();
            }

            State = newState;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Cell c = (Cell)obj;
            return (this.State == c.State);
        }

        public override string ToString()
        {
            switch (State)
            {
                case CellState.X:
                    return "X";
                case CellState.O:
                    return "O";
                case CellState.Empty:
                default:
                    return " ";
            }
        }
    }
}
