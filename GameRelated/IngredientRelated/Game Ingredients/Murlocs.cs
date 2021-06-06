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
        public class MurlocScout : Ingredient
        {
            public MurlocScout() : base("Murloc Scout", 1, Rarity.Common, Tribe.Murloc) { }
        }

        [GameIngredient]
        public class MurlocTidehunter : Ingredient
        {
            public MurlocTidehunter() : base("Murloc Tidehunter", 2, Rarity.Common, Tribe.Murloc, "When picked, create a 1p Murloc in an empty dish slot.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }

                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    //game.player.hand.AddIngredient(game.pool.GetVanillaIngredient("Murloc Tinyfin"));
                    await game.player.hand.AddIngredient(game, new MurlocScout());
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
                public EF() : base(EffectType.WhenPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    for (int i = 0; i < game.player.hand.OptionsCount; i++)
                    {
                        if (game.player.hand.IngredientAt(i) == null) continue;
                        if (game.player.hand.IngredientAt(i) == caller) continue;

                        if (game.player.hand.IngredientAt(i).tribe == Tribe.Murloc)
                            game.player.hand.IngredientAt(i).points++;
                    }

                    game.feedback.Add("Puddle Jumper gave all Murlocs in your dish +1 point.");

                    return Task.CompletedTask;
                }
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
                private bool active = false;
                public EF() : base(new List<EffectType> { EffectType.OnBeingCookedBefore, EffectType.OnBeingCookedAfter }) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    if (args.calledEffect == EffectType.OnBeingCookedBefore)
                    {
                        foreach (var ingr in game.player.hand.GetAllIngredients())
                        {
                            if (ingr == null) continue;
                            if (ingr.tribe != Tribe.Murloc)
                            {
                                return Task.CompletedTask;
                            }
                        }
                        this.active = true;

                    }
                    else if (args.calledEffect == EffectType.OnBeingCookedAfter)
                    {
                        if (this.active)
                        {
                            this.active = false;

                            var spArgs = args as EffectArgs.OnBeingCookedArgs;

                            spArgs.dishPoints *= 2;

                            game.feedback.Add("Captain Cookie doubles the score of your dish.");
                        }
                    }

                    return Task.CompletedTask;
                }
            }

            public override bool GlowCondition(Game game, int kitchenPos)
            {
                foreach (var ingr in game.player.hand.GetAllIngredients())
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

        //Mocha Murloc currently disabled
        //[GameIngredient]
        //public class MochaMurloc : Ingredient
        //{
        //    public MochaMurloc() : base("Mocha Murloc", 3, Rarity.Common, Tribe.Murloc, "When picked, immediately Cook.")
        //    {
        //        this.effects.Add(new EF());
        //    }
        //    private class EF : Effect
        //    {
        //        public EF() : base(EffectType.WhenPicked) { }

        //        public override async Task Call(Ingredient caller, Game game, EffectArgs args)
        //        {
        //            await game.player.hand.Cook(game);

        //            game.feedback.Insert(0, "Mocha Murloc forces you to Cook immediately.");
        //        }
        //    }
        //}

        [GameIngredient]
        public class MrglSpice : Ingredient
        {
            public MrglSpice() : base("Mrgl Spice", 1, Rarity.Rare, Tribe.NoTribe, "Cook: Give Murlocs this game +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedAfter) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.RestOfGameBuff(x => x.tribe == Tribe.Murloc, x => { x.points++; });

                    game.feedback.Add("Mrgl Spice gives your Murlocs this game +1p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class BloatedPufferfin : Ingredient
        {
            public BloatedPufferfin() : base("Bloated Pufferfin", 2, Rarity.Epic, Tribe.Murloc, "Cook: Gain +2p for each other Murloc in your dish.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.OnBeingCookedBefore) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var cookArgs = args as EffectArgs.OnBeingCookedArgs;

                    int buff = 0;

                    foreach (var ingr in game.player.hand.GetAllIngredients())
                    {
                        if (ingr == null) continue;
                        if (ingr == caller) continue;

                        if (ingr.tribe == Tribe.Murloc)
                        {
                            buff += 2;
                        }
                    }

                    caller.points += buff;
                    cookArgs.dishPoints += buff;

                    game.feedback.Add($"Bloated Pufferfin gains +{buff}p.");

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class Saltroc : Ingredient
        {
            public Saltroc() : base("Saltroc", 3, Rarity.Epic, Tribe.Murloc, "When picked and Cook: Create a 1p Murloc in an empty dish slot.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(new List<EffectType> { EffectType.WhenPicked, EffectType.OnBeingCookedAfter }) { }
                public override async Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    //game.player.hand.AddIngredient(game.pool.GetVanillaIngredient("Murloc Tinyfin"));
                    await game.player.hand.AddIngredient(game, new MurlocScout());

                    if (args.calledEffect == EffectType.OnBeingCookedAfter) game.feedback.Add("Saltroc creates a 1p Murloc in your dish.");                    
                }
            }
        }

        [GameIngredient]
        public class OlSalty : Ingredient
        {
            public OlSalty() : base("Ol' Salty", 5, Rarity.Legendary, Tribe.Murloc, "When picked, replace all ingredients in your kitchen with random Murlocs.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.WhenPicked) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    for (int i=0; i<game.player.kitchen.OptionsCount; i++)
                    {
                        Ingredient newIngr = new Ingredient();

                        game.player.kitchen.ReplaceIngredient(i, game.pool.GetRandomIngredient(x => x.tribe == Tribe.Murloc && x.name != "Ol' Salty"));
                    }

                    return Task.CompletedTask;
                }
            }
        }

        [GameIngredient]
        public class Yummyfin : Ingredient
        {
            public Yummyfin() : base("Yummyfin", 2, Rarity.Rare, Tribe.Murloc, "Whenever you pick or create another Murloc, give it +1p.")
            {
                this.effects.Add(new EF());
            }
            private class EF : Effect
            {
                public EF() : base(EffectType.AfterYouAddIngredientToHand) { }

                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    var addArgs = args as EffectArgs.IngredientAddedToHand;

                    if (addArgs.ingredient.tribe == Tribe.Murloc)
                    {
                        addArgs.ingredient.points++;

                        game.feedback.Add($"Yummyfin gives {addArgs.ingredient.name} 1p.");
                    }

                    return Task.CompletedTask;
                }
            }
        }
    }
}
