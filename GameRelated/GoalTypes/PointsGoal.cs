using Aletta_s_Kitchen.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.GoalTypes
{
    public class PointsGoal : Goal
    {
        private int _pointsRequired;
        public PointsGoal(int pointsRequired, int round)
        {
            this._pointsRequired = pointsRequired;
            this.round = round;
        }

        public override bool IsGoalFulfilled(Game game)
        {
            return game.player.curPoints >= this._pointsRequired;
        }

        public override string GetDescription(Game game)
        {
            string ret = $"You need to have at least {BotHandler.IntToEmojis(_pointsRequired)}p to continue to the next round.";

            if (this.IsGoalFulfilled(game)) ret += " *(done!)*";
            else ret += $" *({this._pointsRequired-game.player.curPoints}p needed)*";

            return ret;
        }
    }
}
