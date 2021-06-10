using Aletta_s_Kitchen.BotRelated;
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

        public Effect()
        {
            this.Type = new List<EffectType>();
        }
        public Effect(EffectType type) 
        {
            this.Type = new List<EffectType> { type };
        }
        public Effect(List<EffectType> types)
        {
            this.Type = types.ToList();
        }

        public abstract Task Call(Ingredient caller, Game game, EffectArgs args);    
        
        public virtual Effect Copy()
        {
            return (Effect)Activator.CreateInstance(this.GetType());                                  
        }

        public static async Task CallEffects(List<Effect> effects, EffectType type, Ingredient caller, Game game, EffectArgs args, bool removeAfterCall = false)
        {
            List<Effect> toBeCast = new List<Effect>();

            if (args != null) args.calledEffect = type;

            toBeCast = effects.FindAll(x => x.Type.Contains(type));

            if (removeAfterCall) effects.RemoveAll(x => x.Type.Contains(type));

            for (int i=0; i<toBeCast.Count; i++)
            {
                try
                {
                    await toBeCast[i].Call(caller, game, args);
                }
                catch (Exception e)
                {
                    await BotHandler.reportsChannel.SendMessageAsync($"{caller.name} threw an exception. EffectType = {type}. Check the console <@237264833433567233>.");
                    Console.WriteLine(e.Message);
                }
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
