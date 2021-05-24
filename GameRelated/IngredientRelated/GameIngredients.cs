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
        public class TestIngredient : Ingredient
        {
            public TestIngredient() : base("Test Ingredient", 3, Rarity.Common) { }            
        }

        [GameIngredient]
        public class TestIngredient2 : Ingredient
        {
            public TestIngredient2() : base("Test Ingredient2", 5, Rarity.Common) { }
        }

        [GameIngredient]
        public class TestIngredient3 : Ingredient
        {
            public TestIngredient3() : base("Another Test Ingredient", 8, Rarity.Epic) 
            {
                this.effects.Add(new G());
            }
            private class G : Effect
            {
                public G() : base(EffectType.NullType) { }
                public override Task Call(Ingredient caller, Game game, EffectArgs args)
                {
                    game.player.curPoints += 5;
                    return Task.CompletedTask;
                }
            }
        }
    }
}
