using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.GoalTypes;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Game
    {
        public int curRound;
        public IngredientPool pool;
        public Player player;        

        public GameState gameState;

        public List<string> feedback;
        public GoalGenerator goalGenerator;

        public PickingChoices pickingChoices;

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();
            this.gameState = GameState.None;

            this.feedback = new List<string>();

            this.pickingChoices = new PickingChoices();

            this.goalGenerator = null;
        }

        public async Task Start() => await this.Start(BotHandler.genericPool, new SquareIncrGoalGenerator());
        public async Task Start(IngredientPool pool, GoalGenerator goalGenerator)
        { 
            this.gameState = GameState.Loading;
            this.curRound = 1;            
            this.pool = new IngredientPool(pool);
            this.player = new Player();
            this.pickingChoices = new PickingChoices();

            this.goalGenerator = goalGenerator;

            await this.player.kitchen.Restart(this);
        }

        public async Task NextRound()
        {
            this.curRound++;

            int curIngrInShop = this.player.kitchen.Count;
            await this.player.kitchen.FillEmptySpots(this);

            for (int i=0; i<curIngrInShop; i++)
            {
                EffectArgs args = new EffectArgs(EffectType.Timer);
                await Effect.CallEffects(this.player.kitchen.OptionAt(i).effects, EffectType.Timer, this.player.kitchen.OptionAt(i), this, args);
            }

            this.gameState = GameState.PickFromKitchen;            

            if (this.goalGenerator.CurrentGoal(this).round == this.curRound)
            {
                this.gameState = GameState.BeforeQuota;                
            }
        }

        public void CheckQuota()
        {
            var curGoal = this.goalGenerator.CurrentGoal(this);

            if (!curGoal.IsGoalFulfilled(this))
            {
                this.EndGame();
            }
            else
            {
                this.feedback.Add("You fulfilled the quota!");
                this.gameState = GameState.PickFromKitchen;
            }
        }

        public void EndGame()
        {
            //should send a new embed
            this.gameState = GameState.GameOver;
        }

        public DiscordEmbedBuilder GetUIEmbed()
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{this.player.name}'s Kitchen",
                Color = DiscordColor.Azure
            };            

            if (this.gameState == GameState.Loading)
            {
                embed.Description = $"Preparing your Kitchen. This may take a while... :hourglass_flowing_sand:";
                return embed;
            }
            if (this.gameState == GameState.GameOver)
            {
                string emptyDesc = "\u200B";

                for (int i = 0; i < 50; i++) emptyDesc += " \u200B";

                embed.AddField("\u200B", emptyDesc, true);
                embed.AddField("\u200B", "```\u200B    Game Over!```", true);
                embed.AddField("\u200B", emptyDesc, true);

                embed.AddField("\u200B", $"```fix\n\u200BYou've finished the game with a score of {this.player.curPoints}p and lasted {this.curRound} rounds! To play again, use a!play.```");

                return embed;
            }

            string kitchentStatsString = $"Current Score: {this.player.curPoints}p\nCurrent Round: {this.curRound}";
            // Check if there's a quota next round \n(Quota next Round!)"
            embed.AddField("Kitchen Stats", kitchentStatsString, true);
            embed.AddField("\u200B", "\u200B\n\n**The Kitchen**\n\u200B", true);
            //haven't been done yet

            try
            {
                Goal goal = this.goalGenerator.CurrentGoal(this);

                embed.AddField($"Next Quota: Round {goal.round}", goal.GetDescription(), true);
            }
            catch (Exception)
            {
                embed.AddField("There's no next quota.", "\u200B", true);
            }            


            var kitchen = this.player.kitchen.GetAllIngredients();

            // The kitchen field
            for (int i=0; i<kitchen.Count && i<5; i++)
            {                
                if (kitchen[i] == null)
                {
                    embed.AddField($"Empty Kitchen Slot #{i+1}", "\u200B", true);

                    continue;
                }


                string kitchenTitle = kitchen[i].GetTitleText();
                if (i >= 3) kitchenTitle = $"\u200B\n{kitchenTitle}";
                if (this.gameState == GameState.PickFromKitchen) kitchenTitle += $" - {BotHandler.numToEmoji[i+1]}";

                string kitchenDesc = kitchen[i].GetDescriptionText(this);
                if (kitchenDesc.Equals(string.Empty)) kitchenDesc = "\u200B";

                if (this.pickingChoices.pick == i)
                {
                    if (kitchen[i].text.Equals(string.Empty)) kitchenDesc = "ini\n[selected]";
                    else kitchenDesc = $"ini\n[{kitchenDesc}]";
                }
                else
                {
                    if (kitchen[i].glowLocation == GameLocation.Kitchen || kitchen[i].glowLocation == GameLocation.Any)
                    {
                        if (kitchen[i].GlowCondition(this, i))
                        {
                            kitchenDesc = $"fix\n{kitchenDesc}";
                        }
                    }
                }

                kitchenDesc = $"```{kitchenDesc} ```";

                embed.AddField(kitchenTitle, kitchenDesc, true);
            }

            for (int i=kitchen.Count; i<5; i++)
            {
                embed.AddField($"\u200B\n\nKitchen Slot {i+1}", "(empty)", true);
            }


            // The field about the next ingredient coming
            string nextTitle = "__Next in the Kitchen__\n";

            Ingredient nextIngr = this.player.kitchen.nextOption;

            nextTitle += nextIngr.GetTitleText();
            
            string nextDesc = nextIngr.GetDescriptionText(this);
            if (nextDesc.Equals(string.Empty)) nextDesc = "\u200B";

            nextDesc = "```" + nextDesc + " ```";
            embed.AddField(nextTitle, nextDesc, true);


            // Empty field to add some spacing
            embed.AddField("\u200B", "\u200B");

            // The hand field
            for (int i=0; i<3; i++)
            {
                string handTitle = string.Empty;
                string handDesc = string.Empty;

                if (this.player.hand.ingredients[i] == null)
                {
                    if (this.gameState == GameState.ChooseInHandForIngredient) handTitle = $"Empty Dish Slot {BotHandler.numToEmoji[i+1]}";
                    else handTitle = $"Empty Dish Slot #{i + 1}";
                    embed.AddField(handTitle, "\u200B", true);
                    continue;
                }

                handTitle = this.player.hand.ingredients[i].GetTitleText();

                if (this.gameState == GameState.ChooseInHandForIngredient) handTitle += $" - {BotHandler.numToEmoji[i + 1]}";
                
                if (this.player.hand.ingredients[i].GetDescriptionText(this).Equals(string.Empty))
                {
                    handDesc = "\u200B";
                }
                else
                {
                    handDesc = this.player.hand.ingredients[i].GetDescriptionText(this);

                    if (this.player.hand.ingredients[i].glowLocation == GameLocation.Hand || this.player.hand.ingredients[i].glowLocation == GameLocation.Any)
                    {
                        if (this.player.hand.ingredients[i].GlowCondition(this, i))
                        {
                            handDesc = $"fix\n{handDesc}";
                        }
                    }
                }

                handDesc = $"```{handDesc} ```";
                embed.AddField(handTitle, handDesc, true);
            }

            // Feedback messages from Ingredients
            string feedbackMsg = string.Empty;

            for (int i=0; i<this.feedback.Count; i++)
            {
                feedbackMsg += $"- {this.feedback[i]}\n";
            }

            if (!(this.feedback.Count == 0 || feedbackMsg.Equals(string.Empty)))
            {
                embed.AddField("Game Events", feedbackMsg);
            }


            // Instructions on what buttons to click
            string instrTitle = string.Empty, instrDescription = string.Empty;
            switch (this.gameState)
            {
                case GameState.PickFromKitchen:
                    instrTitle = "Pick an ingredient to add to your dish or cook your dish.";
                    instrDescription = ":one::two::three::four::five: - Pick an ingredient.\n:fork_knife_plate: - Cook your dish.";
                    break;
                case GameState.ChooseInHandForIngredient:
                    instrTitle = "Choose where to put your ingredient.";
                    instrDescription = ":one::two::three: - Place on this spot.\n:no_entry_sign: - Cancel picking an ingredient.";
                    break;
                case GameState.BeforeQuota:
                    instrTitle = "Quota this round! Do you want to cook your dish before the quota?";
                    instrDescription = ":fork_knife_plate: - Cook your dish.\n:no_entry_sign: - Proceed without cooking.";
                    break;
                default:
                    instrTitle = "Instructions";
                    instrDescription = "Instructions Description";
                    break;
            }

            embed.AddField(instrTitle, instrDescription);

            return embed;
        }
    }
}
