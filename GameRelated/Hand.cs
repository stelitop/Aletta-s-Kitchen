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

        public Hand()
        {
            this.ingredients = new List<Ingredient>();
        }

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
            int dishPoints = 0;
            for (int i=0; i<this.ingredients.Count; i++)
            {
                dishPoints += this.ingredients[i].points;
            }

            var args = new EffectArgs.OnBeingCookedArgs(EffectType.OnBeingCookedBefore, dishPoints);

            //1)
            for (int i=0; i<this.ingredients.Count; i++)
            {
                await Effect.CallEffects(this.ingredients[i].effects, EffectType.OnBeingCookedBefore, this.ingredients[i], game, args);                
            }

            List<Ingredient> handTemp = this.ingredients;
            //2)
            this.Clear();

            args.calledEffect = EffectType.OnBeingCookedAfter;

            //3)
            for (int i=0; i<handTemp.Count; i++)
            {
                await Effect.CallEffects(handTemp[i].effects, EffectType.OnBeingCookedAfter, handTemp[i], game, args);
            }

            //4)
            for (int i = 0; i < handTemp.Count; i++)
            {
                game.player.cookHistory.Add(handTemp[i].Copy());
            }

            game.player.curPoints += args.dishPoints;
        }
    }
}
