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
    [RequiredUserState(new UserState[] { UserState.InGame, UserState.Tutorial})]
    public class InGameCommands : BaseCommandModule
    {
        [Command("endgame")]
        public async Task EndGame(CommandContext ctx) => await EndGameStatic(ctx);

        public static async Task EndGameStatic(CommandContext ctx)
        {
            BotHandler.SetUserState(ctx.User.Id, UserState.Idle);

            if (!BotHandler.playerGames.ContainsKey(ctx.User.Id)) return;

            Game game = BotHandler.playerGames[ctx.User.Id];

            await game.EndGame();

            //await game.UIMessage.ModifyAsync(embed: (await BotHandler.playerGames[ctx.User.Id].GetUIEmbed()).Build()).ConfigureAwait(false);
            await game.UpdateUI();

            BotHandler.playerGames.Remove(ctx.User.Id);

            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
        }        

        [Command("newui")]
        public async Task NewUI(CommandContext ctx)
        {
            Game game;
            if (!BotHandler.playerGames.TryGetValue(ctx.User.Id, out game)) return;

            var gameState = game.gameState;
            game.gameState = GameState.Loading;

            DiscordMessage gameMessage = await ctx.RespondAsync((await game.GetUIEmbed()).Build()).ConfigureAwait(false);
            game.UIMessage = gameMessage;

            for (int i = 0; i < BotHandler.emojiButtons.Count; i++)
            {
                if (i >= 6 && BotHandler.GetUserState(ctx.User.Id) != UserState.Tutorial) break;
                await gameMessage.CreateReactionAsync(BotHandler.emojiButtons[i]).ConfigureAwait(false);
            }

            game.gameState = gameState;

            await game.UpdateUI();            
        }
    }
}
