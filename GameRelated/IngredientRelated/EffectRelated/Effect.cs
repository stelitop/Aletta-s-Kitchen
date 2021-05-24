using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public abstract class Effect
    {
        public List<EffectType> Type { get; private set; }

        public abstract void Call(Ingredient caller, Game game, EffectArgs args);    
        
        public virtual Effect Copy()
        {
            Effect ret = (Effect)Activator.CreateInstance(this.GetType());

            for (int i = 0; i < this.Type.Count; i++) ret.Type.Add(this.Type[i]);

            return ret;
        }
    }
}
