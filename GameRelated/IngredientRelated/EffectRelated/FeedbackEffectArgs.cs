using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public partial class EffectArgs
    {
        public class FeedbackEffectArgs : EffectArgs
        {
            public string feedback = string.Empty;

            public FeedbackEffectArgs(EffectType calledEffect) : base(calledEffect) {}
        }
    }
}
