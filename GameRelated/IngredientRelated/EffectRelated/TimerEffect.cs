using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public abstract class TimerEffect : Effect
    {
        private int _tickInterval;
        private int _currentTime;

        protected TimerEffect(int tickInterval) : base(EffectType.Timer)
        {
            if (tickInterval < 1) tickInterval = 1;
            this._tickInterval = this._currentTime = tickInterval;
        }
        
        //Equivalent to the timer ticking by 1
        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        {
            _currentTime--;
            if (_currentTime == 0)
            {
                _currentTime = _tickInterval;
                await Trigger(caller, game, args);
            }
        }

        protected abstract Task Trigger(Ingredient caller, Game game, EffectArgs args);
    }
}
