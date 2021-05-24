using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Kitchen
    {
        private List<Ingredient> _options;
        public Ingredient nextOption { get; private set; }

        public void Refresh(IngredientPool pool)
        {
            throw new NotImplementedException();
        }
        public void BuyIngredient(Game game, int pos)
        {
            throw new NotImplementedException();
        }
        public void RemoveIngredient(Game game, int pos)
        {
            throw new NotImplementedException();
        }
        public Ingredient OptionAt(int pos)
        {
            throw new NotImplementedException();
        }
    }
}
