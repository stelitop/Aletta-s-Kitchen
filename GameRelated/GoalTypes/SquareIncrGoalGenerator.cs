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

            return new PointsGoal( 4*quotaNumber*quotaNumber, quotaNumber*5 );       
        }
    }
}
