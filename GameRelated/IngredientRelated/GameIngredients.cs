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
            public TestIngredient3() : base("Another Test Ingredient", 8, Rarity.Epic) { }
        }
    }
}
