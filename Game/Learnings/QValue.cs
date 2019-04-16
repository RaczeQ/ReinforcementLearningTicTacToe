using Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    public class QValue
    {
        public int State { get; set; }
        public double?[] Actions { get; set; } = new double?[Board.DEFAULT_SIZE * Board.DEFAULT_SIZE];
    }
}
