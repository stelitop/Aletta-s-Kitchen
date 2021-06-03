using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public struct NewIngredientAddedData
    {
        public Ingredient ingredient;
        public int handPos;

        public NewIngredientAddedData(Ingredient ingredient, int handPos)
        {
            this.ingredient = ingredient;
            this.handPos = handPos;
        }
    }

    public class Hand
    {
        public int handLimit;

        private List<Ingredient> _ingredients;

        public int OptionsCount { get { return _ingredients.Count; } }
        public int NonNullOptions
        {
            get
            {
                int ret = 0;
                foreach (var ingr in _ingredients) if (ingr != null) ret++;
                return ret;
            }
        }

        public Hand()
        {
            this._ingredients = new List<Ingredient>();
            this.handLimit = 3;
        }

        public void Clear()
        {
            this._ingredients.Clear();
        }
        public Ingredient IngredientAt(int handPos)
        {
            if (0 <= handPos && handPos < this._ingredients.Count) return this._ingredients[handPos];
            else return null;
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _ingredients;
        }
        public List<Ingredient> GetAllNonNullIngredients()
        {
            List<Ingredient> ret = new List<Ingredient>();

            foreach (var ingr in _ingredients) if (ingr != null) ret.Add(ingr);

            return ret;
        }
        public void ReplaceIngredient(int pos, Ingredient newIngr)
        {
            if (0 <= pos && pos < this._ingredients.Count)
            {
                _ingredients[pos] = newIngr;
            }
        }
        public NewIngredientAddedData AddIngredient(Ingredient newIngr)
        {
            for (int i = 0; i < this._ingredients.Count; i++)
            {
                if (this._ingredients[i] == null)
                {
                    this._ingredients[i] = newIngr;
                    return new NewIngredientAddedData(this._ingredients[i], i);
                }
            }

            if (this._ingredients.Count < this.handLimit)
            {
                this._ingredients.Add(newIngr);
                return new NewIngredientAddedData(this._ingredients.Last(), this._ingredients.Count-1);
            }

            return new NewIngredientAddedData(null, -1);
        }
        public bool AvailableSpot()
        {
            for (int i = 0; i < this._ingredients.Count; i++)
            {
                if (this._ingredients[i] == null)
                {
                    return true;
                }
            }

            if (this._ingredients.Count < this.handLimit)
            {
                return true;
            }

            return false;
        }

        /**
         * Order of Operations:
         * 1) Call all OnBeingCookedBefore effects
         * 2) Remove ingredients from Hand
         * 3) Call all OnBeingCookedAfter effects
         * 4) Add the Ingredients to the Cooked history
         * 5) Trigger After You Cook effects
         */
        public async Task Cook(Game game)
        {
            game.feedback.Clear();

            int dishPoints = 0;
            List<Ingredient> allIngr = new List<Ingredient>();

            for (int i = 0; i < this._ingredients.Count; i++)
            {
                if (this._ingredients[i] == null) continue;
                dishPoints += this._ingredients[i].points;
                allIngr.Add(this._ingredients[i]);
            }

            if (allIngr.Count == 0) return;

            string feedbackMsg = "You cooked a dish made out of ";

            if (allIngr.Count == 1) feedbackMsg += $"__{allIngr[0].name}__";
            else if (allIngr.Count == 2) feedbackMsg += $"__{allIngr[0].name}__ and __{allIngr[1].name}__";
            else if (allIngr.Count == 3) feedbackMsg += $"__{allIngr[0].name}__, __{allIngr[1].name}__ and __{allIngr[2].name}__";

            var args = new EffectArgs.OnBeingCookedArgs(EffectType.OnBeingCookedBefore, dishPoints, -1);

            //1)
            for (int i = 0; i < this._ingredients.Count; i++)
            {
                if (this._ingredients[i] == null) continue;
                args.handPos = i;
                await Effect.CallEffects(this._ingredients[i].effects, EffectType.OnBeingCookedBefore, this._ingredients[i], game, args);
            }


            dishPoints = 0;
            for (int i = 0; i < this._ingredients.Count; i++)
            {
                if (this._ingredients[i] == null) continue;
                dishPoints += this._ingredients[i].points;
            }

            args.calledEffect = EffectType.OnBeingCookedAfter;
            args.dishPoints = dishPoints;

            //2)
            var handTemp = this._ingredients.ToList();
            this.Clear();
                    
            //3)
            for (int i = 0; i < handTemp.Count; i++)
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

            //5)
            foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
            {
                var afterCookArgs = new EffectArgs.AfterCookArgs(EffectType.AfterYouCook, handTemp);
                await Effect.CallEffects(ingr.effects, EffectType.AfterYouCook, ingr, game, afterCookArgs);
            }
        }
    }
}
