using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class DeathrattleArgs : EffectArgs
        {
            int pos;
            GameLocation location;

            public DeathrattleArgs(EffectType calledEffect, int pos, GameLocation location) : base(calledEffect)
            {
                this.pos = pos;
                this.location = location;
            }
        }
    }    
}
