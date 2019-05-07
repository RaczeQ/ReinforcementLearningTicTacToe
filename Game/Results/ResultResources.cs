using Game.Learnings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Learnings.RL;

namespace Game.Results
{
    public class ResultResources
    {
        public static readonly string Q_FUNCTION_FILE = String.Format("QFunction_{0}_{1}", QLearning.LEARNING_RATE, QLearning.DISCOUNT_FACTOR);
        public static readonly string Q_FUNCTION_RESULT_FILE = "QFunctionResults";
        public static readonly string DATASET_FILE = "SR_Dataset";
    }
}
