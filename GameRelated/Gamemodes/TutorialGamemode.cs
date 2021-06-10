using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.GoalTypes;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using DSharpPlus.Entities;
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

            public override async Task ApplyGamemodeSettings(Game game)
            {
                game.goalGenerator = new SquareIncrGoalGenerator();
                BotHandler.SetUserState(game.playerId, UserState.Tutorial);

                game.gameState = GameState.Tutorial;
                
                //game.winCondition = new WinCondition(x => x.player.curPoints >= 75);

                //game.pool.ingredients.RemoveAll(x => x.effects.FindAll(y => y.Type.Contains(EffectType.OnEnteringKitchen)).Count > 0);

                await game.player.kitchen.Restart(game);

                game.player.hand.Clear();

                game.player.kitchen.ReplaceIngredient(0, game.pool.GetVanillaIngredient("Manatov Cocktail"));
                game.player.kitchen.ReplaceIngredient(1, game.pool.GetVanillaIngredient("Horse Radish"));
                game.player.kitchen.ReplaceIngredient(2, game.pool.GetVanillaIngredient("Hotpot Hydra"));
                game.player.kitchen.ReplaceIngredient(3, game.pool.GetVanillaIngredient("Monster Platter"));
                game.player.kitchen.ReplaceIngredient(4, game.pool.GetVanillaIngredient("Pup Peas In A Pod"));

                game.player.kitchen.nextOption = game.pool.GetVanillaIngredient("Soulstraw Sucker");

                await game.UIMessage.CreateReactionAsync(DiscordEmoji.FromName(BotHandler.bot.Client, ":arrow_left:"));
                await game.UIMessage.CreateReactionAsync(DiscordEmoji.FromName(BotHandler.bot.Client, ":arrow_right:"));
            }
        }
    }
}
