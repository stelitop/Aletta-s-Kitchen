using Aletta_s_Kitchen.GameRelated.GoalTypes;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
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

        public Queue<Goal> goals;

        public GameState gameState;

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();
            this.gameState = GameState.None;

            this.goals = new Queue<Goal>();
        }

        public async Task Start(IngredientPool pool)
        {
            this.curRound = 1;            
            this.pool = new IngredientPool(pool);
            this.player = new Player();
            this.gameState = GameState.PickFromKitchen;

            await this.player.kitchen.FillEmptySpots(this);
        }

        public void NextRound()
        {
            this.curRound++;

            if (this.goals.Count > 0)
            {
                while (this.curRound >= this.goals.Peek().round)
                {
                    if (this.curRound > this.goals.Peek().round)
                    {
                        this.goals.Dequeue();
                    }
                    else if (this.curRound == this.goals.Peek().round)
                    {
                        bool result = this.goals.Peek().IsGoalFulfilled(this);

                        if (!result)
                        {
                            this.EndGame();
                            return;
                        }
                    }

                    if (this.goals.Count == 0) break;
                }
            }            
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public async Task<int> ChooseAHandSpot()
        {
            throw new NotImplementedException();
        }

        public DiscordEmbedBuilder GetUIEmbed()
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{this.player.name}'s Kitchen",
                Color = DiscordColor.Azure
            };

            embed.AddField("Kitchen Stats", $"Current Score: {this.player.curPoints}p\nCurrent Round: {this.curRound}\n(Quota next Round!)", true);
            embed.AddField("Next in the Kitchen:", $"{this.player.kitchen.nextOption.GetInfo()}", true);
            embed.AddField("Next Quota: Round #", "The Quota's Description.", true);

            string kitchenInfo = "```";            
            for (int i=0; i<this.player.kitchen.Count; i++)
            {
                kitchenInfo += $"{i+1}) {this.player.kitchen.OptionAt(i).GetInfo()}\n";
            }
            kitchenInfo += "```";
            embed.AddField("The Kitchen", kitchenInfo);

            for (int i=0; i<this.player.hand.ingredients.Count && i<3; i++)
            {
                if (this.player.hand.ingredients[i].text.Equals(string.Empty))
                {
                    embed.AddField($"{this.player.hand.ingredients[i].name} - {this.player.hand.ingredients[i].points}p", "(no text)", true);
                }
                else
                {
                    embed.AddField($"{this.player.hand.ingredients[i].name} - {this.player.hand.ingredients[i].points}p", this.player.hand.ingredients[i].text, true);
                }
            }

            for (int i=this.player.hand.ingredients.Count; i<3; i++)
            {
                embed.AddField($"Empty Dish Slot #{i+1}", "\u200B", true);
            }

            embed.AddField("Actions Feedback", "Will say what happened exactly if any randomness happened or whatever.");

            embed.AddField("Instructions on what to do next", "Could be \"Pick an Ingredient in the Kitchen.\", \"Choose an Ingredient in Hand to replace.\" or some other instruction for the title. Would show which reactions are available for a response for the description.");

            return embed;
        }
    }
}
