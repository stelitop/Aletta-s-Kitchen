﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameIngredientAttribute : Attribute
    {
        public GameIngredientAttribute()
        {

        }
    }
}
