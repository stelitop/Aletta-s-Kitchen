using Aletta_s_Kitchen.GameRelated;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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

            game.gameState = GameState.PickFromKitchen;

            var interactivity = ctx.Client.GetInteractivity();

            while (true)
            {
                await gameMessage.ModifyAsync(game.GetUIEmbed().Build()).ConfigureAwait(false);

                var interaction = await interactivity.WaitForReactionAsync(
                    x => x.User.Id == ctx.User.Id && x.Message.Id == gameMessage.Id && emojiButtons.Contains(x.Emoji)).ConfigureAwait(false);

                int emojiIndex = emojiButtons.IndexOf(interaction.Result.Emoji);
                switch (game.gameState)
                {
                    case GameState.PickFromKitchen:                        

                        if (emojiIndex < 5)
                        {                            
                            if (!game.player.kitchen.OptionAt(emojiIndex).CanBeBought(game, emojiIndex))
                            {
                                game.feedback.Clear();
                                game.feedback.Add($"{game.player.kitchen.OptionAt(emojiIndex).name} can't be bought currently!");
                                break;
                            }

                            game.pickingChoices.pick = emojiIndex;
                            game.gameState = GameState.ChooseInHandForIngredient;
                        }
                        else if (emojiIndex == 5)
                        {
                            await game.player.hand.Cook(game);
                        }
                        break;

                    case GameState.ChooseInHandForIngredient:

                        if (emojiIndex < 3)
                        {
                            game.pickingChoices.spot = emojiIndex;
                            if (game.pickingChoices.pick > -1)
                            {
                                await game.player.kitchen.PickIngredient(game);
                            }
                        }
                        else if (emojiIndex == 6)
                        {
                            game.pickingChoices.Clear();
                            game.gameState = GameState.PickFromKitchen;
                        }

                        break;

                    default:
                        break;
                }
            }
        }
    }
}
