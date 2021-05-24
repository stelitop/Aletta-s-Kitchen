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
            public CoconutJuice() : base("Coconut Juice", 2, Rarity.Common) { }
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

                    if (cands.Count == 0) return Task.CompletedTask;

                    int pick = BotHandler.globalRandom.Next(cands.Count);

                    cands[pick].points--;
                    caller.points++;
                    
                    return Task.CompletedTask;
                }
            }
        }
    }
}
