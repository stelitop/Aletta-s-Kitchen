using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class PickingChoices
    {
        public int pick, spot;
        public PickingChoices() => Clear();
        public void Clear()
        {
            pick = spot = -1;
        }
    }
}
