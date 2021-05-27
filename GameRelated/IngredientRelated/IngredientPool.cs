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

        public IngredientPool(IngredientPool copy)
        {
            this.ingredients = new List<Ingredient>();

            foreach (var ingr in copy.ingredients)
            {
                this.ingredients.Add(ingr.Copy());
            }
        }

        public void LoadDefaultPool()
        {
            ingredients = new List<Ingredient>();

            var allIngredientClasses =                
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(GameIngredientAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<GameIngredientAttribute>() };

            foreach (var x in allIngredientClasses)
            {
                this.ingredients.Add((Ingredient)(Activator.CreateInstance(x.Type)));
            }

            this.ingredients.Sort();
        }

        public Ingredient GetVanillaIngredient(string name)
        {
            throw new NotImplementedException();
        }
    }
}
