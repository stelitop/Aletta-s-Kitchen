using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public abstract class TimerEffect : Effect
    {
        public int tickInterval { get; private set; }
        public int currentTime { get; private set; }

        protected TimerEffect(int tickInterval) : base(EffectType.Timer)
        {
            if (tickInterval < 1) tickInterval = 1;
            this.tickInterval = this.currentTime = tickInterval;
        }
        
        //Equivalent to the timer ticking by 1
        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        {
            currentTime--;
            if (currentTime == 0)
            {
                currentTime = tickInterval;
                await Trigger(caller, game, args);
            }
        }

        public override Effect Copy()
        {
            TimerEffect ret = (TimerEffect)base.Copy();

            ret.tickInterval = this.tickInterval;
            ret.currentTime = this.currentTime;

            return ret;
        }

        protected abstract Task Trigger(Ingredient caller, Game game, EffectArgs args);
    }
}
