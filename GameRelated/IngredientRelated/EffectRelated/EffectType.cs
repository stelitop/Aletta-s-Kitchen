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
		OnBeingPicked,
		OnBeingCooked,
		WheneverThisGainsPoints,
		WheneverAddPointsToScore,
		OnBuyingAnIngredient,
		OnEnteringKitchen,
		Deathrattle
	}
}
