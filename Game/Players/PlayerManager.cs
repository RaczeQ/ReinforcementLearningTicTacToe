using Game.Exceptions;

namespace Game.Players
{
    public static class PlayerManager
    {
        public enum PlayerType { Player, Random, QLearning, MLPlayer }

        public static IPlayer GetPlayer(PlayerType type)
        {
            switch (type)
            {
                case PlayerType.Player:
                    return new ConsolePlayer();
                case PlayerType.Random:
                    return new RandomPlayer();
                case PlayerType.QLearning:
                    return new QLearningPlayer();
                case PlayerType.MLPlayer:
                    return new MLPlayer();
                default:
                    throw new NonExistentPlayerException();
            }
        }
    }
}
