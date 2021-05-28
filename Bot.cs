﻿using Aletta_s_Kitchen.BotRelated;
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
            //Commands.RegisterCommands<InGameCommands>();
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
            this.Client.MessageReactionAdded += InGameButtonClickReactionAdded;
            this.Client.MessageReactionRemoved += InGameButtonClickReactionRemoved;
            return Task.CompletedTask;
        }

        private async Task InGameButtonClick(DiscordClient client, ulong id, DiscordMessage msg, DiscordEmoji emoji)
        {
            if (BotHandler.GetUserState(id) != UserState.InGame) return;
            if (!BotHandler.playerGames.ContainsKey(id)) return;
            if (BotHandler.playerGames[id].UIMessage != msg) return;

            List<DiscordEmoji> emojiButtons = new List<DiscordEmoji>
            {
                DiscordEmoji.FromName(client, ":one:"),
                DiscordEmoji.FromName(client, ":two:"),
                DiscordEmoji.FromName(client, ":three:"),
                DiscordEmoji.FromName(client, ":four:"),
                DiscordEmoji.FromName(client, ":five:"),
                DiscordEmoji.FromName(client, ":fork_knife_plate:"),
                DiscordEmoji.FromName(client, ":no_entry_sign:")
            };

            int emojiIndex = emojiButtons.IndexOf(emoji);
            if (emojiIndex == -1) return;

            await BotHandler.playerGames[id].ProceedButtonPress(emojiIndex);
        }

        private async Task InGameButtonClickReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
            => await InGameButtonClick(client, args.User.Id, args.Message, args.Emoji);

        private async Task InGameButtonClickReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
            => await InGameButtonClick(client, args.User.Id, args.Message, args.Emoji);
    }
}
