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
        public class TutorialGamemode : Gamemode
        {
            public TutorialGamemode()
            {
                this.title = "Tutorial";
                this.description = "Learn the rules of the game through this interactive tutorial!";
            }

            public override void ApplyGamemodeSettings(Game game)
            {
                game.goalGenerator = new SquareIncrGoalGenerator();
                BotHandler.SetUserState(game.playerId, UserState.Tutorial);

                game.gameState = GameState.None;

                game.winCondition = new WinCondition(x => false);
            }
        }
    }
}
