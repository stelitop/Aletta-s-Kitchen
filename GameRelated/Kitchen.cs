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
        public Ingredient nextOption { get; private set; }

        public void Restart(IngredientPool pool)
        {
            this._options.Clear();

            for (int i=0; i<5; i++)
            {
                int pick = BotHandler.globalRandom.Next(pool.ingredients.Count);

                _options.Add(pool.ingredients[pick].Copy());
            }

            this.nextOption = pool.ingredients[BotHandler.globalRandom.Next(pool.ingredients.Count)].Copy();
        }
        public async Task<Ingredient> AddIngredient(Game game)
        {
            if (this._options.Count >= 5) return null;

            int pick = BotHandler.globalRandom.Next(game.pool.ingredients.Count);

            Ingredient ret = game.pool.ingredients[pick].Copy();

            _options.Add(ret);

            EffectArgs args = new EffectArgs(EffectType.OnEnteringKitchen);
            await Effect.CallEffects(ret.effects, EffectType.OnEnteringKitchen, ret, game, args);

            return ret;
        }
        public async Task BuyIngredient(Game game, int pos)
        {
            if (0 <= pos && pos < this._options.Count && game.player.hand.ingredients.Count < 3)
            {
                Ingredient ingr = this._options[pos];

                EffectArgs args = new EffectArgs(EffectType.OnBeingPicked);
                await Effect.CallEffects(ingr.effects, EffectType.OnBeingPicked, ingr, game, args);

                this._options.RemoveAt(pos);

                game.player.hand.ingredients.Add(ingr);

                await this.FillEmptySpots(game);
            }
        }
        public async Task DestroyIngredient(Game game, int pos)
        {
            if (0 <= pos && pos < this._options.Count)
            {
                Ingredient ingr = this._options[pos];

                EffectArgs args = new EffectArgs(EffectType.Deathrattle);
                await Effect.CallEffects(ingr.effects, EffectType.Deathrattle, ingr, game, args);

                this._options.RemoveAt(pos);

                await this.FillEmptySpots(game);
            }
        }
        public Ingredient OptionAt(int pos)
        {
            if (0 <= pos && pos < this._options.Count) return _options[pos];
            else return null;
        }
        public async Task FillEmptySpots(Game game)
        {
            while (this._options.Count < 5)
            {
                await this.AddIngredient(game);
            }
        }
    }
}
