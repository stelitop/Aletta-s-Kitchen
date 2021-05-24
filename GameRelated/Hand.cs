using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Hand
    {
        public List<Ingredient> ingredients;

        public void Clear()
        {
            this.ingredients.Clear();
        }
    }
}
