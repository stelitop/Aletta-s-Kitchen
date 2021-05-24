using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public class Ingredient
    {
        public string name;
        public Tribe tribe;
        public int points;

        public List<Effect> effects;

        public Ingredient()
        {
            this.name = "Default Ingredient";
            this.tribe = Tribe.NoTribe;
            this.points = 1;
            this.effects = new List<Effect>();
        }

        public virtual bool CanBeBought(Game game, int kitchenPos)
        {
            return true;
        }

        public void Buy(Game game, int kitchenPos)
        {
            throw new NotImplementedException();
        }

        public virtual Ingredient Copy()
        {
            Ingredient ret = (Ingredient)Activator.CreateInstance(this.GetType());

            ret.name = this.name;
            ret.points = this.points;
            ret.tribe = this.tribe;

            foreach (var effect in this.effects)
            {
                ret.effects.Add(effect.Copy());
            }

            return ret;
        }
    }
}
