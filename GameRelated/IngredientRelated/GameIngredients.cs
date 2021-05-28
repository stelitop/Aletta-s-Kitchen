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
            public GruelImp() : base("Gruel Imp", 2, Rarity.Common, Tribe.Demon, "When picked, steal 1p from a random ingredient in your kitchen.")
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
                        if (game.player.kitchen.OptionAt(i) == null) continue;
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
            public PuddleJumper() : base("Puddle Jumper", 2, Rarity.Common, Tribe.Murloc, "When picked, give all other Murlocs in your dish +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    for (int i = 0; i < game.player.hand.ingredients.Count; i++)
                    {
                        if (game.player.hand.ingredients[i] == null) continue;
                        if (game.player.hand.ingredients[i] == caller) continue;

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
            public MountainDewdrop() : base("Mountain Dewdrop", 2, Rarity.Common, Tribe.Elemental, "If you picked an Elemental last, gain +1p.")
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
            public Creampooch() : base("Creampooch", 2, Rarity.Common, Tribe.Elemental, "If you picked an Elemental last, give all ingredients in your kitchen +1p.")
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
                        return Task.CompletedTask;
                    }

                    if (game.player.pickHistory.Last().tribe == Tribe.Elemental)
                    {
                        var kitchen = game.player.kitchen.GetAllIngredients();
                        foreach (var ingr in kitchen)
                        {
                            if (ingr == null) continue;
                            ingr.points++;
                        }

                        game.feedback.Add("Your Creampooch gives all ingredients in the Kitchen +1p.");
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
            public BoxOfChocolate() : base("Box of Chocolate", 0, Rarity.Common, Tribe.NoTribe, "Each turn in your kitchen, change between 0-4p.")
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
            public SteamscaleSoba() : base("Steamscale Soba", 1, Rarity.Common, Tribe.Dragon, "Cook: If your kitchen has a Dragon, gain +1p.")
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
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

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
                        cookArgs.dishPoints++;
                        caller.points++;
                        game.feedback.Add("Steamscale Soba gains +1p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class StovebellyGriller : Ingredient
        {
            public StovebellyGriller() : base("Stovebelly Griller", 2, Rarity.Rare, Tribe.Dragon, "Cook: If your kitchen has a Dragon, gain +3p.")
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
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

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
                        cookArgs.dishPoints+=3;
                        caller.points+=3;
                        game.feedback.Add("Steamscale Soba gains +3p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class WasabiSpice : Ingredient
        {
            public WasabiSpice() : base("Wasabi Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Dragons this game +1p.") 
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

                    game.feedback.Add("Wasabi Spice gives your Dragons this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class WildberrySpice : Ingredient
        {
            public WildberrySpice() : base("Wildberry Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Beasts this game +1p.")
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

                    game.feedback.Add("Wildberry Spice gives your Beasts this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class CalamariOfNZoth : Ingredient
        {
            public CalamariOfNZoth() : base("Calamari of N'Zoth", 2, Rarity.Legendary, Tribe.NoTribe, "Cook: If your dish has exactly 8p, give +8p to all ingredients in your kitchen.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

                    if (cookArgs.dishPoints == 8)
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
            public Fruitreant() : base("Fruitreant", 0, Rarity.Common, Tribe.NoTribe, "Each turn in your kitchen, gain +1p.")
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
        public class BakusMooncake : Ingredient
        {
            public BakusMooncake() : base("Baku's Mooncake", 9, Rarity.Legendary, Tribe.NoTribe, "Can only be played if every ingredient in your kitchen is odd-point.")
            {
                this.glowLocation = GameLocation.Kitchen;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.kitchen.GetAllIngredients())
                {
                    if (ingr.points % 2 == 0) return false;
                }
                return true;
            }

            public override bool CanBeBought(Game game, int kitchenPos) => this.GlowCondition(game, kitchenPos);            
        }

        [GameIngredient]
        public class KeywordSoup : Ingredient
        {
            public KeywordSoup() : base("Keyword Soup", 2, Rarity.Rare, Tribe.NoTribe, "When picked, gain +1p for each different tribe you've cooked this game.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    HashSet<Tribe> tribes = new HashSet<Tribe>();

                    foreach (var ingr in game.player.cookHistory)
                    {
                        if (ingr.tribe != Tribe.NoTribe && !tribes.Contains(ingr.tribe)) tribes.Add(ingr.tribe);
                    }

                    caller.points += tribes.Count;

                    return Task.CompletedTask;
                }
            }

            public override string GetDescriptionText(Game game)
            {
                HashSet<Tribe> tribes = new HashSet<Tribe>();

                foreach (var ingr in game.player.cookHistory)
                {
                    if (ingr.tribe != Tribe.NoTribe && !tribes.Contains(ingr.tribe)) tribes.Add(ingr.tribe);
                }

                return base.GetDescriptionText(game) + $" ({tribes.Count})";
            }
        }

        [GameIngredient] 
        public class BeatRoot : Ingredient
        {
            public BeatRoot() : base("Beat Root", 1, Rarity.Common, Tribe.NoTribe, "When picked, destroy all ingredients in your kitchen.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> allIndexes = new List<int>();

                    for (int i = 0; i < game.player.kitchen.Count; i++)
                    {
                        allIndexes.Add(i);
                    }

                    game.feedback.Add("Beat Root destroys all ingredients in your kitchen.");

                    await game.player.kitchen.DestroyMultipleIngredients(game, allIndexes);

                    await game.player.kitchen.FillEmptySpots(game);
                }
            }
        }
    
        [GameIngredient]
        public class ChestNut : Ingredient
        {
            public ChestNut() : base("Chest Nut", 1, Rarity.Rare, Tribe.NoTribe, "Deathrattle: Give +5p to all ingredients in your dish.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.Deathrattle) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.hand.ingredients)
                    {
                        if (ingr == null) continue;
                        ingr.points += 5;
                    }

                    game.feedback.Add("Chest Nut gives +5p to all ingredients in your dish.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class TheCodfather : Ingredient
        {
            public TheCodfather() : base("The Codfather", 3, Rarity.Legendary, Tribe.Beast, "Deathrattle: Give this ingredient's points to all Beasts this game.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.Deathrattle) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr == null) continue;
                        if (caller == ingr) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points += caller.points;
                    }
                    foreach (var ingr in game.player.hand.ingredients)
                    {
                        if (ingr == null) continue;
                        if (caller == ingr) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points += caller.points;
                    }
                    if (game.player.kitchen.nextOption != null)
                    {
                        if (game.player.kitchen.nextOption.tribe == Tribe.Beast) game.player.kitchen.nextOption.points += caller.points;
                    }
                    foreach (var ingr in game.pool.ingredients)
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points += caller.points;
                    }

                    game.feedback.Add($"The Codfather gives your Beasts this game +{caller.points}p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class ImpDigestible : Ingredient
        {
            public ImpDigestible() : base("Imp-Digestible", 1, Rarity.Rare, Tribe.Demon, "Deathrattle: Add +7p to your total score.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.Deathrattle) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.player.curPoints += 7;

                    game.feedback.Add("Imp-Digestible gives your total score +7p.");

                    return Task.CompletedTask;
                }
            }
        }
    
        [GameIngredient]
        public class DelicacyOfArgus : Ingredient
        {
            public DelicacyOfArgus() : base("Delicacy of Argus", 4, Rarity.Common, Tribe.NoTribe, "Cook: Give adjacent ingredients +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

                    int prev = cookArgs.handPos-1, next = cookArgs.handPos+1;
                    int total = 0;

                    if (0 <= prev && prev < game.player.hand.ingredients.Count)
                    {
                        if (game.player.hand.ingredients[prev] != null)
                        {
                            game.player.hand.ingredients[prev].points++;
                            cookArgs.dishPoints++;
                            total++;
                        }
                    }

                    if (0 <= next && next < game.player.hand.ingredients.Count)
                    {
                        if (game.player.hand.ingredients[next] != null)
                        {
                            game.player.hand.ingredients[next].points++;
                            cookArgs.dishPoints++;
                            total++;
                        }
                    }

                    game.feedback.Add($"Delicacy of Argus gives adjacent ingredients +1p, total of +{total}p.");

                    return Task.CompletedTask;
                }
            }
        }
    
        [GameIngredient]
        public class BigMac : Ingredient
        {
            public BigMac() : base("Big Mac", 5, Rarity.Common, Tribe.NoTribe, "When picked, replace a random ingredient in your kitchen with a 1p Small Fry.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> cands = new List<int>();
                    for (int i=0; i<game.player.kitchen.Count; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) != null)
                        {
                            cands.Add(i);
                        }
                    }

                    int replace = cands[BotHandler.globalRandom.Next(cands.Count)];
                    string name = game.player.kitchen.OptionAt(replace).name;

                    game.player.kitchen.ReplaceIngredient(replace, new SmallFry());

                    game.feedback.Add($"Big Mac replaces {name} in your Kitchen with a Small Fry.");

                    return Task.CompletedTask;
                }
            }
            public class SmallFry : Ingredient
            {
                public SmallFry() : base("Small Fry", 1, Rarity.Common, Tribe.NoTribe) { }
            }
        }

        [GameIngredient]
        public class RisingDough : Ingredient
        {
            public RisingDough() : base("Rising Dough", 0, Rarity.Common, Tribe.NoTribe, "Every 2 turns in your kitchen, give this and its neighbors +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(2) { }

                protected override Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    var timerArgs = args as EffectArgs.TimerArgs;

                    caller.points++;
                    int prev = timerArgs.kitchenPos - 1, next = timerArgs.kitchenPos + 1;

                    Ingredient prevIng = game.player.kitchen.OptionAt(prev), nextIng = game.player.kitchen.OptionAt(next);

                    if (prevIng != null) prevIng.points++;
                    if (nextIng != null) nextIng.points++;

                    return Task.CompletedTask;
                }
            }
            public override string GetDescriptionText(Game game)
            {
                string ret = base.GetDescriptionText(game);

                for (int i=0; i<this.effects.Count; i++)
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
        public class BombAppetit : Ingredient
        {
            public BombAppetit() : base("Bomb Appetit", 3, Rarity.Epic, Tribe.NoTribe, "In 4 turns in your kitchen, destroy all ingredients in it.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(4) { }

                protected override async Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {                    
                    List<int> allIndexes = new List<int>();
                    
                    for (int i=0; i<game.player.kitchen.Count; i++)
                    {
                        allIndexes.Add(i);
                    }

                    game.feedback.Add("Bomb Appetit destroys all ingredients in your kitchen.");

                    await game.player.kitchen.DestroyMultipleIngredients(game, allIndexes);

                    await game.player.kitchen.FillEmptySpots(game);
                }
            }
            public override string GetDescriptionText(Game game)
            {
                string ret = base.GetDescriptionText(game);

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

        //[GameIngredient]
        //public class GrapesOfWrath : Ingredient
        //{
        //    public GrapesOfWrath() : base("Grapes of Wrath", 2, Rarity.Epic, Tribe.NoTribe, "When this enters the kitchen, destroy all ingredients with less points. Gain +1 for each.")
        //    {
        //        this.effects.Add(new EF());
        //    }
        //    private class EF : Effect
        //    {
        //        public EF() : base(EffectType.OnEnteringKitchen) { }

        //        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        //        {
        //            List<int> toBeDestroyed = new List<int>();

        //            for (int i=0; i<game.player.kitchen.Count; i++)
        //            {
        //                if (game.player.kitchen.OptionAt(i) != null)
        //                {
        //                    if (game.player.kitchen.OptionAt(i).points < caller.points) toBeDestroyed.Add(i);
        //                }
        //            }

        //            if (toBeDestroyed.Count > 0)
        //            {
        //                game.feedback.Add($"Grapes of Wrath destroys all ingredients with less points and gains +{toBeDestroyed.Count}p.");

        //                caller.points += toBeDestroyed.Count;

        //                await game.player.kitchen.DestroyMultipleIngredients(game, toBeDestroyed);

        //                await game.player.kitchen.FillEmptySpots(game);
        //            }
        //        }
        //    }
        //}
    }
}
