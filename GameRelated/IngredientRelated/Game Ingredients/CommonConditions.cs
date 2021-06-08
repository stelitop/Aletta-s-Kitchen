using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.Game_Ingredients
{
    public class CommonConditions
    {
        public static bool ElementalCondition(Game game)
        {
            if (game.player.dishHistory.Count == 0) return false;
            if (game.player.dishHistory.Last().FindAll(x => x.tribe == Tribe.Elemental).Count > 0) return true;
            return false;
        }

        public static bool OutcastCondition(int kitchenPos)
        {
            return (kitchenPos == 0 || kitchenPos == 4);
        }

        public static bool DragonCondition(Game game)
        {
            foreach (var ingr in game.player.kitchen.GetAllNonNullIngredients())
            {
                if (ingr.tribe == Tribe.Dragon) return true;
            }
            return false;
        }
    }
}
