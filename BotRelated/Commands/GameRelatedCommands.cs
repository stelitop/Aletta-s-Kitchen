using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated.Commands
{
    public class GameRelatedCommands : BaseCommandModule
    {
        [RequiredUserState(UserState.Idle)]
        [Command("play")]
        public async Task StartGame(CommandContext ctx)
        {
            BotHandler.SetUserState(ctx.User.Id, UserState.InGame);

            await ctx.RespondAsync("Test").ConfigureAwait(false);
        }
    }
}
