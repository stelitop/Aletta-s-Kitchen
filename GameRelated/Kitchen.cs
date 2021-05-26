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
        public Ingredient nextOption { get; private set; } = new Ingredient();
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
        public async Task PickIngredient(Game game, int pos)
        {
            if (0 <= pos && pos < this._options.Count)
            {
                game.feedback.Clear();

                if (!this._options[pos].CanBeBought(game, pos))
                {
                    game.feedback.Add($"{this._options[pos].name} can't be bought currently!");
                    return;
                }

                int newSpot = await game.ChooseAHandSpot();
                
                Ingredient ingr = this._options[pos];

                this._options.RemoveAt(pos);

                EffectArgs args = new EffectArgs(EffectType.OnBeingPicked);
                await Effect.CallEffects(ingr.effects, EffectType.OnBeingPicked, ingr, game, args);
                
                game.player.hand.ingredients[newSpot] = ingr;                

                game.player.pickHistory.Add(ingr.Copy());

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
        public List<Ingredient> GetAllIngredients()
        {
            return _options;
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
