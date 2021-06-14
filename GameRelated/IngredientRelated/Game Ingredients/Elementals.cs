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
        public class StarAniseSpice : Ingredient
        {
            public StarAniseSpice() : base("Star Anise Spice", 1, Rarity.Common, Tribe.NoTribe, "Cook: Give Elementals this game +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.tribe == Tribe.Elemental, x => { x.points++; });

                    game.feedback.Add("Star Arise Spice gives your Elementals this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class VanillaIceCream : Ingredient
        {
            public VanillaIceCream() : base("Vanilla Ice Cream", 3, Rarity.Common, Tribe.Elemental, "Cook: Give tribeless ingredients in your kitchen +1p.") 
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
                        if (ingr.tribe == Tribe.NoTribe) ingr.points++;
                    }

                    game.feedback.Add("Vanilla Ice Cream gives tribeless ingredients in your kitchen +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class SushiRager : Ingredient
        {
            public SushiRager() : base("Sushi Rager", 3, Rarity.Common, Tribe.Elemental, "When this enters your kitchen, is picked and Cook: Destroy the lowest-point ingredient in your kitchen.") 
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
                public EF() : base(new List<EffectType> { EffectType.OnEnteringKitchen, EffectType.WhenPicked, EffectType.OnBeingCookedBefore}) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int min = 99999999;
                    List<int> candidates = new List<int>();
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        var ingr = game.player.kitchen.OptionAt(i);

                        if (ingr == null) continue;
                        if (ingr.points < min)
                        {
                            min = ingr.points;
                            candidates.Clear();
                            candidates.Add(i);
                        }
                        else if (ingr.points == min)
                        {
                            candidates.Add(i);
                        }
                    }

                    if (candidates.Count == 0) return;

                    int index = candidates[BotHandler.globalRandom.Next(candidates.Count)];

                    game.feedback.Add($"Sushi Rager destroys {game.player.kitchen.OptionAt(index).name} in your kitchen.");

                    await game.player.kitchen.DestroyIngredient(game, index);
                }
            }
        }

        [GameIngredient]
        public class MountainDewdrop : Ingredient
        {
            public MountainDewdrop() : base("Mountain Dewdrop", 2, Rarity.Common, Tribe.Elemental, "Cook: If you cooked an Elemental last, gain +2p.")
            {
                this.glowLocation = GameLocation.Any;
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (!CommonConditions.ElementalCondition(game)) return Task.CompletedTask;

                    caller.points+=2;
                    game.feedback.Add("Your Mountain Dewdrop gains +2p.");                    

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.ElementalCondition(game);
        }

        [GameIngredient]
        public class RisingDough : Ingredient
        {
            public RisingDough() : base("Rising Dough", 2, Rarity.Rare, Tribe.Elemental, "Cook: If you cooked an Elemental last, give all ingredients in your kitchen +3p.")
            {
                this.glowLocation = GameLocation.Any;
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    try
                    {
                        if (!CommonConditions.ElementalCondition(game)) return;

                        var kitchen = game.player.kitchen.GetAllNonNullIngredients();
                        foreach (var ingr in kitchen)
                        {
                            ingr.points += 3;
                        }

                        game.feedback.Add("Your Rising Dough gives all ingredients in the Kitchen +3p.");
                    }
                    catch (Exception e)
                    {
                        await BotHandler.reportsChannel.SendMessageAsync("Rising Dough threw an exception. Check the console <@237264833433567233>.");
                        Console.WriteLine(e.Message);
                    }
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.ElementalCondition(game);
        }

        [GameIngredient]
        public class Creampooch : Ingredient
        {
            public Creampooch() : base("Creampooch", 3, Rarity.Rare, Tribe.Elemental, "Cook: If you cooked an Elemental last, replace the next ingredient in your kitchen with an Elemental. Give it +5p.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Any;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    try
                    {
                        if (!CommonConditions.ElementalCondition(game)) return;

                        game.player.kitchen.nextOption = game.pool.GetRandomIngredient(x => x.tribe == Tribe.Elemental && x.name != "Creampooch");
                        game.player.kitchen.nextOption.points += 5;

                        game.feedback.Add("Your Creampooch replaces the next ingredient in the kitchen with an Elemental with +5p.");                        
                    }
                    catch (Exception e)
                    {
                        await BotHandler.reportsChannel.SendMessageAsync("Creampooch threw an exception. Check the console <@237264833433567233>.");
                        Console.WriteLine(e.Message);
                    }
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.ElementalCondition(game);
        }

        [GameIngredient]
        public class GelatoGloop : Ingredient
        {
            public GelatoGloop() : base("Gelato Gloop", 4, Rarity.Epic, Tribe.Elemental, "When picked, transform a random non-Elemental in your kitchen into a Gelato Gloop.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var ingrs = game.player.kitchen.GetAllIngredients();
                    List<int> indexes = new List<int>();

                    for (int i = 0; i < ingrs.Count; i++)
                    {
                        if (ingrs[i] != null)
                        {
                            if (ingrs[i].tribe != Tribe.Elemental) indexes.Add(i);
                        }
                    }

                    int index = indexes[BotHandler.globalRandom.Next(indexes.Count)];

                    game.feedback.Add($"Gelato Gloop transforms {game.player.kitchen.OptionAt(index).name} into a Gelato Gloop.");

                    game.player.kitchen.ReplaceIngredient(index, game.pool.GetVanillaIngredient("Gelato Gloop"));                    

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class IcecreamConenberg : Ingredient
        {
            public IcecreamConenberg() : base("Icecream Conenberg", 5, Rarity.Legendary, Tribe.Elemental, "When picked, gain +10 for each Elemental you cooked in a row before this.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Kitchen;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    int depth = 1;

                    while (game.player.dishHistory.Count >= depth)
                    {
                        if (game.player.dishHistory[game.player.dishHistory.Count - depth].FindAll(x => x.tribe == Tribe.Elemental).Count > 0)
                        {
                            depth++;
                        }
                        else break;
                    }

                    caller.points += 10 * (depth - 1);

                    return Task.CompletedTask;
                }
            }

            public override string GetDescriptionText(Game game, GameLocation gameLocation)
            {
                if (gameLocation == GameLocation.Hand) return base.GetDescriptionText(game, gameLocation);

                string ret = base.GetDescriptionText(game, gameLocation);

                int depth = 1;

                while (game.player.dishHistory.Count >= depth)
                {
                    if (game.player.dishHistory[game.player.dishHistory.Count - depth].FindAll(x => x.tribe == Tribe.Elemental).Count > 0)
                    {
                        depth++;
                    }
                    else break;
                }

                return ret + $" ({depth - 1})";
            }

            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.ElementalCondition(game);
        }

        [GameIngredient]
        public class RagnarOs : Ingredient
        {
            public RagnarOs() : base("Ragnar-Os", 4, Rarity.Legendary, Tribe.Elemental, "After you pick an ingredient while this is in your dish, give a random one in your kitchen +8p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.AfterYouPickAnIngerdientInHand) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cands = game.player.kitchen.GetAllNonNullIngredients();
                    var pick = cands[BotHandler.globalRandom.Next(cands.Count)];
                    pick.points += 8;
                    game.feedback.Add($"Ragnar-Os gives {pick.name} +8p.");
                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class FastfoodChainer : Ingredient
        {
            public FastfoodChainer() : base("Fastfood Chainer", 3, Rarity.Rare, Tribe.Elemental, "Cook: If you cooked an Elemental last, create a 5p one and put another to enter your kitchen next.")
            {
                this.glowLocation = GameLocation.Any;
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (!CommonConditions.ElementalCondition(game)) return;

                    await game.player.hand.AddIngredient(game, game.pool.GetTokenIngredient("Appetiser Elemental"));

                    game.player.kitchen.nextOption = game.pool.GetTokenIngredient("Appetiser Elemental");

                    game.feedback.Add("Fastfood Chainer adds a 5p Appetiser Elemental to your dish and next in the kitchen.");
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.ElementalCondition(game);            

            [TokenIngredient]
            public class AppetiserElemental : Ingredient
            {
                public AppetiserElemental() : base("Appetiser Elemental", 5, Rarity.Common, Tribe.Elemental) { }
            }
        }
    }
}
