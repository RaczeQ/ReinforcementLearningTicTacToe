using Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    public class MoveStates
    {       
        public Board GameState { get; set; }
        public double[] QValue { get; set; } = Enumerable.Repeat(0.6, Board.DEFAULT_SIZE * Board.DEFAULT_SIZE).ToArray();
    }
}
