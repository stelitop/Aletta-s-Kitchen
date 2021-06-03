using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class IngredientPickArgs : EffectArgs
        {
            public Ingredient pickedIngr;
            public IngredientPickArgs(EffectType effectType, Ingredient ingr) : base(effectType)
            {
                this.pickedIngr = ingr;
            }
        }
    }
}
