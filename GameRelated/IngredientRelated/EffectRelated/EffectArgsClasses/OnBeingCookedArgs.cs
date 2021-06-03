using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class OnBeingCookedArgs : EffectArgs
        {
            public int dishPoints;
            public int handPos;
            public OnBeingCookedArgs(EffectType effectType, int dishPoints, int handPos) : base(effectType)
            {
                this.dishPoints = dishPoints;
                this.handPos = handPos;
            }
        }
    }
}
