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

        public DiscordMessage UIMessage;

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();
            this.gameState = GameState.None;

            this.feedback = new List<string>();

            this.goalGenerator = null;

            this.UIMessage = null;
        }

        public async Task Start() => await this.Start(BotHandler.genericPool, new SquareIncrGoalGenerator());
        public async Task Start(IngredientPool pool, GoalGenerator goalGenerator)
        { 
            this.gameState = GameState.Loading;
            this.curRound = 1;            
            this.pool = new IngredientPool(pool);
            this.player = new Player();

            this.goalGenerator = goalGenerator;

            await this.player.kitchen.Restart(this);
        }

        public async Task NextRound()
        {
            this.curRound++;

            await this.player.kitchen.FillEmptySpots(this);
            int curIngrInShop = this.player.kitchen.OptionsCount;

            for (int i=0; i<curIngrInShop; i++)
            {
                if (this.player.kitchen.OptionAt(i) == null) continue;

                EffectArgs args = new EffectArgs.TimerArgs(EffectType.Timer, i);
                await Effect.CallEffects(this.player.kitchen.OptionAt(i).effects, EffectType.Timer, this.player.kitchen.OptionAt(i), this, args);
            }

            this.gameState = GameState.PickFromKitchen;            

            if (this.goalGenerator.CurrentGoal(this).round == this.curRound)
            {
                this.gameState = GameState.BeforeQuota;

                if (this.goalGenerator.CurrentGoal(this).IsGoalFulfilled(this))
                {
                    this.CheckQuota();
                }
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
                embed.Color = DiscordColor.Gray;

                string emptyDesc = "\u200B";

                for (int i = 0; i < 50; i++) emptyDesc += " \u200B";

                embed.AddField("\u200B", emptyDesc, true);
                embed.AddField("\u200B", "```\u200B    Game Over!```", true);
                embed.AddField("\u200B", emptyDesc, true);

                embed.AddField("\u200B", $"```fix\n\u200BYou've finished the game with a score of {this.player.curPoints}p and lasted {this.curRound} rounds! To play again, use a!play.```");

                return embed;
            }

            string kitchentStatsString = $"Current Score: {this.player.curPoints}p\nCurrent Round: {this.curRound}";
            
            if (this.goalGenerator.CurrentGoal(this).round - this.curRound == 1)
            {
                kitchentStatsString += "\n*(quota next turn!)*";
            }


            embed.AddField("Kitchen Stats", kitchentStatsString, true);
            embed.AddField("\u200B", "\u200B\n\n\n**The Kitchen**\n\u200B", true);
            //haven't been done yet

            try
            {
                Goal goal = this.goalGenerator.CurrentGoal(this);

                if (goal.round == this.curRound && this.gameState != GameState.BeforeQuota)
                {
                    this.curRound++;
                    goal = this.goalGenerator.CurrentGoal(this);
                    this.curRound--;
                }

                embed.AddField($"Next Quota: Round {goal.round}", goal.GetDescription(this), true);
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
                if (i >= 3) kitchenTitle = $"\u200B\n\n{kitchenTitle}";
                if (this.gameState == GameState.PickFromKitchen && this.player.hand.OptionsCount < 3) kitchenTitle += $" - {BotHandler.numToEmoji[i+1]}";

                string kitchenDesc = kitchen[i].GetDescriptionText(this, GameLocation.Kitchen);
                if (kitchenDesc.Equals(string.Empty)) kitchenDesc = "\u200B";

                if (kitchen[i].glowLocation == GameLocation.Kitchen || kitchen[i].glowLocation == GameLocation.Any)
                {
                    if (kitchen[i].GlowCondition(this, i))
                    {
                        kitchenDesc = $"fix\n{kitchenDesc}";
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
            string nextTitle = "__Next in the Kitchen__\n\n";

            Ingredient nextIngr = this.player.kitchen.nextOption;

            nextTitle += nextIngr.GetTitleText();
            
            string nextDesc = nextIngr.GetDescriptionText(this, GameLocation.NextIngredient);
            if (nextDesc.Equals(string.Empty)) nextDesc = "\u200B";

            nextDesc = "```" + nextDesc + " ```";
            embed.AddField(nextTitle, nextDesc, true);


            // The Dish title
            embed.AddField("\u200B", "\u200B", true);
            embed.AddField("\u200B", "**The Dish**", true);
            embed.AddField("\u200B", "\u200B", true);

            // The hand field
            for (int i=0; i<this.player.hand.OptionsCount && i<3; i++)
            {
                string handTitle = string.Empty;
                string handDesc = string.Empty;

                if (this.player.hand.IngredientAt(i) == null)
                {
                    //if (this.gameState == GameState.ChooseInHandForIngredient) handTitle = $"Empty Dish Slot {BotHandler.numToEmoji[i+1]}";
                    //else
                    handTitle = $"Empty Dish Slot #{i + 1}";
                    embed.AddField(handTitle, "\u200B", true);
                    continue;
                }

                handTitle = this.player.hand.IngredientAt(i).GetTitleText();

                //if (this.gameState == GameState.ChooseInHandForIngredient) handTitle += $" - {BotHandler.numToEmoji[i + 1]}";
                
                if (this.player.hand.IngredientAt(i).GetDescriptionText(this, GameLocation.Hand).Equals(string.Empty))
                {
                    handDesc = "\u200B";
                }
                else
                {
                    handDesc = this.player.hand.IngredientAt(i).GetDescriptionText(this, GameLocation.Hand);

                    if (this.player.hand.IngredientAt(i).glowLocation == GameLocation.Hand || this.player.hand.IngredientAt(i).glowLocation == GameLocation.Any)
                    {
                        if (this.player.hand.IngredientAt(i).GlowCondition(this, i))
                        {
                            handDesc = $"fix\n{handDesc}";
                        }
                    }
                }

                handDesc = $"```{handDesc} ```";
                embed.AddField(handTitle, handDesc, true);
            }

            for (int i=this.player.hand.OptionsCount; i<3; i++)
            {
                string handTitle = string.Empty;
                string handDesc = string.Empty;
                
                handTitle = $"Empty Dish Slot #{i + 1}";
                embed.AddField(handTitle, "\u200B", true);
                continue;
                
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
                    if (this.player.hand.OptionsCount < 3 && this.player.hand.OptionsCount < this.player.hand.handLimit) instrDescription = ":one::two::three::four::five: - Pick an ingredient.\n:fork_knife_plate: - Cook your dish.";
                    else instrDescription = ":fork_knife_plate: - Cook your dish.";
                    break;
                //case GameState.ChooseInHandForIngredient:
                //    instrTitle = "Choose where to put your ingredient.";
                //    instrDescription = ":one::two::three: - Place on this spot.\n:no_entry_sign: - Cancel picking an ingredient.";
                //    break;
                case GameState.BeforeQuota:
                    instrTitle = "Quota this round! Do you want to cook your dish before the quota?";
                    //instrDescription = ":fork_knife_plate: - Cook your dish.\n:no_entry_sign: - Proceed without cooking.";
                    instrDescription = ":fork_knife_plate: - Cook your dish.";
                    break;
                default:
                    instrTitle = "Instructions";
                    instrDescription = "Instructions Description";
                    break;
            }

            embed.AddField(instrTitle, instrDescription);

            return embed;
        }

        /*
         * 0-4 - buttons from 1 to 5
         * 5 - cook
         * 6 - cancel
         */
        public async Task ProceedButtonPress(int emojiIndex)
        {
            switch (this.gameState)
            {
                case GameState.PickFromKitchen:

                    if (emojiIndex < 5)
                    {
                        if (!this.player.kitchen.OptionAt(emojiIndex).CanBeBought(this, emojiIndex))
                        {
                            this.feedback.Clear();
                            this.feedback.Add($"{this.player.kitchen.OptionAt(emojiIndex).name} can't be picked currently!");
                            break;
                        }

                        await this.player.kitchen.PickIngredient(this, emojiIndex);
                    }
                    else if (emojiIndex == 5)
                    {
                        await this.player.hand.Cook(this);
                    }
                    break;

                case GameState.BeforeQuota:

                    if (emojiIndex >= 5)
                    {
                        if (emojiIndex == 5)
                        {
                            await this.player.hand.Cook(this);
                        }
                        else if (emojiIndex == 6) ;

                        this.CheckQuota();
                    }

                    break;

                default:
                    break;
            }


            await this.UIMessage.ModifyAsync(this.GetUIEmbed().Build()).ConfigureAwait(false);

            if (this.gameState == GameState.GameOver) this.EndGame();
        }

        public void RestOfGameBuff(RoGCondition condition, RoGBuff buff, bool includeHand = true)
        {
            foreach (var ingr in this.player.kitchen.GetAllNonNullIngredients())
            {
                if (condition(ingr)) buff(ingr);
            }

            if (includeHand)
                foreach (var ingr in this.player.hand.GetAllIngredients())
                {
                    if (ingr == null) continue;
                    if (condition(ingr)) buff(ingr);
                }   

            if (this.player.kitchen.nextOption != null)
            {
                if (condition(this.player.kitchen.nextOption)) buff(this.player.kitchen.nextOption);
            }

            foreach (var ingr in this.pool.ingredients)
            {
                if (ingr == null) continue;
                if (condition(ingr)) buff(ingr);
            }
        }

        public delegate bool RoGCondition(Ingredient ingr);
        public delegate void RoGBuff(Ingredient ingr);
    }
}
