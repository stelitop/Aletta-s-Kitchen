using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        //public CommandContext ctx;s
        public EffectType calledEffect;

        public EffectArgs(EffectType calledEffect)
        {
            this.calledEffect = calledEffect;
        }
    }
}
