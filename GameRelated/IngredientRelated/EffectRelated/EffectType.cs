using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated
{
    public enum EffectType
    {
		NullType,
		WhenPicked,
		OnBeingCookedBefore, //called before the ingredients are removed from hand, should be used usually
		OnBeingCookedAfter, //called after the ingredients are removed from hand
		WheneverThisGainsPoints, //not done currently
		WheneverAddPointsToScore, //not done currently
		OnEnteringKitchen,
		Deathrattle,
		Timer,
		Outcast,
		AfterYouPickAnIngerdientInHand,
		AfterYouPickAnIngerdientInKitchen,
		AfterYouCook,
		AfterYouAddIngredientToHand,
		WheneverIngredientEntersKitchen,
	}
}
