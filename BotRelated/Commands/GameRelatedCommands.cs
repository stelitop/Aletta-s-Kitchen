using Aletta_s_Kitchen.GameRelated;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
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

            if (BotHandler.playerGames.ContainsKey(ctx.User.Id)) BotHandler.playerGames.Remove(ctx.User.Id);

            Game game = new Game();
            BotHandler.playerGames.Add(ctx.User.Id, game);

            await game.Start();
            game.player.name = ctx.User.Username;

            DiscordMessage gameMessage = await ctx.RespondAsync(game.GetUIEmbed().Build()).ConfigureAwait(false);
            game.UIMessage = gameMessage;

            List<DiscordEmoji> emojiButtons = new List<DiscordEmoji>
            {
                DiscordEmoji.FromName(ctx.Client, ":one:"),
                DiscordEmoji.FromName(ctx.Client, ":two:"),
                DiscordEmoji.FromName(ctx.Client, ":three:"),
                DiscordEmoji.FromName(ctx.Client, ":four:"),
                DiscordEmoji.FromName(ctx.Client, ":five:"),
                DiscordEmoji.FromName(ctx.Client, ":fork_knife_plate:"),
                DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")
            };

            for (int i=0; i<emojiButtons.Count; i++)
            {
                await gameMessage.CreateReactionAsync(emojiButtons[i]).ConfigureAwait(false);
            }

            await game.UIMessage.ModifyAsync(game.GetUIEmbed().Build()).ConfigureAwait(false);

            game.gameState = GameState.PickFromKitchen;

            while (true)
            {
                if (game.gameState == GameState.GameOver) break;
            }

            BotHandler.SetUserState(ctx.User.Id, UserState.Idle);
            if (BotHandler.playerGames.ContainsKey(ctx.User.Id)) BotHandler.playerGames.Remove(ctx.User.Id);
        }
    }
}
