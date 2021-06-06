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

        //Keleseth Kream

        //[GameIngredient]
        //public class KelesethKream : Ingredient
        //{
        //    public KelesethKream() : base("Keleseth Kream", 2, Rarity.Legendary, Tribe.NoTribe, "If you pick this while your kitchen has no other 2p ingredient, give all future ingredients +1 this game.")
        //    {
        //        this.effects.Add(new EF());
        //        this.glowLocation = GameLocation.Kitchen;
        //    }
        //    public override bool GlowCondition(Game game, int kitchenPos)
        //    {
        //        foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
        //        {
        //            if (ingr == this) continue;
        //            if (ingr.points == 2) return false;
        //        }
        //        return true;
        //    }
        //    private class EF : Effect
        //    {
        //        public EF() : base(EffectType.WhenPicked) { }

        //        public override Task Call(Ingredient caller, Game game, EffectArgs args)
        //        {
        //            foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
        //            {
        //                if (ingr.points == 2) return Task.CompletedTask;
        //            }

        //            game.feedback.Add("Keleseth Kream gives your future ingredients +1p.");

        //            game.RestOfGameBuff(x => true, x => { x.points++; }, false);

        //            return Task.CompletedTask;
        //        }
        //    }
        //}
    }
}
