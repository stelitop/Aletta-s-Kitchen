using Aletta_s_Kitchen.GameRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated
{    
    public sealed class BotHandler
    {
        public static Random globalRandom = new Random();

        public static Bot bot = new Bot();

        public static Dictionary<ulong, Game> playerGames = new Dictionary<ulong, Game>();

        private static Dictionary<ulong, UserState> _userState = new Dictionary<ulong, UserState>();

        public static Dictionary<int, string> numToEmoji { get; } = new Dictionary<int, string>{
            { 1, ":one:"},
            { 2, ":two:"},
            { 3, ":three:"},
            { 4, ":four:"},
            { 5, ":five:"}
        };

        public static List<DiscordEmoji> emojiButtons = new List<DiscordEmoji>();

        public static UserState GetUserState(ulong id)
        {
            if (_userState.ContainsKey(id)) return _userState[id];
            else
            {
                _userState.Add(id, UserState.Idle);
                return UserState.Idle;
            }
        }
        public static void SetUserState(ulong id, UserState state)
        {
            if (_userState.ContainsKey(id)) _userState[id] = state;
            else _userState.Add(id, state);
        }

        public readonly static IngredientPool genericPool = new IngredientPool();
    }
}
