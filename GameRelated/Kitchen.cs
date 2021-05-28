using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Kitchen
    {
        private List<Ingredient> _options;
        public Ingredient nextOption { get; set; } = new Ingredient();
        public int Count { get { return _options.Count; } }

        public Kitchen()
        {
            this._options = new List<Ingredient>();
        }

        public async Task Restart(Game game)
        {
            this._options.Clear();

            this.nextOption = game.pool.ingredients[BotHandler.globalRandom.Next(game.pool.ingredients.Count)].Copy();

            for (int i=0; i<5; i++)
            {
                await this.AddIngredient(game);
            }
        }
        public async Task<Ingredient> AddIngredient(Game game)
        {
            if (this._options.Count >= 5) return null;
            
            Ingredient ret = this.nextOption;

            _options.Add(ret);

            EffectArgs args = new EffectArgs(EffectType.OnEnteringKitchen);
            await Effect.CallEffects(ret.effects, EffectType.OnEnteringKitchen, ret, game, args);

            int pick = BotHandler.globalRandom.Next(game.pool.ingredients.Count);

            this.nextOption = game.pool.ingredients[pick].Copy();

            return ret;
        }
        public async Task PickIngredient(Game game, int kitchenPos)
        {
            int newSpot = game.player.hand.ingredients.Count;

            if (0 <= kitchenPos && kitchenPos < this._options.Count)
            {
                game.feedback.Clear();

                if (!this._options[kitchenPos].CanBeBought(game, kitchenPos))
                {
                    game.feedback.Add($"{this._options[kitchenPos].name} can't be bought currently!");
                    return;
                }
                if (newSpot >= 3)
                {
                    game.feedback.Add("Your dish is full!");
                    return;
                }
                
                Ingredient ingr = this._options[kitchenPos];

                this._options[kitchenPos] = null;

                game.player.hand.ingredients.Add(ingr);

                EffectArgs args = new EffectArgs.OnBeingPickedArgs(EffectType.OnBeingPicked, kitchenPos, newSpot);
                await Effect.CallEffects(ingr.effects, EffectType.OnBeingPicked, ingr, game, args);                

                game.player.pickHistory.Add(ingr.Copy());

                await game.NextRound();
            }
        }
        public async Task DestroyIngredient(Game game, int pos)
        {
            if (0 <= pos && pos < this._options.Count)
            {
                Ingredient ingr = this._options[pos].Copy();

                this._options[pos] = null;

                EffectArgs args = new EffectArgs.DeathrattleArgs(EffectType.Deathrattle, pos, GameLocation.Kitchen);
                await Effect.CallEffects(ingr.effects, EffectType.Deathrattle, ingr, game, args);
            }
        }
        public async Task DestroyMultipleIngredients(Game game, List<int> pos)
        {
            List<int> posFiltered = pos.FindAll(x => 0 <= x && x < this._options.Count && this.OptionAt(x) != null).Distinct().ToList();
            posFiltered.Sort();

            List<Ingredient> ingredients = new List<Ingredient>();

            for (int i=0; i<posFiltered.Count; i++)
            {
                ingredients.Add(this._options[posFiltered[i]].Copy());
                this._options[posFiltered[i]] = null;
            }

            for (int i = 0; i < posFiltered.Count; i++)
            {
                EffectArgs args = new EffectArgs.DeathrattleArgs(EffectType.Deathrattle, posFiltered[i], GameLocation.Kitchen);
                await Effect.CallEffects(ingredients[i].effects, EffectType.Deathrattle, ingredients[i], game, args);
            }
        }
        public void ReplaceIngredient(int pos, Ingredient newIngr)
        {
            if (0 <= pos && pos < this._options.Count)
            {
                _options[pos] = newIngr;
            }
        }
        public Ingredient OptionAt(int pos)
        {
            if (0 <= pos && pos < this._options.Count) return _options[pos];
            else return null;
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _options;
        }
        public async Task FillEmptySpots(Game game)
        {
            this._options.RemoveAll(x => x == null);                         

            while (this._options.Count < 5)
            {
                await this.AddIngredient(game);
            }
        }
    }
}
