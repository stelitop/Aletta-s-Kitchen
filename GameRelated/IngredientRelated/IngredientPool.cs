using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public class IngredientPool
    {
        public List<Ingredient> ingredients;

        public IngredientPool()
        {
            this.LoadDefaultPool();
        }

        public void LoadDefaultPool()
        {
            ingredients.Clear();
            throw new NotImplementedException();
        }
    }
}
