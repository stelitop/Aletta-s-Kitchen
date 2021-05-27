﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class OnBeingCookedArgs : EffectArgs
        {
            public int dishPoints;
            public OnBeingCookedArgs(EffectType effectType, int dishPoints) : base(effectType)
            {
                this.dishPoints = dishPoints;
            }
        }
    }
}
