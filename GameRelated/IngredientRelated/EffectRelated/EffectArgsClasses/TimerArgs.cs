using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class TimerArgs : EffectArgs
        {
            public int kitchenPos;

            public TimerArgs(EffectType effectType, int kitchenPos) : base(effectType)
            {
                this.kitchenPos = kitchenPos;
            }
        }
    }    
}
