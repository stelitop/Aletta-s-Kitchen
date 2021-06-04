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

            for (int i=0; i<BotHandler.emojiButtons.Count; i++)
            {
                await gameMessage.CreateReactionAsync(BotHandler.emojiButtons[i]).ConfigureAwait(false);
            }

            game.gameState = GameState.PickFromKitchen;

            await game.UIMessage.ModifyAsync(game.GetUIEmbed().Build()).ConfigureAwait(false);

            //await Analysis.DiscordEmbedAnalysis(game.GetUIEmbed(), ctx.Channel);

            while (true)
            {
                if (game.gameState == GameState.GameOver) break;
            }

            BotHandler.SetUserState(ctx.User.Id, UserState.Idle);
            if (BotHandler.playerGames.ContainsKey(ctx.User.Id)) BotHandler.playerGames.Remove(ctx.User.Id);
        }

        [Command("report")]
        public async Task UserReport(CommandContext ctx, [RemainingText]string report)
        {
            DiscordChannel channel = null;

            try
            {
                channel = ctx.Guild.GetChannel(849630044288843786);
            }
            catch(Exception)
            {
                await ctx.Channel.SendMessageAsync("Your report couldn't be send. There was a problem with finding the reports channel.");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Brown,
                Title = $"{ctx.User.Username} Submitted a Report",
                Description = report,
                Timestamp = ctx.Message.Timestamp,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
            };
            embed.Footer.IconUrl = ctx.User.AvatarUrl;
            embed.Footer.Text = "\u200B";

            await channel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
        }

        [Command("feedback")]
        public async Task UserFeedback(CommandContext ctx, [RemainingText] string feedback)
        {
            DiscordChannel channel = null;

            try
            {
                channel = ctx.Guild.GetChannel(849961675255054346);
            }
            catch (Exception)
            {
                await ctx.Channel.SendMessageAsync("Your feedback couldn't be send. There was a problem with finding the feedback channel.");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Azure,
                Title = $"{ctx.User.Username} Submitted Feedback",
                Description = feedback,
                Timestamp = ctx.Message.Timestamp,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
            };
            embed.Footer.IconUrl = ctx.User.AvatarUrl;
            embed.Footer.Text = "\u200B";

            await channel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
        }
    }
}
