using Aletta_s_Kitchen.BotRelated.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
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
                Timeout = TimeSpan.FromSeconds(60)
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
            //Commands.RegisterCommands<InformationCommands>();

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task OnClientReady(DiscordClient client, ReadyEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

            return;
        }
    }
}
