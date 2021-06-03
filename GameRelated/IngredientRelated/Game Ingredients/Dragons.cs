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
        public class SteamscaleSoba : Ingredient
        {
            public SteamscaleSoba() : base("Steamscale Soba", 2, Rarity.Common, Tribe.Dragon, "Cook: If your kitchen has a Dragon, gain +1p.")
            {
                this.glowLocation = GameLocation.Hand;
                this.effects.Add(new EF());
            }

            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.DragonCondition(game);

            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

                    bool hasDragon = false;
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
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
                foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                {
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
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
                    {
                        if (ingr.tribe == Tribe.Dragon)
                        {
                            hasDragon = true;
                            break;
                        }
                    }

                    if (hasDragon)
                    {
                        cookArgs.dishPoints += 3;
                        caller.points += 3;
                        game.feedback.Add("Steamscale Soba gains +3p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class ScaledSaltshaker : Ingredient
        {
            public ScaledSaltshaker() : base("Scaled Saltshaker", 4, Rarity.Rare, Tribe.Dragon, "Cook: If your kitchen has a Dragon, give Dragons +2p this game.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.feedback.Add("Scaled Saltshaker gives you Dragons this game +2p.");

                    game.RestOfGameBuff(x => x.tribe == Tribe.Dragon, x => { x.points += 2; });

                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos) => CommonConditions.DragonCondition(game);            
        }

        [GameIngredient]
        public class PlumpKing : Ingredient
        {
            public PlumpKing() : base("Plump King", 3, Rarity.Epic, Tribe.Dragon, "Cook: If your dish is full and this is the highest-point ingredient, gain +4p.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (game.player.hand.NonNullOptions != game.player.hand.handLimit) return Task.CompletedTask;
                    int best = -1;
                    foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients()) if (best < ingr.points) best = ingr.points;

                    if (caller.points != best) return Task.CompletedTask;

                    caller.points += 4;

                    game.feedback.Add("Plump King gains +4p for being the highest-point ingredient in your dish.");

                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                if (game.player.hand.NonNullOptions != game.player.hand.handLimit) return false;
                int best = -1;
                foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients()) if (best < ingr.points) best = ingr.points;

                if (this.points != best) return false;

                return true;
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
                    game.RestOfGameBuff(x => x.tribe == Tribe.Dragon, x => { x.points++; });

                    game.feedback.Add("Wasabi Spice gives your Dragons this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class HighTeaLordPrestor : Ingredient
        {
            public HighTeaLordPrestor() : base("High Tea Lord Prestor", 4, Rarity.Legendary, Tribe.Dragon, "Cook: If your dish has an ingredient with 10 or more points, it will return to your kitchen next turn.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Hand;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cands = game.player.hand.GetAllNonNullIngredients().FindAll(x => x.points >= 10);

                    if (cands.Count == 0) return Task.CompletedTask;

                    var pick = cands[BotHandler.globalRandom.Next(cands.Count)];
                    game.feedback.Add($"High Tea Lord Prestor makes {pick.name} return to your kitchen next turn.");
                    game.player.kitchen.nextOption = pick.Copy();

                    return Task.CompletedTask;
                }
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                var cands = game.player.hand.GetAllNonNullIngredients().FindAll(x => x.points >= 10);

                return cands.Count > 0;
            }
        }

        [GameIngredient]
        public class Applestrasza : Ingredient
        {
            public Applestrasza() : base("Applestrasza", 7, Rarity.Legendary, Tribe.Dragon, "If this enters a kitchen without a full dish, it leaves immediately.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnEnteringKitchen) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var enterArgs = args as EffectArgs.EnteringKitchenArgs;

                    if (game.player.hand.NonNullOptions != game.player.hand.handLimit)
                    {
                        game.feedback.Add("Applestrasza leaves your kitchen!");
                        game.player.kitchen.ReplaceIngredient(enterArgs.kitchenPos, null);
                        await game.player.kitchen.FillEmptySpots(game);
                    }
                }
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
        public class MatchaRoaster : Ingredient
        {
            public MatchaRoaster() : base("Matcha Roaster", 1, Rarity.Epic, Tribe.Dragon, "When picked, gain points to match the highest in your dish.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

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
        public class HighFibreIrontail : Ingredient
        {
            public HighFibreIrontail() : base("High-Fibre Irontail", 3, Rarity.Epic, Tribe.Dragon, "Whenever you pick a higher-point ingredient, gain +3p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(new List<EffectType> { EffectType.AfterYouPickAnIngerdientInHand, EffectType.AfterYouPickAnIngerdientInKitchen}) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var pickArgs = args as EffectArgs.IngredientPickArgs;

                    if (pickArgs.pickedIngr.points > caller.points)
                    {
                        caller.points += 3;
                        game.feedback.Add("High-Fibre Irontail gains +3p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class PureDragon : Ingredient
        {
            public PureDragon() : base("Pure Dragon", 4, Rarity.Common, Tribe.Dragon, "After you cook a dish with a Dragon, give Dragons +1p this game.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Kitchen;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return game.player.hand.GetAllNonNullIngredients().FindAll(x => x.tribe == Tribe.Dragon).Count > 0;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.AfterYouCook) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.AfterCookArgs;

                    bool trig = false;
                    foreach (var ingr in cookArgs.hand)
                    {
                        if (ingr == null) continue;
                        if (ingr.tribe == Tribe.Dragon)
                        {
                            trig = true;
                            break;
                        }
                    }
                    if (!trig) return Task.CompletedTask;

                    game.feedback.Add("Pure Dragon gives your Dragons this game +1p.");

                    game.RestOfGameBuff(x => x.tribe == Tribe.Dragon, x => { x.points++; });

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class MonsterPlatter : Ingredient
        {
            public MonsterPlatter() : base("Monster Platter", 3, Rarity.Rare, Tribe.Dragon, "After you cook a full dish, gain +5p.")
            {
                this.effects.Add(new EF());
                this.glowLocation = GameLocation.Kitchen;
            }
            public override bool GlowCondition(Game game, int kitchenPos)
            {
                return game.player.hand.NonNullOptions == game.player.hand.handLimit;
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.AfterYouCook) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.AfterCookArgs;

                    if (cookArgs.hand.FindAll(x => x != null).Count == game.player.hand.handLimit)
                    {
                        caller.points += 5;
                        game.feedback.Add("Monster Platter gains +5p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }
    }
}
