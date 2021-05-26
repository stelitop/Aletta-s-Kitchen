using Aletta_s_Kitchen.GameRelated;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated.Commands
{
    [RequireOwnerAttribute]
    public class DebugCommands : BaseCommandModule
    {
        [Group("debug")]
        [Description("Commands only accessable to the bot owner. Only for debugging.")]
        public class DebugCommandsGroup : BaseCommandModule
        {
            [Command("roll")]
            [Description("Rolls a die between 1-6")]
            public async Task Roll(CommandContext ctx, int maxValue = 6, int amount = 1)
            {
                Random r = new Random();
                int roll = r.Next(1, maxValue + 1);
                string msg = roll.ToString();

                if (amount > 50) amount = 50;

                for (int i = 2; i <= amount; i++)
                {
                    roll = r.Next(1, maxValue + 1);
                    msg += ", " + roll.ToString();
                }

                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = $"You rolled: {msg}",
                    Color = DiscordColor.Azure
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
            }

            [Command("playerui")]
            [Description("Prototype of how the Player UI will look")]
            public async Task PrototypePlayerUI(CommandContext ctx)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Title = "Aletta's Kitchen",
                    Color = DiscordColor.Azure
                };

                embed.AddField("Kitchen Stats", "Current Score: ###\nCurrent Round: ##\n(Quota next Round!)", true);
                embed.AddField("Next in the Kitchen:", "Next Kitchen Ingredient's Info", true);
                embed.AddField("Next Quota: Round #", "The Quota's Description.", true);                

                embed.AddField("The Kitchen", "```1) Kitchen Ingredient 1's info\n2) Kitchen Ingredient 2's info\n3) Kitchen Ingredient 3's info\n4) Kitchen Ingredient 4's info\n5) Kitchen Ingredient 5's info\n```");                

                embed.AddField("Hand Ingredient 1", "```The effect of Hand Ingredient 1```", true);
                embed.AddField("Hand Ingredient 2", "```The effect of Hand Ingredient 2```", true);
                embed.AddField("Hand Ingredient 3", "```The effect of Hand Ingredient 3```", true);

                embed.AddField("Actions Feedback", "Will say what happened exactly if any randomness happened or whatever.");

                embed.AddField("Instructions on what to do next", "Could be \"Pick an Ingredient in the Kitchen.\", \"Choose an Ingredient in Hand to replace.\" or some other instruction for the title. Would show which reactions are available for a response for the description.");

                await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
            }

            [Command("playerui2")]
            [Description("Second prototype of how the Player UI will look")]
            public async Task PrototypePlayerUI2(CommandContext ctx)
            {
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Title = "Aletta's Kitchen",
                    Color = DiscordColor.Azure
                };

                embed.AddField("Kitchen Stats", "Current Score: ###\nCurrent Round: ##\n(Quota next Round!)", true);
                embed.AddField("\u200B", "\u200B\n\n\n\n\n  **The Kitchen**\n\u200B", true);
                embed.AddField("Next Quota: Round #", "The Quota's Description.", true);

                //embed.AddField("Next in the Kitchen:", "Next Kitchen Ingredient's Info", true);

                //embed.AddField("The Kitchen", "```1) Kitchen Ingredient 1's info\n2) Kitchen Ingredient 2's info\n3) Kitchen Ingredient 3's info\n4) Kitchen Ingredient 4's info\n5) Kitchen Ingredient 5's info\n```");

                embed.AddField("Kitchen Ingredient 1", "```The text of Kitchen Ingredient 1```", true);
                embed.AddField("Kitchen Ingredient 2", "```The text of Kitchen Ingredient 2```", true);
                embed.AddField("Kitchen Ingredient 3", "```The text of Kitchen Ingredient 3```", true);
                embed.AddField("Kitchen Ingredient 4", "```The text of Kitchen Ingredient 4```", true);
                embed.AddField("Kitchen Ingredient 5", "```The text of Kitchen Ingredient 5```", true);
                embed.AddField("Next Kitchen Ingredient", "```The text of Next Kitchen Ingredient```", true);

                embed.AddField("\u200B", "\u200B");

                embed.AddField("Hand Ingredient 1", "```The effect of Hand Ingredient 1```", true);
                embed.AddField("Hand Ingredient 2", "```The effect of Hand Ingredient 2```", true);
                embed.AddField("Hand Ingredient 3", "```The effect of Hand Ingredient 3```", true);

                embed.AddField("Actions Feedback", "Will say what happened exactly if any randomness happened or whatever.");

                embed.AddField("Instructions on what to do next", "Could be \"Pick an Ingredient in the Kitchen.\", \"Choose an Ingredient in Hand to replace.\" or some other instruction for the title. Would show which reactions are available for a response for the description.");

                await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
            }            
        }
    }
}
