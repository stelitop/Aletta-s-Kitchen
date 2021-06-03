using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class EnteringKitchenArgs : EffectArgs
        {
            public int kitchenPos;
            public EnteringKitchenArgs(EffectType effectType, int kitchenPos) : base(effectType)
            {
                this.kitchenPos = kitchenPos;
            }
        }
    }
}
