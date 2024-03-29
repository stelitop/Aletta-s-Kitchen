﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated.Commands
{
    public class InformationCommands : BaseCommandModule
    {
        [Command("about")]
        public async Task Rules(CommandContext ctx)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = "Aletta's Kitchen",
                Color = DiscordColor.Azure
            };

            embed.AddField("About",
                "```In a quaint part of the world there works a girl in a cafe. " +
                "The food she serves smell of exotic spices, seeds and meats - a fusion of things otherwise a thousand miles apart. " +
                "Her patrons can tell you that she experiments - in texture, taste and nutrition - and never to expect the same.It may be foul. It may be ambrosia.But it is always something new. " +
                "Welcome to Aletta’s Kitchen.```");

            embed.AddField("How To Play",
                "```1) The game consists of the kitchen and your dish.\n" +
                "2) You can select ingredients from the kitchen to add to your dish. Your dish can only hold a maximum of 3 ingredients.\n" +
                "3) The kitchen holds 5 ingredients. Whenever you select an ingredient, a new one takes its place. You can preview what the next ingredient will be.\n" +
                "4) Once you've picked some ingredient, you can cook your dish. The points will be tallied to your total score. The dish doesn't need to be full to be cooked.\n" +
                "5) You must meet an ever-increasing score quota every 5 turns or you will lose the game. After you pick an ingredient, the game goes to the next round.```");

            embed.AddField("How To Use The Bot",
                "```1) To play, use the command a!play in a channel or in DMs.\n" +
                "2) The bot would then create a message which would be used for the entire game.\n" +
                "3) There would be reactions that act as buttons through which you interact with the game." +
                "4) You can use the emojis with number 1-5 to pick ingredients, or the fork & knife to cook your dish. Instructions would be provided.```");

            await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
        }
    }
}
