using Aletta_s_Kitchen.BotRelated;
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
        public List<Ingredient> tokens;

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

            this.tokens = new List<Ingredient>();

            foreach (var ingr in copy.tokens)
            {
                this.tokens.Add(ingr.Copy());
            }
        }

        public void LoadDefaultPool()
        {
            ingredients = new List<Ingredient>();
            tokens = new List<Ingredient>();

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

            var allTokensClasses =
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(TokenIngredientAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<TokenIngredientAttribute>() };

            foreach (var x in allTokensClasses)
            {
                this.tokens.Add((Ingredient)(Activator.CreateInstance(x.Type)));
            }

            this.ingredients.Sort();
            this.tokens.Sort();
        }

        public Ingredient GetVanillaIngredient(string name)
        {
            for (int i=0; i<this.ingredients.Count; i++)
            {
                if (this.ingredients[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return this.ingredients[i].Copy();
                }
            }
            return null;
        }
        public Ingredient GetTokenIngredient(string name)
        {
            for (int i = 0; i < this.tokens.Count; i++)
            {
                if (this.tokens[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return this.tokens[i].Copy();
                }
            }
            return null;
        }


        public Ingredient GetRandomIngredient()
        {
            int pick = BotHandler.globalRandom.Next(this.ingredients.Count);
            return this.ingredients[pick].Copy();
        }
        public Ingredient GetRandomIngredient(RandomIngredientCondition condition)
        {
            List<Ingredient> subList = this.ingredients.FindAll(x => condition(x));

            if (subList.Count == 0) return null;

            int pick = BotHandler.globalRandom.Next(subList.Count);
            return subList[pick].Copy();
        }

        public delegate bool RandomIngredientCondition(Ingredient ingredient);
    }
}
