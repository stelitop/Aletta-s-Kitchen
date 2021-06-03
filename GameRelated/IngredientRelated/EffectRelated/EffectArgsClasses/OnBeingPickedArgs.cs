using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class OnBeingPickedArgs : EffectArgs
        {
            public int kitchenPos, handPos;
            public OnBeingPickedArgs(EffectType effectType, int kitchenPos, int handPos) : base(effectType)
            {
                this.kitchenPos = kitchenPos;
                this.handPos = handPos;
            }
        }
    }
}
