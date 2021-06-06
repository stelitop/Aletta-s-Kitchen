using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.Game_Ingredients
{
    public partial class Ingredients
    {
        [GameIngredient]
        public class Heartbeet : Ingredient
        {
            public Heartbeet() : base("Heartbeet", 0, Rarity.Common, Tribe.Vegetable, "Each turn in your kitchen, gain +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(1) { }

                protected override Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    caller.points++;
                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class BeatRoot : Ingredient
        {
            public BeatRoot() : base("Beat Root", 0, Rarity.Common, Tribe.Vegetable, "Every 2 turns in your kitchen, give all ingredients in it +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(2) { }

                protected override Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    var timerArgs = args as EffectArgs.TimerArgs;

                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        ingr.points++;
                    }

                    game.feedback.Add("Beat Root gives all ingredients in the kitchen +1p.");

                    return Task.CompletedTask;
                }
            }
            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation != GameLocation.Kitchen) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

                for (int i = 0; i < this.effects.Count; i++)
                {
                    if (this.effects[i] is TimerEffect effect)
                    {
                        if (effect.currentTime == 1) ret += " (next turn)";
                        else ret += $" (in {effect.currentTime} turns)";
                        break;
                    }
                }

                return ret;
            }
        }

        [GameIngredient]
        public class ChestNut : Ingredient
        {
            public ChestNut() : base("Chest Nut", 0, Rarity.Rare, Tribe.Vegetable, "Every 3 turns in your kitchen, add 9 points to your total score.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(3) { }

                protected override Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    game.player.curPoints += 9;

                    game.feedback.Add("Chest Nut adds 9p to your total score.");

                    return Task.CompletedTask;
                }
            }
            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation != GameLocation.Kitchen) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

                for (int i = 0; i < this.effects.Count; i++)
                {
                    if (this.effects[i] is TimerEffect effect)
                    {
                        if (effect.currentTime == 1) ret += " (next turn)";
                        else ret += $" (in {effect.currentTime} turns)";
                        break;
                    }
                }

                return ret;
            }
        }

        [GameIngredient]
        public class HotPotato : Ingredient
        {
            public HotPotato() : base("Hot Potato", 0, Rarity.Epic, Tribe.Vegetable, "After 12 turns in your kitchen, destroy this and give all ingredients +2p this game.")
            {
                this.effects.Add(new EF());
            }

            private class EF : TimerEffect
            {
                public EF() : base(12) { }

                protected override async Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    game.feedback.Add("Hot Potato gives all ingredients this game +2p.");

                    game.RestOfGameBuff(x => true, x => { x.points += 2; });

                    var timerArgs = args as EffectArgs.TimerArgs;
                    await game.player.kitchen.DestroyIngredient(game, timerArgs.kitchenPos);
                }
            }

            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation != GameLocation.Kitchen) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

                for (int i = 0; i < this.effects.Count; i++)
                {
                    if (this.effects[i] is TimerEffect effect)
                    {
                        if (effect.currentTime == 1) ret += " (next turn)";
                        else ret += $" (in {effect.currentTime} turns)";
                        break;
                    }
                }

                return ret;
            }
        }        
    }
}
