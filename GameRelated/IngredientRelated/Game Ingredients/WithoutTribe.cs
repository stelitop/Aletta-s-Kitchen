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
        public class JadeLotus : Ingredient
        {
            public JadeLotus() : base("Jade Lotus", 1, Rarity.Common, Tribe.NoTribe, "Cook: Gain 1p for each Jade Lotus you've cooked this game.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int copies = game.player.cookHistory.FindAll(x => x.name == "Jade Lotus").Count();

                    caller.points += copies;

                    return Task.CompletedTask;
                }
            }
            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                int copies = game.player.cookHistory.FindAll(x => x.name == "Jade Lotus").Count();

                return base.GetDescriptionText(game, gameLocation) + $" ({copies})";
            }
        }

        [GameIngredient]
        public class JadeSideDish : Ingredient
        {
            public JadeSideDish() : base("Jade Side Dish", 3, Rarity.Common, Tribe.NoTribe, "When picked, replace a random ingredient in your kitchen with a Jade Lotus.")            
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> candidates = new List<int>();
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) == null) continue;
                        candidates.Add(i);
                    }
                    int index = candidates[BotHandler.globalRandom.Next(candidates.Count)];

                    game.feedback.Add($"Jade Side Dish replaces {game.player.kitchen.OptionAt(index).name} with a Jade Lotus.");

                    game.player.kitchen.ReplaceIngredient(index, game.pool.GetVanillaIngredient("Jade Lotus"));

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class JadeMealSet : Ingredient
        {
            public JadeMealSet() : base("Jade Meal Set", 2, Rarity.Rare, Tribe.NoTribe, "When picked, create a Jade Lotus in an empty dish slot.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    await game.player.hand.AddIngredient(game, game.pool.GetVanillaIngredient("Jade Lotus"));                    
                }
            }
        }

        [GameIngredient]
        public class AyasJadeBuffet : Ingredient
        {
            public AyasJadeBuffet() : base("Aya's Jade Buffet", 4, Rarity.Legendary, Tribe.NoTribe, "When this enters your kitchen, is picked, and Cook: Create a Jade Lotus in an empty dish slot.")
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
                public EF() : base(new List<EffectType> { EffectType.WhenPicked, EffectType.OnEnteringKitchen, EffectType.OnBeingCookedAfter }) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    await game.player.hand.AddIngredient(game, game.pool.GetVanillaIngredient("Jade Lotus"));

                    game.feedback.Add("Aya Blackpaw's Jade Buffet created a Jade Lotus in your dish.");
                }
            }
        }

        [GameIngredient]
        public class BoxOfChocolates : Ingredient
        {
            public BoxOfChocolates() : base("Box of Chocolates", 0, Rarity.Common, Tribe.NoTribe, "Each turn in your kitchen, change between 0-4p.")
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
        public class BakusMooncake : Ingredient
        {
            public BakusMooncake() : base("Baku's Mooncake", 9, Rarity.Legendary, Tribe.NoTribe, "Can only be picked if your kitchen is all odd-point. Cook: Give future copies of this +10p.")
            {
                this.glowLocation = GameLocation.Kitchen;
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.name == "Baku's Mooncake", x => { x.points += 10; }, false);

                    game.feedback.Add("Baku's Mooncake gives its future copies +10p.");
                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
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
                public EF() : base(EffectType.WhenPicked) { }

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

                    int prev = cookArgs.handPos - 1, next = cookArgs.handPos + 1;
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
        public class TaterTinytots : Ingredient
        {
            public TaterTinytots() : base("Tater Tinytots", 1, Rarity.Epic, Tribe.NoTribe, "When picked, set all 1p ingredients in your kitchen to 10p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    bool trig = false;

                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.points == 1)
                        {
                            ingr.points = 10;
                            trig = true;
                        }
                    }

                    if (trig) game.feedback.Add("Tater Tinytots sets all 1p ingredients in your kitchen to 10p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class MenagerieMayonnaise : Ingredient
        {
            public MenagerieMayonnaise() : base("Menagerie Mayonnaise", 3, Rarity.Rare, Tribe.NoTribe, "When picked, give a random ingredient of each tribe in the kitchen +3p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (Tribe type in Enum.GetValues(typeof(Tribe)))
                    {
                        if (type == Tribe.NoTribe) continue;

                        var successes = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => this.Predicate(x, type));

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

        [GameIngredient]
        public class Crisp : Ingredient
        {
            public Crisp() : base("Crisp", 1, Rarity.Common, Tribe.NoTribe, "When picked, give a random ingredient in your kitchen +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cands = game.player.kitchen.GetAllNonNullIngredients();
                    var pick = cands[BotHandler.globalRandom.Next(cands.Count)];
                    pick.points++;
                    game.feedback.Add($"Crisp gives {pick.name} +1p.");
                    return Task.CompletedTask;
                }
            }
        }        

        [GameIngredient]
        public class ManatovCocktail : Ingredient
        {
            public ManatovCocktail() : base("Manatov Cocktail", 2, Rarity.Common, Tribe.NoTribe, "Cook: Give ingredients in your kitchen +1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients()) ingr.points++;

                    game.feedback.Add("Malatov Cocktail gives ingredients in your kitchen +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class FacelessFood : Ingredient
        {
            public FacelessFood() : base("Faceless Food", 5, Rarity.Rare, Tribe.NoTribe, "When picked, transform into a random ingredient with a tribe in the kitchen.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Kitchen;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var pickArgs = args as EffectArgs.OnBeingPickedArgs;

                    var cands = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.tribe != Tribe.NoTribe);
                    if (cands.Count == 0) return Task.CompletedTask;

                    var pick = cands[BotHandler.globalRandom.Next(cands.Count)];

                    game.feedback.Add($"Faceless Food transforms into {pick.name}.");

                    game.player.hand.ReplaceIngredient(pickArgs.handPos, pick.Copy());

                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x.tribe != Tribe.NoTribe).Count > 0;
            }
        }        

        [GameIngredient]
        public class LateBroomshroom : Ingredient
        {
            public LateBroomshroom() : base("Late Broomshroom", 2, Rarity.Epic, Tribe.NoTribe, "When picked as the lowest-point ingredient in the kitchen, gain +4p.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Kitchen;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.points < caller.points) return Task.CompletedTask;
                    }

                    game.feedback.Add("Late Broomshroom gains +4p.");
                    caller.points += 4;
                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                {
                    if (ingr.points < this.points) return false;
                }
                return true;
            }
        }

        [GameIngredient]
        public class YoggurtSaron : Ingredient
        {
            public YoggurtSaron() : base("Yoggurt-Saron", 0, Rarity.Legendary, Tribe.NoTribe, "When picked, create random ingredients in 2 empty dish slots.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    for (int i=0; i<2; i++)
                    {
                        await game.player.hand.AddIngredient(game, game.pool.GetRandomIngredient());
                    }
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
