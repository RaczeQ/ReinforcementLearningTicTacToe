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
        public Board State { get; set; }
       // public int State { get; set; }
         public double?[] QValue { get; set; } = new double?[Board.DEFAULT_SIZE * Board.DEFAULT_SIZE];

        //  public double?[] QValue { get; set; } = 
        //Enumerable.Repeat(null, Board.DEFAULT_SIZE * Board.DEFAULT_SIZE).ToArray();
    }
}
