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
        public int OptionsCount { get { return _options.Count; } }        

        public Kitchen()
        {
            this._options = new List<Ingredient>();
        }

        public async Task Restart(Game game)
        {
            this._options.Clear();

            this.nextOption = game.pool.ingredients[BotHandler.globalRandom.Next(game.pool.ingredients.Count)].Copy();

            await this.FillEmptySpots(game);
        }
        private void FillNextOption(Game game)
        {

            this.nextOption = game.pool.GetRandomIngredient();
        }
        public async Task<Ingredient> AddIngredient(Game game)
        {
            int newSpot = -1;
            
            for (int i=0; i<this._options.Count; i++)
            {
                if (this._options[i] == null)
                {
                    newSpot = i;
                    break;
                }
            }
            if (newSpot == -1 && this._options.Count < 5)
            {
                this._options.Add(null);
                newSpot = this._options.Count - 1;
            }
            if (newSpot == -1) return null;


            Ingredient ret = this.nextOption;
            ret.roundEntered = game.curRound;
            _options[newSpot] = ret;
            this.FillNextOption(game);

            EffectArgs args = new EffectArgs(EffectType.OnEnteringKitchen);
            await Effect.CallEffects(ret.effects, EffectType.OnEnteringKitchen, ret, game, args);                       

            return ret;
        }        
        public async Task FillEmptySpots(Game game)
        {
            //could add this part back to go back to adding at the end of the line
            //this._options.RemoveAll(x => x == null);

            //while (this._options.Count < 5)
            //{
            //    await this.AddIngredient(game);
            //    this._options.RemoveAll(x => x == null);
            //}

            List<int> filledSpots = new List<int>();

            for (int i=0; i<this._options.Count; i++)
            {
                if (this._options[i] == null)
                {                   
                    Ingredient newIngr = this.nextOption;
                    newIngr.roundEntered = game.curRound;
                    _options[i] = newIngr;

                    this.FillNextOption(game);

                    filledSpots.Add(i);
                }
            }

            while (this._options.Count < 5)
            {
                Ingredient newIngr = this.nextOption;
                newIngr.roundEntered = game.curRound;
                this._options.Add(newIngr);

                this.FillNextOption(game);

                filledSpots.Add(this._options.Count - 1);
            }

            for (int i=0; i<filledSpots.Count; i++)
            {
                EffectArgs args = new EffectArgs(EffectType.OnEnteringKitchen);
                await Effect.CallEffects(this._options[filledSpots[i]].effects, EffectType.OnEnteringKitchen, this._options[filledSpots[i]], game, args);
            }
        }
        public async Task PickIngredient(Game game, int kitchenPos)
        {
            if (0 <= kitchenPos && kitchenPos < this._options.Count)
            {
                game.feedback.Clear();

                if (!this._options[kitchenPos].CanBeBought(game, kitchenPos))
                {
                    game.feedback.Add($"{this._options[kitchenPos].name} can't be bought currently!");
                    return;
                }

                if (!game.player.hand.AvailableSpot())
                {
                    game.feedback.Add("Your dish is full!");
                    return;
                }
                
                Ingredient ingr = this._options[kitchenPos];

                this._options[kitchenPos] = null;

                var newIngrInfo = game.player.hand.AddIngredient(ingr);

                EffectArgs args = new EffectArgs.OnBeingPickedArgs(EffectType.OnBeingPicked, kitchenPos, newIngrInfo.handPos);
                await Effect.CallEffects(ingr.effects, EffectType.OnBeingPicked, newIngrInfo.ingredient, game, args);                

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
        
    }
}
