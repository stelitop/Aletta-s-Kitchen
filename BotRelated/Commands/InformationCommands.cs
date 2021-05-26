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
    public class InformationCommands : BaseCommandModule
    {
        [Command("about")]
        public async Task Rules(CommandContext ctx)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = "Aletta's Kitchen",
                Color = DiscordColor.Gold
            };

            embed.AddField("About",
                "```In a quaint part of the world there works a girl in a cafe. " +
                "The food she serves smell of exotic spices, seeds and meats - a fusion of things otherwise a thousand miles apart. " +
                "Her patrons can tell you that she experiments - in texture, taste and nutrition - and never to expect the same.It may be foul. It may be ambrosia.But it is always something new. " +
                "Welcome to Aletta’s Kitchen.```");

            embed.AddField("How To Play",
                "```1) The game consists of the kitchen and your dish.\n" +
                "2) You can select ingredients from the kitchen to add to your dish. Your dish can only hold a maximum of 3 ingredients.\n" +
                "3) The kitchen holds a maximum of 5 ingredients. Whenever you select an ingredient, a new one takes its place. You can preview what the next ingredient will be.\n" +
                "4) Once you cook your dish, the points will be tallied to your total score.\n" +
                "5) You must meet an ever-increasing score quota every 5 turns or you will lose the game.```");

            await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
        }
    }
}
