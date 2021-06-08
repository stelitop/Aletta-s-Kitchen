using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.BotRelated.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen
{
    public class Bot
    {
        public static Bot DiscordBot = new Bot();

        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async void RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                //UseInternalLoggingHandler = true
            };

            Client = new DiscordClient(config);

            //listens to events
            Client.Ready += OnClientReady;
            Client.GuildAvailable += OnGuildAvailable;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(10)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<GameRelatedCommands>();
            Commands.RegisterCommands<InGameCommands>();
            Commands.RegisterCommands<InformationCommands>();

            Commands.RegisterCommands<DebugCommands>();
            
            await Client.ConnectAsync();

            this._cancelToken = new CancellationTokenSource();

            try { await Task.Delay(-1, this._cancelToken.Token); }
            catch (Exception) { }

            await Client.DisconnectAsync();            
        }

        public void StopBot()
        {
            if (this._cancelToken.Token.CanBeCanceled)
            {
                this._cancelToken.Cancel();
            }
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            //load the button emojis

            BotHandler.emojiButtons.Clear();
            
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":one:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":two:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":three:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":four:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":five:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":fork_knife_plate:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":arrow_left:"));
            BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":arrow_right:"));
            //BotHandler.emojiButtons.Add(DiscordEmoji.FromName(client, ":no_entry_sign:"));            

            //load the events for clicking buttons
            this.Client.MessageReactionAdded += InGameButtonClickReactionAdded;
            this.Client.MessageReactionRemoved += InGameButtonClickReactionRemoved;                        

            return Task.CompletedTask;
        }

        private Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs args)
        {
            if (args.Guild.Id == 846070541604749322)
            {
                try
                {
                    BotHandler.feedbackChannel = args.Guild.GetChannel(849961675255054346);

                    //reports channel
                    BotHandler.reportsChannel = args.Guild.GetChannel(849630044288843786);
                }
                catch (Exception)
                {

                }

            }

            return Task.CompletedTask;
        }

        private async Task InGameButtonClick(DiscordClient client, DiscordUser user, DiscordMessage msg, DiscordEmoji emoji)
        {
            if (BotHandler.GetUserState(user.Id) == UserState.Idle) return;
            if (!BotHandler.playerGames.ContainsKey(user.Id)) return;
            if (BotHandler.playerGames[user.Id].UIMessage != msg) return;

            int emojiIndex = BotHandler.emojiButtons.IndexOf(emoji);
            if (emojiIndex == -1) return;

            await BotHandler.playerGames[user.Id].ProcessButtonPress(user, emojiIndex);
        }

        private async Task InGameButtonClickReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
            => await InGameButtonClick(client, args.User, args.Message, args.Emoji);

        private async Task InGameButtonClickReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
            => await InGameButtonClick(client, args.User, args.Message, args.Emoji);
        
    }
}
