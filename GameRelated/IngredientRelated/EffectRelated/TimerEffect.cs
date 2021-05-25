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
        
        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        {
            _currentTime--;
            if (_currentTime == 0)
            {
                _currentTime = _tickInterval;
                await Tick(caller, game, args);
            }
        }

        public abstract Task Tick(Ingredient caller, Game game, EffectArgs args);
    }
}
