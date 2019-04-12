using Game.Objects;
using System;

namespace Game.Players
{
    public interface IPlayer
    {
        Tuple<int, int> GetMove(Board board);
    }
}
