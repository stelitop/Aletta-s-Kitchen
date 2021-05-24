﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
                    Color = DiscordColor.Blue
                };

                await ctx.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
            }
        }
    }
}
