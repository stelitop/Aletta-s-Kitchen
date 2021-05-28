using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public abstract class Goal
    {
        public int round;
       
        public abstract bool IsGoalFulfilled(Game game);
        public abstract string GetDescription(Game game);
    }   
}
