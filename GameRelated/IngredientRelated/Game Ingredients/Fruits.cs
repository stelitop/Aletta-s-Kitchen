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
        public class CoconutJuice : Ingredient
        {
            public CoconutJuice() : base("Coconut Juice", 1, Rarity.Common, Tribe.Fruit, "Cook: Give Fruits +1 this game.") 
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.tribe == Tribe.Fruit && x != caller, x => { x.points++; });

                    game.feedback.Add("Coconut Juice gives your Fruits this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class Warmelon : Ingredient
        {
            public Warmelon() : base("Warmelon", 1, Rarity.Common, Tribe.Fruit, "Cook: Destroy all ingredients in your kitchen. The new ones to enter have +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> destr = new List<int>();
                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++) destr.Add(i);

                    game.feedback.Add("Warmelon destroys your kitchen.");

                    await game.player.kitchen.DestroyMultipleIngredients(game, destr);

                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        ingr.points++;
                    }

                    game.feedback.Add("Warmelon gives the new ingredients +1p.");
                }
            }
        }

        [GameIngredient]
        public class CherryBomb : Ingredient
        {
            public CherryBomb() : base("Cherry Bomb", 3, Rarity.Common, Tribe.Fruit, "In 4 turns in your kitchen, destroy all ingredients in it.")
            {
                this.effects.Add(new EF());
            }
            private class EF : TimerEffect
            {
                public EF() : base(4) { }

                protected override async Task Trigger(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> allIndexes = new List<int>();

                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
                    {
                        allIndexes.Add(i);
                    }

                    game.feedback.Add("Cherry Bomb destroys all ingredients in your kitchen.");

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
        public class YserasDragonfruit : Ingredient
        {
            public YserasDragonfruit() : base("Ysera's Dragonfruit", 3, Rarity.Common, Tribe.Fruit, "When picked, destroy all ingredients in your kitchen except Dragons.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var indexes = game.player.kitchen.GetAllIndexes().FindAll(x => game.player.kitchen.OptionAt(x).tribe != Tribe.Dragon);
                    game.feedback.Add("Ysera's Dragonfruit destroys all non-Dragons in your kitchen.");
                    await game.player.kitchen.DestroyMultipleIngredients(game, indexes);
                }
            }
        }

        [GameIngredient]
        public class Fruitreant : Ingredient
        {
            public Fruitreant() : base("Fruitreant", 2, Rarity.Common, Tribe.Fruit) { }
        }
        
        [GameIngredient]
        public class FaerieStarfruit : Ingredient
        {
            public FaerieStarfruit() : base("Faerie Starfruit", 0, Rarity.Rare, Tribe.Fruit, "When this enters your kitchen, give a random ingredient in it +2p and destroy this.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.NextIngredient;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return true;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnEnteringKitchen) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    EffectArgs.EnteringKitchenArgs enterArgs = args as EffectArgs.EnteringKitchenArgs;

                    var cands = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x != caller);
                    Ingredient pick = cands[BotHandler.globalRandom.Next(cands.Count)];
                    pick.points += 2;
                    game.feedback.Add($"Faerie Starfruit gives {pick.name} +2p and destroys itself.");
                    await game.player.kitchen.DestroyIngredient(game, enterArgs.kitchenPos);
                }
            }
        }

        [GameIngredient]
        public class Mineapple : Ingredient
        {
            public Mineapple() : base("Mineapple", 3, Rarity.Rare, Tribe.Fruit, "If destroyed, add 9p to your total score.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.Deathrattle) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.player.curPoints += 9;

                    game.feedback.Add("Mineapple adds +9p to your total score.");

                    return Task.CompletedTask;
                }
            }                
        }

        [GameIngredient]
        public class Squash : Ingredient
        {
            public Squash() : base("Squash", 3, Rarity.Rare, Tribe.Fruit, "When picked, destroy all ingredients with less points. Gain +1 for each.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> toBeDestroyed = new List<int>();

                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) != null)
                        {
                            if (game.player.kitchen.OptionAt(i).points < caller.points) toBeDestroyed.Add(i);
                        }
                    }

                    if (toBeDestroyed.Count > 0)
                    {
                        game.feedback.Add($"Squash destroys all ingredients with less points and gains +{toBeDestroyed.Count}p.");

                        caller.points += toBeDestroyed.Count;

                        await game.player.kitchen.DestroyMultipleIngredients(game, toBeDestroyed);
                    }
                }
            }
        }

        [GameIngredient]
        public class Pear : Ingredient
        {
            public Pear() : base("Pear", 2, Rarity.Epic, Tribe.Fruit, "Cook: If your kitchen or dish has a pair, double their points.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                Dictionary<string, int> pairs = new Dictionary<string, int>();

                foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                {
                    if (ingr == this) continue;

                    if (pairs.ContainsKey(ingr.name)) pairs[ingr.name]++;
                    else pairs.Add(ingr.name, 1);
                }

                foreach (var ingr in game.player.hand.GetAllNonNullIngredients())
                {
                    if (ingr == this) continue;

                    if (pairs.ContainsKey(ingr.name)) pairs[ingr.name]++;
                    else pairs.Add(ingr.name, 1);
                }

                List<string> cands = new List<string>();

                foreach (var name in pairs) if (name.Value >= 2) cands.Add(name.Key);

                return cands.Count > 0;                
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    Dictionary<string, int> pairs = new Dictionary<string, int>();

                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr == caller) continue;
                             
                        if (pairs.ContainsKey(ingr.name)) pairs[ingr.name]++;
                        else pairs.Add(ingr.name, 1);
                    }

                    foreach (var ingr in game.player.hand.GetAllNonNullIngredients())
                    {
                        if (ingr == caller) continue;

                        if (pairs.ContainsKey(ingr.name)) pairs[ingr.name]++;
                        else pairs.Add(ingr.name, 1);
                    }

                    List<string> cands = new List<string>();

                    foreach (var name in pairs) if (name.Value >= 2) cands.Add(name.Key);

                    if (cands.Count == 0) return Task.CompletedTask;

                    string pick = cands[BotHandler.globalRandom.Next(cands.Count)];
                    List<Ingredient> matches = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.name == pick).Concat(game.player.hand.GetAllNonNullIngredients().FindAll(x => x.name == pick && x != caller)).ToList();
                    var randomList = matches.OrderBy(x => BotHandler.globalRandom.Next()).ToList();
                    if (randomList.Count < 2) return Task.CompletedTask;

                    randomList[0].points *= 2;
                    randomList[1].points *= 2;

                    game.feedback.Add($"Pear doubles the points of a pair of {pick}.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class MalygooseBerry : Ingredient
        {
            public MalygooseBerry() : base("Malygoose Berry", 2, Rarity.Epic, Tribe.Fruit, "Cook: If your dish has a Dragon and Beast, give both tribes +2p this game.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Any;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                bool beast = false, dragon = false;

                foreach (var ingr in game.player.hand.GetAllNonNullIngredients())
                {
                    if (ingr.tribe == Tribe.Beast) beast = true;
                    if (ingr.tribe == Tribe.Dragon) dragon = true;
                }

                return beast && dragon;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    bool beast = false, dragon = false;

                    foreach (var ingr in game.player.hand.GetAllNonNullIngredients())
                    {
                        if (ingr.tribe == Tribe.Beast) beast = true;
                        if (ingr.tribe == Tribe.Dragon) dragon = true;
                    }

                    if (!beast || !dragon) return Task.CompletedTask;

                    game.RestOfGameBuff(x => x.tribe == Tribe.Beast || x.tribe == Tribe.Dragon, x => { x.points += 2; });

                    game.feedback.Add("Malygoose Berry gives a yours Beasts and Dragons this game +2p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class GrapesOfWrath : Ingredient
        {
            public GrapesOfWrath() : base("Grapes of Wrath", 3, Rarity.Legendary, Tribe.Fruit, "When picked, destroy and replace the kitchen with one that has a Grapes of Wrath.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> pos = new List<int> { 0, 1, 2, 3, 4 };
                    int reserved = BotHandler.globalRandom.Next(5);
                    //pos.RemoveAt(reserved);                    

                    game.feedback.Add("Grapes of Wrath destroys and replaces the kitchen with one with Grapes of Wrath.");

                    await game.player.kitchen.DestroyMultipleIngredients(game, pos);
                    game.player.kitchen.ReplaceIngredient(reserved, game.pool.GetVanillaIngredient("Grapes of Wrath"));
                }
            }
        }

        [GameIngredient]
        public class CherryOnTop : Ingredient
        {
            public CherryOnTop() : base("Cherry on Top", 1, Rarity.Common, Tribe.Fruit, "Whenever an ingredient enters your kitchen, give it +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WheneverIngredientEntersKitchen) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var enterArgs = args as EffectArgs.EnteringKitchenArgs;

                    if (game.player.kitchen.OptionAt(enterArgs.kitchenPos) == null) return Task.CompletedTask;

                    game.player.kitchen.OptionAt(enterArgs.kitchenPos).points++;
                    game.feedback.Add($"Cherry on Top gives {game.player.kitchen.OptionAt(enterArgs.kitchenPos).name} +1p.");

                    return Task.CompletedTask;
                }
            }
        }
    }
}
