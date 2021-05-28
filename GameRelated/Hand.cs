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
        //public List<Ingredient> ingredients;
        public List<Ingredient> ingredients;

        public Hand()
        {
            this.ingredients = new List<Ingredient>();            
        }

        public void Clear()
        {
            this.ingredients = new List<Ingredient>();
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
            game.feedback.Clear();

            int dishPoints = 0;
            List<Ingredient> allIngr = new List<Ingredient>();

            for (int i=0; i<this.ingredients.Count; i++)
            {
                if (this.ingredients[i] == null) continue;
                dishPoints += this.ingredients[i].points;
                allIngr.Add(this.ingredients[i]);
            }

            if (allIngr.Count == 0) return;

            string feedbackMsg = "You cooked a dish made out of ";

            if (allIngr.Count == 1) feedbackMsg += $"__{allIngr[0].name}__";
            else if (allIngr.Count == 2) feedbackMsg += $"__{allIngr[0].name}__ and __{allIngr[1].name}__";
            else if (allIngr.Count == 3) feedbackMsg += $"__{allIngr[0].name}__, __{allIngr[1].name}__ and __{allIngr[2].name}__";

            var args = new EffectArgs.OnBeingCookedArgs(EffectType.OnBeingCookedBefore, dishPoints, -1);

            //1)
            for (int i=0; i<this.ingredients.Count; i++)
            {
                if (this.ingredients[i] == null) continue;
                args.handPos = i;
                await Effect.CallEffects(this.ingredients[i].effects, EffectType.OnBeingCookedBefore, this.ingredients[i], game, args);                
            }

            var handTemp = this.ingredients.ToList();
            //2)
            this.Clear();

            args.calledEffect = EffectType.OnBeingCookedAfter;

            //3)
            for (int i=0; i<handTemp.Count; i++)
            {
                if (handTemp[i] == null) continue;
                args.handPos = i;
                await Effect.CallEffects(handTemp[i].effects, EffectType.OnBeingCookedAfter, handTemp[i], game, args);
            }

            //4)
            for (int i = 0; i < handTemp.Count; i++)
            {
                if (handTemp[i] == null) continue;
                game.player.cookHistory.Add(handTemp[i].Copy());
            }

            game.player.curPoints += args.dishPoints;


            feedbackMsg += $" worth {args.dishPoints} points!";
            game.feedback.Add(feedbackMsg);
        }

        public async Task DestroyIngredient(Game game, int pos)
        {
            if (0 <= pos && pos < this.ingredients.Count)
            {
                if (this.ingredients[pos] == null) return;

                Ingredient ingr = this.ingredients[pos];

                EffectArgs args = new EffectArgs.DeathrattleArgs(EffectType.Deathrattle, pos, GameLocation.Hand);
                await Effect.CallEffects(ingr.effects, EffectType.Deathrattle, ingr, game, args);

                this.ingredients[pos] = null;
            }
        }

        public async Task DestroyMultipleIngredients(Game game, List<int> pos)
        {
            List<int> posFiltered = pos.FindAll(x => 0 <= x && x < this.ingredients.Count && this.ingredients[x] != null).Distinct().ToList();
            posFiltered.Sort();

            List<Ingredient> ingredients = new List<Ingredient>();

            for (int i = 0; i < posFiltered.Count; i++)
            {
                ingredients.Add(this.ingredients[posFiltered[i]].Copy());
                this.ingredients[posFiltered[i]] = null;
            }

            for (int i = 0; i < posFiltered.Count; i++)
            {
                EffectArgs args = new EffectArgs.DeathrattleArgs(EffectType.Deathrattle, posFiltered[i], GameLocation.Hand);
                await Effect.CallEffects(ingredients[i].effects, EffectType.Deathrattle, ingredients[i], game, args);
            }
        }
    }
}
