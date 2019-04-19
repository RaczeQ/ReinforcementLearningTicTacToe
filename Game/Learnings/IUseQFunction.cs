using Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    interface IUseQFunction
    {
        Tuple<int, int> GetMoveFromQFunction(Board board);
        
    }
}
