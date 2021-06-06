using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated.Commands
{
    [RequiredUserState(UserState.InGame)]
    public class InGameCommands : BaseCommandModule
    {
        [Command("endgame")]
        public async Task EndGame(CommandContext ctx)
        {
            await BotHandler.playerGames[ctx.User.Id].EndGame();

            await BotHandler.playerGames[ctx.User.Id].UIMessage.ModifyAsync(embed: BotHandler.playerGames[ctx.User.Id].GetUIEmbed().Build()).ConfigureAwait(false);

            BotHandler.playerGames.Remove(ctx.User.Id);
        }
    }
}
