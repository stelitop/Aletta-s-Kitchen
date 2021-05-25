using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Hand
    {
        public List<Ingredient> ingredients;

        public void Clear()
        {
            this.ingredients.Clear();
        }

        /**
         * Order of Operations:
         * 1) Call all OnBeingCookedBefore effects
         * 2) Remove ingredients from Hand
         * 3) Call all OnBeingCookedAfter effects
         * 4) Add the Ingredients to the Cooked history
         */
        public async Task Cook(Game game)
        {
            //1)
            for (int i=0; i<this.ingredients.Count; i++)
            {
                EffectArgs args = new EffectArgs(EffectType.OnBeingCookedBefore);
                await Effect.CallEffects(this.ingredients[i].effects, EffectType.OnBeingCookedBefore, this.ingredients[i], game, args);                
            }

            List<Ingredient> handTemp = this.ingredients;
            //2)
            this.Clear();

            //3)
            for (int i=0; i<handTemp.Count; i++)
            {
                EffectArgs args = new EffectArgs(EffectType.OnBeingCookedAfter);
                await Effect.CallEffects(handTemp[i].effects, EffectType.OnBeingCookedAfter, handTemp[i], game, args);
            }

            //4)
            for (int i = 0; i < handTemp.Count; i++)
            {
                game.player.cookHistory.Add(handTemp[i].Copy());
            }
        }
    }
}
