using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public class SquareIncrGoalGenerator : GoalGenerator
    {
        public override Goal CurrentGoal(Game game)
        {
            int quotaNumber = (game.curRound-1)/5 + 1;

            int num = 3 * quotaNumber * quotaNumber;

            return new PointsGoal( num, quotaNumber*5 );       
        }
    }
}
