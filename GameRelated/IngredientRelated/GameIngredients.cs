using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public partial class GameIngredients
    {        
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
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
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
                    if (ingr == null) continue;

                    if (ingr.tribe == Tribe.Dragon) return true;                    
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
                    if (ingr == null) continue;

                    if (ingr.tribe == Tribe.Dragon) return true;                    
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
            public CalamariOfNZoth() : base("Calamari of N'Zoth", 2, Rarity.Legendary, Tribe.Beast, "Cook: If your dish has exactly 8p, give +8p to all ingredients in your kitchen.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }

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
                foreach (var ingr in game.player.hand.GetAllIngredients())
                {
                    if (ingr == null) continue;
                    points += ingr.points;
                }
                return (points == 8);
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
                    if (ingr == null) continue;

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

            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation == GameLocation.Hand) return base.GetDescriptionText(game, gameLocation);

                HashSet<Tribe> tribes = new HashSet<Tribe>();

                foreach (var ingr in game.player.cookHistory)
                {
                    if (ingr.tribe != Tribe.NoTribe && !tribes.Contains(ingr.tribe)) tribes.Add(ingr.tribe);
                }

                return base.GetDescriptionText(game, gameLocation) + $" ({tribes.Count})";
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

                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
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
                    foreach (var ingr in game.player.hand.GetAllIngredients())
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
                    foreach (var ingr in game.player.hand.GetAllIngredients())
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

                    if (0 <= prev && prev < game.player.hand.OptionsCount)
                    {
                        if (game.player.hand.IngredientAt(prev) != null)
                        {
                            game.player.hand.IngredientAt(prev).points++;
                            cookArgs.dishPoints++;
                            total++;
                        }
                    }

                    if (0 <= next && next < game.player.hand.OptionsCount)
                    {
                        if (game.player.hand.IngredientAt(next) != null)
                        {
                            game.player.hand.IngredientAt(next).points++;
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
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
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
            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation != GameLocation.Kitchen) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

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
                    
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        allIndexes.Add(i);
                    }

                    game.feedback.Add("Bomb Appetit destroys all ingredients in your kitchen.");

                    await game.player.kitchen.DestroyMultipleIngredients(game, allIndexes);

                    await game.player.kitchen.FillEmptySpots(game);
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
        public class GrapesOfWrath : Ingredient
        {
            public GrapesOfWrath() : base("Grapes of Wrath", 2, Rarity.Epic, Tribe.NoTribe, "When this enters the kitchen, destroy all ingredients with less points. Gain +1 for each.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnEnteringKitchen) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> toBeDestroyed = new List<int>();

                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) != null)
                        {
                            if (game.player.kitchen.OptionAt(i).points < caller.points) toBeDestroyed.Add(i);
                        }
                    }

                    if (toBeDestroyed.Count > 0)
                    {
                        game.feedback.Add($"Grapes of Wrath destroys all ingredients with less points and gains +{toBeDestroyed.Count}p.");

                        caller.points += toBeDestroyed.Count;

                        await game.player.kitchen.DestroyMultipleIngredients(game, toBeDestroyed);
                    }
                }
            }
        }

        [GameIngredient]
        public class JadeLotus : Ingredient
        {
            public JadeLotus() : base("Jade Lotus", 1, Rarity.Common, Tribe.NoTribe, "When picked, gain +2p for each copy of this you picked this game.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int copies = game.player.pickHistory.FindAll(x => x.name == "Jade Lotus").Count();

                    caller.points += copies*2;

                    return Task.CompletedTask;
                }                
            }
            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation == GameLocation.Hand) return base.GetDescriptionText(game, gameLocation);

                int copies = game.player.pickHistory.FindAll(x => x.name == "Jade Lotus").Count();

                return base.GetDescriptionText(game, gameLocation) + $" ({copies})";
            }
        }        
    
        [GameIngredient]
        public class ShrimpshellBento : Ingredient
        {
            public ShrimpshellBento() : base("Shrimpshell Bento", 1, Rarity.Common, Tribe.NoTribe, "When this enters your kitchen, destroy a random Beast in it to gain +2.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnEnteringKitchen) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> candidates = new List<int>();

                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) == null) continue;
                        if (game.player.kitchen.OptionAt(i).tribe == Tribe.Beast)
                        {
                            candidates.Add(i);
                        }
                    }

                    if (candidates.Count == 0)
                    {
                        return;
                    }

                    int destroyedIndex = candidates[BotHandler.globalRandom.Next(candidates.Count)];

                    game.feedback.Add($"Shrimshell Bento destroys {game.player.kitchen.OptionAt(destroyedIndex).name} and gains +2p.");
                    caller.points += 2;

                    await game.player.kitchen.DestroyIngredient(game, destroyedIndex);
                }
            }
        }
    
        [GameIngredient]
        public class HotpotHydra : Ingredient
        {
            public HotpotHydra() : base("Hotpot Hydra", 2, Rarity.Epic, Tribe.Beast, "Deathrattle: Double the points of ingredients in your kitchen.")
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

                        ingr.points *= 2;
                    }

                    return Task.CompletedTask;
                }
            }
        }        

        [GameIngredient]
        public class IcecreamConenberg : Ingredient
        {
            public IcecreamConenberg() : base("Icecream Conenberg", 0, Rarity.Legendary, Tribe.Elemental, "When picked, gain +5p for each Elemental you picked in a row before this.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int depth = 1;

                    while (game.player.pickHistory.Count >= depth)
                    {
                        if (game.player.pickHistory[game.player.pickHistory.Count - depth].tribe == Tribe.Elemental)
                        {
                            depth++;
                        }
                        else break;
                    }

                    caller.points += 5 * (depth-1); 

                    return Task.CompletedTask;
                }
            }

            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation == GameLocation.Hand) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

                int depth = 1;

                while (game.player.pickHistory.Count >= depth)
                {
                    if (game.player.pickHistory[game.player.pickHistory.Count - depth].tribe == Tribe.Elemental)
                    {
                        depth++;
                    }
                    else break;
                }

                return ret + $" ({depth-1})";
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                if (game.player.pickHistory.Count == 0) return false;
                if (game.player.pickHistory.Last().tribe == Tribe.Elemental) return true;
                return false;
            }
        }        

        [GameIngredient]
        public class HorseRadish : Ingredient
        {
            public HorseRadish() : base("Horse Radish", 2, Rarity.Common, Tribe.Beast, "If you pick this the turn it enters your kitchen, gain +2p.")
            {
                this.glowLocation = GameLocation.Kitchen;

                this.effects.Add(new EF());
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    Console.WriteLine($"{caller.roundEntered} - {game.curRound}");

                    if (caller.roundEntered == game.curRound) caller.points += 2;

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return this.roundEntered == game.curRound;
            }
        }

        [GameIngredient]
        public class TaterTinytots : Ingredient
        {
            public TaterTinytots() : base("Tater Tinytots", 1, Rarity.Epic, Tribe.NoTribe, "When picked, give all 1-point ingredients in your kitchen +3p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    bool trig = false;

                    foreach (var ingr in game.player.kitchen.GetAllIngredients())
                    {
                        if (ingr == null) continue;

                        if (ingr.points == 1)
                        {
                            ingr.points += 3;
                            trig = true;
                        }
                    }

                    if (trig) game.feedback.Add("Tater Tinytots gives your 1-point ingredients in your kitchen +3p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class MatchaCoffee : Ingredient
        {
            public MatchaCoffee() : base("Matcha Coffee", 1, Rarity.Epic, Tribe.NoTribe, "When picked, gain points to match the highest in your dish.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int maxP = 0;

                    foreach (var ingr in game.player.hand.GetAllIngredients())
                    {
                        if (ingr == null) continue;

                        maxP = Math.Max(maxP, ingr.points);
                    }

                    caller.points = maxP;

                    return Task.CompletedTask;
                }
            }
        }
    
        [GameIngredient]
        public class MenagerieMayonnaise : Ingredient
        {
            public MenagerieMayonnaise() : base("Menagerie Mayonnaise", 3, Rarity.Rare, Tribe.NoTribe, "When this enters your kitchen, give a random ingredient of each type +3.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnEnteringKitchen) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (Tribe type in Enum.GetValues(typeof(Tribe)))
                    {
                        if (type == Tribe.NoTribe) continue;

                        var successes = game.player.kitchen.GetAllIngredients().FindAll(x => this.Predicate(x, type));

                        if (successes.Count == 0) continue;

                        successes[BotHandler.globalRandom.Next(successes.Count)].points += 3;
                    }

                    game.feedback.Add("Menagerie Mayonnaise gives an ingredient of each type +3p.");

                    return Task.CompletedTask;
                }

                private bool Predicate(Ingredient ingr, Tribe type)
                {
                    if (ingr == null) return false;

                    return ingr.tribe == type;
                }
            }
        }
    }
}
