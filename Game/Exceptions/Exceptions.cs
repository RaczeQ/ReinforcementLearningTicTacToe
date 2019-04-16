namespace Game.Exceptions
{
    public class TicTacToeException : System.Exception
    {
        public TicTacToeException(string message):
            base(message)
        {
        }
    }

    public class CoordinatesOutOfRangeException : TicTacToeException
    {
        public CoordinatesOutOfRangeException()
            :base("Given coordinates are invalid!")
        {
        }
    }

    public class EmptyGameStateException : TicTacToeException
    {
        public EmptyGameStateException()
            : base("Cannot set a cell state to 'Empty'!")
        {
        }
    }

    public class CellAlreadyOccupiedException : TicTacToeException
    {
        public CellAlreadyOccupiedException()
            : base("Given cell is already occupied!")
        {
        }
    }

    public class NonExistentPlayerException : TicTacToeException
    {
        public NonExistentPlayerException()
            : base("Selected player doesn't exist!")
        {
        }
    }

    public class QLearningRewardReturnException : TicTacToeException
    {
        public QLearningRewardReturnException()
            : base("Cannot return reward for a game in progress!")
        {
        }
    }
}
