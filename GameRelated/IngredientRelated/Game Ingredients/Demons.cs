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
        public class GruelImp : Ingredient
        {
            public GruelImp() : base("Gruel Imp", 2, Rarity.Common, Tribe.Demon, "When picked, steal 1p from a random ingredient in your kitchen.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> cands = new List<Ingredient>();
                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
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
        public class BigMac : Ingredient
        {
            public BigMac() : base("Big Mac", 5, Rarity.Common, Tribe.Demon, "When picked, replace a random ingredient in your kitchen with a 1p Small Fry.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<int> cands = new List<int>();
                    for (int i = 0; i < game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) != null)
                        {
                            cands.Add(i);
                        }
                    }

                    int replace = cands[BotHandler.globalRandom.Next(cands.Count)];
                    string name = game.player.kitchen.OptionAt(replace).name;

                    game.player.kitchen.ReplaceIngredient(replace, game.pool.GetTokenIngredient("Small Fry"));

                    game.feedback.Add($"Big Mac replaces {name} in your Kitchen with a Small Fry.");

                    return Task.CompletedTask;
                }
            }
            [TokenIngredient]
            public class SmallFry : Ingredient
            {
                public SmallFry() : base("Small Fry", 1, Rarity.Common, Tribe.NoTribe) { }
            }
        }

        [GameIngredient]
        public class Demonade : Ingredient
        {
            public Demonade() : base("Demonade", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give all Demons this game +6p. Give all other ingredients -1p (but not less than 1)")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.tribe == Tribe.Demon, x => { x.points += 6; });
                    game.RestOfGameBuff(x => x.tribe != Tribe.Demon, x => { x.points -= 1; if (x.points < 1) x.points = 1; }); 

                    game.feedback.Add("Demonade gives your Demons this game +6p and your other ingredients -1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class SoulstrawSucker : Ingredient
        {
            public SoulstrawSucker() : base("Soulstraw Sucker", 4, Rarity.Rare, Tribe.Demon, "When picked, steal 1 point from each ingredient in the kitchen.")
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
                        if (ingr.points > 0)
                        {
                            ingr.points--;
                            caller.points++;
                        }
                    }

                    game.feedback.Add("Soulstraw Sucker steals 1 point from each ingredient in your kitchen.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class Hamburglar : Ingredient
        {
            public Hamburglar() : base("Hamburglar", 2, Rarity.Rare, Tribe.Demon, "Cook: Steal points from all ingredients in your kitchen until they're 1.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int stolen = 0;

                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.points > 1)
                        {
                            caller.points += ingr.points - 1;
                            stolen += ingr.points - 1;
                            ingr.points = 1;
                        }
                    }

                    game.feedback.Add($"Hamburglar steals {stolen}p from your kitchen.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class TiramisuTerror : Ingredient
        {
            public TiramisuTerror() : base("Tiramisu Terror", 0, Rarity.Epic, Tribe.Demon, "When this enters your kitchen, destroy the 2 highest-point ingredients in it and gain their points.")
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
                    List<Ingredient> sortedIngr = game.player.kitchen.GetAllNonNullIngredients().FindAll(x => x != caller).OrderByDescending(x => x.points).ToList();

                    if (sortedIngr.Count >= 2)
                    {
                        int index1 = game.player.kitchen.GetAllNonNullIngredients().FindIndex(x => x == sortedIngr[0]);
                        int index2 = game.player.kitchen.GetAllNonNullIngredients().FindIndex(x => x == sortedIngr[1]);

                        caller.points += sortedIngr[0].points + sortedIngr[1].points;
                        game.feedback.Add($"Tiramisu Terror destroys and steals the points of {sortedIngr[0].name} and {sortedIngr[1].name}.");

                        await game.player.kitchen.DestroyMultipleIngredients(game, new List<int>() { index1, index2 });
                    }
                    else if (sortedIngr.Count == 1)
                    {
                        int index1 = game.player.kitchen.GetAllNonNullIngredients().FindIndex(x => x == sortedIngr[0]);

                        caller.points += sortedIngr[0].points;
                        game.feedback.Add($"Tiramisu Terror destroys and steals the points of {sortedIngr[0].name}");

                        await game.player.kitchen.DestroyIngredient(game, index1);
                    }
                }
            }
        }

        [GameIngredient]
        public class Gingerdreadsteed : Ingredient
        {
            public Gingerdreadsteed() : base("Gingerdreadsteed", 5, Rarity.Epic, Tribe.Demon, "Cook: The next copy of this has 1p less.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.name == "Gingerdreadsteed") ingr.points = Math.Max(0, ingr.points - 1);
                    }
                    if (game.player.kitchen.nextOption != null)
                    {
                        if (game.player.kitchen.nextOption.name == "Gingerdreadsteed") game.player.kitchen.nextOption.points = Math.Max(0, game.player.kitchen.nextOption.points - 1);
                    }
                    foreach (var ingr in game.pool.ingredients)
                    {
                        if (ingr == null) continue;
                        if (ingr.name == "Gingerdreadsteed") ingr.points = Math.Max(0, ingr.points - 1);
                    }

                    game.feedback.Add("Gingerdreadsteed makes the next copy of it have 1p less.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class TorenaiOutOfThyme : Ingredient
        {
            public TorenaiOutOfThyme() : base("Torenai, Out of Thyme", 7, Rarity.Legendary, Tribe.Demon, "Cook: Replace all ingredients in the kitchen with a copy of the lowest-point one.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    List<Ingredient> candidates = new List<Ingredient>();
                    int min = 99999999;
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.points < min)
                        {
                            min = ingr.points;
                            candidates.Clear();
                            candidates.Add(ingr);
                        }
                        else if (ingr.points == min)
                        {
                            candidates.Add(ingr);
                        }
                    }

                    Ingredient copy = candidates[BotHandler.globalRandom.Next(candidates.Count)];

                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        if (game.player.kitchen.OptionAt(i) == null) continue;

                        game.player.kitchen.ReplaceIngredient(i, copy.Copy());
                    }

                    game.feedback.Add($"Torenai, the One Out of Thyme replaces your ingredients with copies of {copy.name}.");

                    return Task.CompletedTask;
                }
            }
        }
    }
}
