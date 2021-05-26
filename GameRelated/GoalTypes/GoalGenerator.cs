using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public abstract class GoalGenerator
    {
        public abstract Goal CurrentGoal(Game game);
    }
}
