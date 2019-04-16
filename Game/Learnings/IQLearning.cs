using Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    interface IQLearning
    {

        Tuple<int, int> MakeMove(Board board);
        //  void LearnQFunction(Board board);

        //  double Reward(Board board);

        void Train();
    }
}
