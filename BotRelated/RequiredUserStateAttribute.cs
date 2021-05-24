using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.BotRelated
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]

    public class RequiredUserStateAttribute : CheckBaseAttribute
    {
        public UserState[] States { get; private set; }

        public RequiredUserStateAttribute(UserState state)
        {
            this.States = new UserState[1];
            this.States[0] = state;
        }
        public RequiredUserStateAttribute(UserState[] states)
        {
            this.States = states;
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(this.States.Contains(BotHandler.GetUserState(ctx.User.Id)));
        }
    }
}
