using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public abstract class Effect
    {
        public List<EffectType> Type { get; private set; } = new List<EffectType>();

        public bool ToBeRemoved { get; protected set; } = false;

        protected Effect(EffectType type) 
        {
            this.Type = new List<EffectType> { type };
        }
        protected Effect(List<EffectType> types)
        {
            this.Type = types.ToList();
        }

        public abstract Task Call(Ingredient caller, Game game, EffectArgs args);    
        
        public virtual Effect Copy()
        {
            Effect ret = (Effect)Activator.CreateInstance(this.GetType());

            for (int i = 0; i < this.Type.Count; i++) ret.Type.Add(this.Type[i]);

            return ret;
        }


        public static async Task CallEffects(List<Effect> effects, EffectType type, Ingredient caller, Game game, EffectArgs args, bool removeAfterCall = false)
        {
            List<Effect> toBeCast = new List<Effect>();

            if (args != null) args.calledEffect = type;

            for (int i = 0; i < effects.Count(); i++)
            {
                if (effects[i].Type.Contains(type))
                {
                    toBeCast.Add(effects[i]);
                }
            }

            if (removeAfterCall) effects.RemoveAll(x => x.Type.Contains(type));

            foreach (var effect in toBeCast)
            {
                await effect.Call(caller, game, args);
            }

            if (!removeAfterCall)
                for (int i = 0; i < effects.Count(); i++)
                {
                    if (effects[i].ToBeRemoved)
                    {
                        effects.RemoveAt(i);
                        i--;
                    }
                }
        }
    }
}
