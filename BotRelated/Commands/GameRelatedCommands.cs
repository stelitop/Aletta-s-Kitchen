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
        [Command("play")]
        public async Task StartGame(CommandContext ctx)
        {
            if (BotHandler.resetExpected)
            {
                await ctx.RespondAsync("Currently a reset is planned to take place soon, so the play function has been disabled. We are sorry for the inconvenience.");

                return;
            }

            if (BotHandler.GetUserState(ctx.User.Id) != UserState.Idle)
            {
                //await ctx.Channel.SendMessageAsync(embed: new DiscordEmbedBuilder { 
                //    Title = "You're Already In A Game",
                //    Description = "To end your current game, type a!endgame.",
                //    Color = DiscordColor.Red                  
                //}).ConfigureAwait(false);

                //return;

                await InGameCommands.EndGameStatic(ctx);
            }

            BotHandler.SetUserState(ctx.User.Id, UserState.InGame);

            if (BotHandler.playerGames.ContainsKey(ctx.User.Id)) BotHandler.playerGames.Remove(ctx.User.Id);

            Game game = new Game();
            BotHandler.playerGames.Add(ctx.User.Id, game);
            game.gameState = GameState.Loading;            
            game.player.name = ctx.User.Username;

            game.playerId = ctx.User.Id;

            DiscordMessage gameMessage = await ctx.RespondAsync((await game.GetUIEmbed()).Build()).ConfigureAwait(false);
            game.UIMessage = gameMessage;

            for (int i=0; i<BotHandler.emojiButtons.Count; i++)
            {
                if (i >= 6) break;
                await gameMessage.CreateReactionAsync(BotHandler.emojiButtons[i]).ConfigureAwait(false);
            }

            //game.gameState = GameState.PickFromKitchen;
            game.gameState = GameState.ChooseGamemode;

            //await game.UIMessage.ModifyAsync((await game.GetUIEmbed()).Build()).ConfigureAwait(false);
            await game.UpdateUI();

            //await Analysis.DiscordEmbedAnalysis(game.GetUIEmbed(), ctx.Channel);            
        }

        [Command("report")]
        public async Task UserReport(CommandContext ctx, [RemainingText]string report)
        {
            if (BotHandler.reportsChannel == null)
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

            await BotHandler.reportsChannel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
        }

        [Command("feedback")]
        public async Task UserFeedback(CommandContext ctx, [RemainingText] string feedback)
        {
            if (BotHandler.feedbackChannel == null)
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

            await BotHandler.feedbackChannel.SendMessageAsync(embed: embed.Build()).ConfigureAwait(false);
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
        }
    }
}
