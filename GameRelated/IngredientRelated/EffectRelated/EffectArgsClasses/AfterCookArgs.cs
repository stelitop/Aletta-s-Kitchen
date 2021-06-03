using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class AfterCookArgs : EffectArgs
        {
            public List<Ingredient> hand;
            public AfterCookArgs(EffectType effectType, List<Ingredient> hand) : base(effectType)
            {
                this.hand = hand;
            }
        }
    }
}
