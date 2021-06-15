using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.GoalTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.Gamemodes
{
    public partial class Gamemode
    {
        public class LimitedGamemode : Gamemode
        {
            const int AllowedTribes = 5;
            public LimitedGamemode()
            {
                this.title = "Limited";
                this.description = $"Play with a limited pool of ingredients, consisting only of {AllowedTribes} types.";
            }

            public override Task ApplyGamemodeSettings(Game game)
            {
                game.goalGenerator = new SquareIncrGoalGenerator();

                List<Tribe> tribes = new List<Tribe>();
                foreach (var tribe in Enum.GetValues(typeof(Tribe)))
                {
                    if ((Tribe)tribe == Tribe.NoTribe) continue;
                    tribes.Add((Tribe)tribe);
                }
                tribes = tribes.OrderBy(x => BotHandler.globalRandom.Next()).ToList();

                string feedbackMsg = "Ingredient types this game:";

                for (int i = 0; i < AllowedTribes && i < tribes.Count; i++)
                {
                    feedbackMsg += $" {tribes[i]},";
                }
                feedbackMsg = feedbackMsg.TrimEnd(',');
                feedbackMsg += ".";

                game.feedback.Add(feedbackMsg);

                for (int i = AllowedTribes; i < tribes.Count; i++)
                {
                    if (tribes[i] == Tribe.NoTribe) continue;

                    game.pool.ingredients.RemoveAll(x => x.tribe == tribes[i]);
                    game.pool.ingredients.RemoveAll(x => x.text.Contains(tribes[i].ToString()));
                    game.pool.ingredients.RemoveAll(x => x.text.Contains(tribes[i].ToString().ToLower()));
                }

                return Task.CompletedTask;
            }
        }
    }
}
