using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.Game_Ingredients
{
    public class ArchivedIngredients
    {
        //Beat Root
        //[GameIngredient]
        //public class BeatRoot : Ingredient
        //{
        //    public BeatRoot() : base("Beat Root", 1, Rarity.Common, Tribe.NoTribe, "When picked, destroy all ingredients in your kitchen.")
        //    {
        //        this.effects.Add(new EF());
        //    }
        //    private class EF : Effect
        //    {
        //        public EF() : base(EffectType.WhenPicked) { }

        //        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        //        {
        //            List<int> allIndexes = new List<int>();

        //            for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
        //            {
        //                allIndexes.Add(i);
        //            }

        //            game.feedback.Add("Beat Root destroys all ingredients in your kitchen.");

        //            await game.player.kitchen.DestroyMultipleIngredients(game, allIndexes);

        //            await game.player.kitchen.FillEmptySpots(game);
        //        }
        //    }
        //}
    }
}
