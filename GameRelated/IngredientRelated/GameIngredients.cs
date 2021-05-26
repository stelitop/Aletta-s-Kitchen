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
                this.glowLocation = GameLocation.Kitchen;
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

            public override bool GlowCondition(Game game, int kitchenPos)
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
                this.glowLocation = GameLocation.Kitchen;
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

            public override bool GlowCondition(Game game, int kitchenPos)
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

                protected override Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    caller.points = BotHandler.globalRandom.Next(5);
                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class SteamscaleSoba : Ingredient
        {
            public SteamscaleSoba() : base("Steamscale Soba", 1, Rarity.Common, Tribe.Dragon, "Cook: If your kitchen has a Dragon, gain +1.")
            {
                this.glowLocation = GameLocation.Hand;
                this.effects.Add(new EF());
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.kitchen.GetAllIngredients())
                {
                    if (ingr.tribe == Tribe.Dragon)
                    {
                        return true;
                    }
                }
                return false;
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var curArgs = args as EffectArgs.OnBeingCookedArgs;

                    bool hasDragon = false;
                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr.tribe == Tribe.Dragon)
                        {
                            hasDragon = true;
                            break;
                        }
                    }

                    if (hasDragon)
                    {
                        game.player.curPoints++;
                        game.feedback.Add("Steamscale Soba gives you +1 point.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class StovebellyGriller : Ingredient
        {
            public StovebellyGriller() : base("Stovebelly Griller", 2, Rarity.Rare, Tribe.Dragon, "Cook: If your kitchen has a Dragon, gain +3.")
            {
                this.glowLocation = GameLocation.Hand;
                this.effects.Add(new EF());
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.kitchen.GetAllIngredients())
                {
                    if (ingr.tribe == Tribe.Dragon)
                    {
                        return true;
                    }
                }
                return false;
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var curArgs = args as EffectArgs.OnBeingCookedArgs;

                    bool hasDragon = false;
                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr.tribe == Tribe.Dragon)
                        {
                            hasDragon = true;
                            break;
                        }
                    }

                    if (hasDragon)
                    {
                        game.player.curPoints+=3;
                        game.feedback.Add("Stovebelly Griller gives you +3 points.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class WasabiSpice : Ingredient
        {
            public WasabiSpice() : base("Wasabi Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Dragons this game +1.") 
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Dragon) ingr.points++;
                    }
                    if (game.player.kitchen.nextOption != null)
                    {
                        if (game.player.kitchen.nextOption.tribe == Tribe.Dragon) game.player.kitchen.nextOption.points++;
                    }
                    foreach (var ingr in game.pool.ingredients)
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Dragon) ingr.points++;
                    }

                    game.feedback.Add("Wasabi Spice gives your Dragons this game +1 point.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class WildberrySpice : Ingredient
        {
            public WildberrySpice() : base("Wildberry Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Beasts this game +1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points++;
                    }
                    if (game.player.kitchen.nextOption != null)
                    {
                        if (game.player.kitchen.nextOption.tribe == Tribe.Beast) game.player.kitchen.nextOption.points++;
                    }
                    foreach (var ingr in game.pool.ingredients)
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points++;
                    }

                    game.feedback.Add("Wildberry Spice gives your Beasts this game +1 point.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class CalamariOfNZoth : Ingredient
        {
            public CalamariOfNZoth() : base("Calamari of N'Zoth", 2, Rarity.Legendary, Tribe.NoTribe, "Cook: If your dish has exactly 8 points, give +8 to all ingredients in your kitchen.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (args.dishPoints == 8)
                    {
                        foreach (var ingr in game.player.kitchen.GetAllIngredients())
                        {
                            if (ingr == null) continue;
                            ingr.points += 8;
                        }

                        game.feedback.Add("Your Calamari of N'Zoth gives +8p to all ingredients in your kitchen.");
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                int points = 0;
                foreach (var ingr in game.player.hand.ingredients)
                {
                    if (ingr == null) continue;
                    points += ingr.points;
                }
                return (points == 8);
            }
        }

        [GameIngredient]
        public class CaptainCookie : Ingredient
        {
            public CaptainCookie() : base("Captain Cookie", 3, Rarity.Legendary, Tribe.Murloc, "Cook: If your dish is all Murlocs, double its total score.")
            {
                this.glowLocation = GameLocation.Hand;
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    bool cond = true;

                    foreach (var ingr in game.player.hand.ingredients)
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe != Tribe.Murloc)
                        {
                            cond = false;
                            break;
                        }
                    }

                    if (cond)
                    {
                        var spArgs = args as EffectArgs.OnBeingCookedArgs;

                        spArgs.dishPoints *= 2;

                        game.feedback.Add("Captain Cookie doubles the score of your dish.");
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.hand.ingredients)
                {
                    if (ingr == null) continue;
                    if (ingr.tribe != Tribe.Murloc)
                    {
                        return false;                        
                    }
                }
                return true;
            }
        }
    
        [GameIngredient]
        public class Asparagos : Ingredient
        {
            public Asparagos() : base("Asparagos", 10, Rarity.Legendary, Tribe.Dragon, "Can only be picked if you've Cooked a dragon this game.") 
            {
                this.glowLocation = GameLocation.Kitchen;    
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return (game.player.cookHistory.FindAll(x => x.tribe == Tribe.Dragon).Count > 0);
            }
            public override bool CanBeBought(Game game, int kitchenPos)
            {
                return this.GlowCondition(game, kitchenPos);
            }
        }
    
        [GameIngredient]
        public class Fruitreant : Ingredient
        {
            public Fruitreant() : base("Fruitreant", 0, Rarity.Common, Tribe.NoTribe, "Every 1 turn in your kitchen, gain +1.")
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
    }
}
