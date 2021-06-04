using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class IngredientAddedToHand : EffectArgs
        {
            public Ingredient ingredient;
            public IngredientAddedToHand(EffectType effectType, Ingredient ingr) : base(effectType)
            {
                this.ingredient = ingr;
            }
        }
    }
}
