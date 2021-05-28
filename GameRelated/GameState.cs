using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public enum GameState
    {
        None,
        Loading,
        PickFromKitchen,
        //ChooseInHandForIngredient,
        BeforeQuota,
        GameOver,
        TimedOut
    }
}
