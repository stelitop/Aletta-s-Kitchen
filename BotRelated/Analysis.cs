using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated
{
    public class Analysis
    {
        public static async Task DiscordEmbedAnalysis(DiscordEmbedBuilder embed, DiscordChannel respondChannel)
        {
            int totalLen = 0;
            string analysis = string.Empty;

            if (embed.Title != null)
            {
                analysis += $"Title: {embed.Title.Length} chars\n";
                totalLen += embed.Title.Length;
            }

            if (embed.Description != null)
            {
                analysis += $"Description{embed.Description.Length} chars\n";
                totalLen += embed.Description.Length;
            }

            foreach (var field in embed.Fields)
            {
                analysis += $"Field ({field.Name}): {field.Name.Length + field.Value.Length} chars\n";
                totalLen += field.Name.Length + field.Value.Length;
            }

            analysis += $"Total embed length: {totalLen}\n";
            await respondChannel.SendMessageAsync(analysis).ConfigureAwait(false);
        }
    }
}
