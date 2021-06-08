using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public class WinCondition
    {
        private WinConditionDelegate f;

        public WinCondition(WinConditionDelegate cond)
        {
            this.f = cond;
        }

        public bool Check(Game game)
        {
            return f(game);
        }

        public delegate bool WinConditionDelegate(Game game);
    }
}
