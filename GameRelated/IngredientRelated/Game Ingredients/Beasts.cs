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
        public class WildberrySpice : Ingredient
        {
            public WildberrySpice() : base("Wildberry Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Beasts this game +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.tribe == Tribe.Beast, x => { x.points++; });

                    game.feedback.Add("Wildberry Spice gives your Beasts this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class Saladmander : Ingredient
        {
            public Saladmander() : base("Saladmander", 1, Rarity.Common, Tribe.Beast, "When picked, destroy a random Fruit in your kitchen to gain +3p.") 
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> candidates = new List<int>();

                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) == null) continue;
                        if (game.player.kitchen.OptionAt(i).tribe == Tribe.Fruit) candidates.Add(i);
                    }

                    if (candidates.Count == 0) return;

                    int index = candidates[BotHandler.globalRandom.Next(candidates.Count)];

                    game.feedback.Add($"Saladmander destroys {game.player.kitchen.OptionAt(index).name} and gains +3p.");
                    caller.points += 3;

                    await game.player.kitchen.DestroyIngredient(game, index);
                }
            }
        }

        [GameIngredient]
        public class HorseRadish : Ingredient
        {
            public HorseRadish() : base("Horse Radish", 2, Rarity.Common, Tribe.Beast, "When picked, give a random Beast in your kitchen +1p.")
            {
                this.glowLocation = GameLocation.Kitchen;

                this.effects.Add(new EF());
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> candidates = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.tribe == Tribe.Beast);
                    if (candidates.Count == 0) return Task.CompletedTask;

                    Ingredient pick = candidates[BotHandler.globalRandom.Next(candidates.Count)];
                    pick.points++;

                    game.feedback.Add($"Horse Radish gives {pick.name} +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class ShrimpshellBento : Ingredient
        {
            public ShrimpshellBento() : base("Shrimpshell Bento", 1, Rarity.Common, Tribe.Beast, "When picked, give another random Beast in your kitchen and dish +2p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> candidatesKitchen = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.tribe == Tribe.Beast);
                    if (candidatesKitchen.Count > 0)
                    {
                        Ingredient pickKitchen = candidatesKitchen[BotHandler.globalRandom.Next(candidatesKitchen.Count)];
                        pickKitchen.points += 2;
                        game.feedback.Add($"Shrimpshell Bento gives {pickKitchen.name} in the kitchen +2p.");
                    }

                    List<Ingredient> candidatesDish = game.player.hand.GetAllIngredients().FindAll(x => x.tribe == Tribe.Beast && x != caller);
                    if (candidatesDish.Count > 0)
                    {
                        Ingredient pickDish = candidatesDish[BotHandler.globalRandom.Next(candidatesDish.Count)];
                        pickDish.points += 2;
                        game.feedback.Add($"Shrimpshell Bento gives {pickDish.name} in the dish +2p.");
                    }                    

                    return Task.CompletedTask;
                }
            }
        }
    
        [GameIngredient]
        public class Platterpus : Ingredient
        {
            public Platterpus() : base("Platterpus", 1, Rarity.Common, Tribe.Beast, "Cook: If your dish is full, gain +2p.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (game.player.hand.NonNullOptions == game.player.hand.handLimit)
                    {
                        caller.points += 2;
                        game.feedback.Add("Platterpus gains +2p.");
                    }

                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return game.player.hand.NonNullOptions == game.player.hand.handLimit;
            }
        }

        [GameIngredient]
        public class ScarabSnack : Ingredient
        {
            public ScarabSnack() : base("Scarab Snack", 2, Rarity.Rare, Tribe.Beast, "When picked, transform a random ingredient in the kitchen into a 3p one.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var options = BotHandler.genericPool.ingredients.FindAll(x => x.points == 3);

                    Ingredient ingr = game.pool.GetVanillaIngredient(options[BotHandler.globalRandom.Next(options.Count)].name);

                    int index = game.player.kitchen.GetAllIndexes()[BotHandler.globalRandom.Next(game.player.kitchen.GetAllIndexes().Count)];

                    game.feedback.Add($"Scarab Snack replaces {game.player.kitchen.OptionAt(index).name} with a {ingr.name}.");

                    game.player.kitchen.ReplaceIngredient(index, ingr);

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class ScarabStew : Ingredient
        {
            public ScarabStew() : base("Scarab Stew", 3, Rarity.Rare, Tribe.Beast, "When picked, transform a random ingredient in the kitchen into a 4p one.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var options = BotHandler.genericPool.ingredients.FindAll(x => x.points == 4);

                    Ingredient ingr = game.pool.GetVanillaIngredient(options[BotHandler.globalRandom.Next(options.Count)].name);

                    int index = game.player.kitchen.GetAllIndexes()[BotHandler.globalRandom.Next(game.player.kitchen.GetAllIndexes().Count)];

                    game.feedback.Add($"Scarab Stew replaces {game.player.kitchen.OptionAt(index).name} with a {ingr.name}.");

                    game.player.kitchen.ReplaceIngredient(index, ingr);

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class RareKodoSteak : Ingredient
        {
            public RareKodoSteak() : base("Rare Kodo Steak", 2, Rarity.Rare, Tribe.Beast, "When picked, give a random Beast in your kitchen these points.")
            {
                this.effects.Add(new EF());
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> candidates = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.tribe == Tribe.Beast);
                    if (candidates.Count == 0) return Task.CompletedTask;

                    Ingredient pick = candidates[BotHandler.globalRandom.Next(candidates.Count)];
                    pick.points += caller.points;

                    game.feedback.Add($"Rare Kodo Steak gives {pick.name} +{caller.points}p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class CornDog : Ingredient
        {
            public CornDog() : base("Corn Dog", 4, Rarity.Rare, Tribe.Beast, "Cook: Set adjacent ingredients' points to this one.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

                    Ingredient prev = game.player.hand.IngredientAt(cookArgs.handPos - 1);
                    Ingredient next = game.player.hand.IngredientAt(cookArgs.handPos + 1);

                    if (prev != null) prev.points = caller.points;
                    if (next != null) next.points = caller.points;

                    game.feedback.Add($"Corn Dog sets adjacent ingredients' points to {caller.points}.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class GoldenGoose : Ingredient
        {
            public GoldenGoose() : base("Golden Goose", 1, Rarity.Rare, Tribe.Beast, "Cook: If this has 2 or more points, give ingredients in your kitchen +2p.")
            {
                this.glowLocation = GameLocation.Hand;
                this.effects.Add(new EF());
            }

            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (caller.points >= 2)
                    {
                        foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                        {
                            ingr.points += 2;
                        }

                        game.feedback.Add("Golden Goose gives ingredients in your kitchen +2p.");
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return this.points >= 2;
            }
        }

        [GameIngredient]
        public class PupPeasInAPod : Ingredient
        {
            public PupPeasInAPod() : base("Pup Peas In A Pod", 1, Rarity.Epic, Tribe.Beast, "When picked, create 2 copies of this in empty dish slots.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.player.hand.AddIngredient(caller.Copy());
                    game.player.hand.AddIngredient(caller.Copy());

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class HotpotHydra : Ingredient
        {
            public HotpotHydra() : base("Hotpot Hydra", 2, Rarity.Epic, Tribe.Beast, "When picked, double the points of Beasts in your kitchen and dish.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.tribe == Tribe.Beast) ingr.points *= 2;
                    }
                    foreach (var ingr in game.player.hand.GetAllIngredients())
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Beast) ingr.points *= 2;
                    }

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
                        foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                        {
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
    }
}
