using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public class GameIngredients
    {
        [GameIngredient]
        public class MurlocTinyfin : Ingredient
        {
            public MurlocTinyfin() : base("Murloc Tinyfin", 1, Rarity.Common, Tribe.Murloc) { }            
        }

        [GameIngredient]
        public class MakruraPrawn : Ingredient
        {
            public MakruraPrawn() : base("Makrura Prawn", 3, Rarity.Common, Tribe.Beast) { }
        }

        [GameIngredient]
        public class Saladmander : Ingredient
        {
            public Saladmander() : base("Saladmander", 2, Rarity.Common, Tribe.Beast) { }
        }

        [GameIngredient]
        public class VanillaIceCream : Ingredient
        {
            public VanillaIceCream() : base("Vanilla Ice Cream", 1, Rarity.Common, Tribe.Elemental) { }
        }

        [GameIngredient]
        public class SushiRager : Ingredient
        {
            public SushiRager() : base("Sushi Rager", 3, Rarity.Common) { }
        }

        [GameIngredient]
        public class CoconutJuice : Ingredient
        {
            public CoconutJuice() : base("Coconut Juice", 2, Rarity.Common) {}
        }

        [GameIngredient]
        public class GruelImp : Ingredient
        {
            public GruelImp() : base("Gruel Imp", 2, Rarity.Common, Tribe.Demon, "When picked, steal 1 point from a random ingredient in your kitchen.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> cands = new List<Ingredient>();
                    for (int i=0; i<game.player.kitchen.Count; i++)
                    {
                        if (game.player.kitchen.OptionAt(i).points > 0) cands.Add(game.player.kitchen.OptionAt(i));
                    }

                    if (cands.Count == 0)
                    {
                        game.feedback.Add("Gruel Imp couldn't steal a point from anything.");
                        return Task.CompletedTask;
                    }

                    int pick = BotHandler.globalRandom.Next(cands.Count);

                    cands[pick].points--;
                    caller.points++;

                    game.feedback.Add($"Gruel Imp stole a point from {cands[pick].name}.");
                    return Task.CompletedTask;
                }
            }
        }
    
        [GameIngredient]
        public class PuddleJumper : Ingredient
        {
            public PuddleJumper() : base("Puddle Jumper", 2, Rarity.Common, Tribe.Murloc, "When picked, give all Murlocs in your dish +1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (game.player.hand.ingredients[i] == null) continue;

                        if (game.player.hand.ingredients[i].tribe == Tribe.Murloc)
                            game.player.hand.ingredients[i].points++;
                    }

                    game.feedback.Add("Puddle Jumper gave all Murlocs in your dish +1 point.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class MountainDewdrop : Ingredient
        {
            public MountainDewdrop() : base("Mountain Dewdrop", 2, Rarity.Common, Tribe.Elemental, "If you picked an Elemental last, gain +1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (game.player.pickHistory.Count == 0) return Task.CompletedTask;

                    if (game.player.pickHistory.Last().tribe == Tribe.Elemental)
                    {
                        caller.points++;
                        game.feedback.Add("Your Mountain Dewdrop gains +1 point.");
                    }
                    else
                    {
                        game.feedback.Add("Your Mountain Dewdrop fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool IsSpecialConditionFulfilled(Game game, int kitchenPos)
            {
                if (game.player.pickHistory.Count == 0) return false;
                if (game.player.pickHistory.Last().tribe == Tribe.Elemental) return true;
                return false;
            }
        }

        [GameIngredient]
        public class Creampooch : Ingredient
        {
            public Creampooch() : base("Creampooch", 2, Rarity.Common, Tribe.Elemental, "If you picked an Elemental last, give all Elementals in your kitchen +1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (game.player.pickHistory.Count == 0)
                    {
                        game.feedback.Add("Your Creampooch fails to trigger.");
                        return Task.CompletedTask;
                    }

                    if (game.player.pickHistory.Last().tribe == Tribe.Elemental)
                    {
                        var kitchen = game.player.kitchen.GetAllIngredients();
                        foreach (var ingr in kitchen)
                        {
                            if (ingr.tribe == Tribe.Elemental) ingr.points++;
                        }

                        game.feedback.Add("Your Creampooch gives all Elementals in the Kitchen +1 point.");
                    }
                    else
                    {
                        game.feedback.Add("Your Creampooch fails to trigger.");
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool IsSpecialConditionFulfilled(Game game, int kitchenPos)
            {
                if (game.player.pickHistory.Count == 0) return false;
                if (game.player.pickHistory.Last().tribe == Tribe.Elemental) return true;
                return false;
            }
        }

        [GameIngredient]
        public class BoxOfChocolate : Ingredient
        {
            public BoxOfChocolate() : base("Box of Chocolate", 0, Rarity.Common, Tribe.NoTribe, "Each turn in your kitchen, change between 0-4 points.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(1) { }

                public override Task Tick(Ingredient caller, Game game, EffectArgs args)
                {
                    caller.points = BotHandler.globalRandom.Next(5);
                    return Task.CompletedTask;
                }
            }
        }
    }
}
