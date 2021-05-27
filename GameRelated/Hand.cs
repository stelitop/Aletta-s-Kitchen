﻿using Aletta_s_Kitchen.GameRelated.IngredientRelated;
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
        public Ingredient[] ingredients;

        public Hand()
        {
            this.ingredients = new Ingredient[3];
            this.ingredients[0] = this.ingredients[1] = this.ingredients[2] = null;
        }

        public void Clear()
        {
            this.ingredients[0] = this.ingredients[1] = this.ingredients[2] = null;
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

            for (int i=0; i<3; i++)
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

            var args = new EffectArgs.OnBeingCookedArgs(EffectType.OnBeingCookedBefore, dishPoints);

            //1)
            for (int i=0; i<3; i++)
            {
                if (this.ingredients[i] == null) continue;
                await Effect.CallEffects(this.ingredients[i].effects, EffectType.OnBeingCookedBefore, this.ingredients[i], game, args);                
            }

            var handTemp = this.ingredients.ToList();
            //2)
            this.Clear();

            args.calledEffect = EffectType.OnBeingCookedAfter;

            //3)
            for (int i=0; i<3; i++)
            {
                if (handTemp[i] == null) continue;
                await Effect.CallEffects(handTemp[i].effects, EffectType.OnBeingCookedAfter, handTemp[i], game, args);
            }

            //4)
            for (int i = 0; i < 3; i++)
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
            if (0 <= pos && pos < this.ingredients.Length)
            {
                if (this.ingredients[pos] == null) return;

                Ingredient ingr = this.ingredients[pos];

                EffectArgs args = new EffectArgs(EffectType.Deathrattle);
                await Effect.CallEffects(ingr.effects, EffectType.Deathrattle, ingr, game, args);

                this.ingredients[pos] = null;
            }
        }
    }
}
