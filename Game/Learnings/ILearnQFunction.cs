using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Learnings
{
    interface ILearnQFunction
    {
        void LearnQFunction(double? learning_rate = null, double? discount_rate=null);
        void LoadQFunction();
        void SaveQFunction();
    }
}
