using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public class PointsGoal : Goal
    {
        private readonly int _pointsRequired;
        public PointsGoal(int pointsRequired, int round)
        {
            this._pointsRequired = pointsRequired;
            this.round = round;
        }

        public override bool IsGoalFulfilled(Game game)
        {
            throw new NotFiniteNumberException();
        }

        public override string GetDescription()
        {
            return $"You need to have at least {_pointsRequired} Points to continue to the next round";
        }
    }
}
