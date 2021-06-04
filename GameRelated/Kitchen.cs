using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.Game_Ingredients;
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

            //trigger WheneverIngredientEnterKitchen
            foreach (var kitchenIngr in game.player.kitchen.GetAllNonNullIngredients())
            {
                if (kitchenIngr == ret) continue;
                var effArgs = new EffectArgs.EnteringKitchenArgs(EffectType.WheneverIngredientEntersKitchen, newSpot);
                await Effect.CallEffects(kitchenIngr.effects, EffectType.WheneverIngredientEntersKitchen, kitchenIngr, game, effArgs);
            }
            foreach (var handIngr in game.player.hand.GetAllNonNullIngredients())
            {
                var effArgs = new EffectArgs.EnteringKitchenArgs(EffectType.WheneverIngredientEntersKitchen, newSpot);
                await Effect.CallEffects(handIngr.effects, EffectType.WheneverIngredientEntersKitchen, handIngr, game, effArgs);
            }
            //trigger WheneverIngredientEnterKitchen

            this.FillNextOption(game);

            //trigger OnEnteringKitchen
            EffectArgs args = new EffectArgs.EnteringKitchenArgs(EffectType.OnEnteringKitchen, newSpot);
            await Effect.CallEffects(ret.effects, EffectType.OnEnteringKitchen, ret, game, args);                       
            //trigger OnEnteringKitchen

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
                //trigger WheneverIngredientEnterKitchen
                foreach (var kitchenIngr in game.player.kitchen.GetAllNonNullIngredients())
                {
                    if (kitchenIngr == this._options[filledSpots[i]]) continue;
                    var effArgs = new EffectArgs.EnteringKitchenArgs(EffectType.WheneverIngredientEntersKitchen, filledSpots[i]);
                    await Effect.CallEffects(kitchenIngr.effects, EffectType.WheneverIngredientEntersKitchen, kitchenIngr, game, effArgs);
                }
                foreach (var handIngr in game.player.hand.GetAllNonNullIngredients())
                {
                    var effArgs = new EffectArgs.EnteringKitchenArgs(EffectType.WheneverIngredientEntersKitchen, filledSpots[i]);
                    await Effect.CallEffects(handIngr.effects, EffectType.WheneverIngredientEntersKitchen, handIngr, game, effArgs);
                }
                //trigger WheneverIngredientEnterKitchen

                EffectArgs args = new EffectArgs.EnteringKitchenArgs(EffectType.OnEnteringKitchen, filledSpots[i]);
                await Effect.CallEffects(this._options[filledSpots[i]].effects, EffectType.OnEnteringKitchen, this._options[filledSpots[i]], game, args);
            }
        }

        /*
         * Pick Ingredient Order:
         * 1) Clear Feedback
         * 2) Check if it can be bought
         * 3) Clear the kitchen spot
         * 4) Add to hand
         * 5) Trigger OnBeingPicked
         * 6) Check and if possible trigger Outcast effects
         * 7) Add to play history
         * 8) Trigger After you pick an ingredient
         * 9) Start next round         
         */
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

                var newIngrInfo = await game.player.hand.AddIngredient(game, ingr);                

                EffectArgs args = new EffectArgs.OnBeingPickedArgs(EffectType.WhenPicked, kitchenPos, newIngrInfo.handPos);
                await Effect.CallEffects(ingr.effects, EffectType.WhenPicked, newIngrInfo.ingredient, game, args); 
                
                if (CommonConditions.OutcastCondition(kitchenPos))
                {
                    args = new EffectArgs.OnBeingPickedArgs(EffectType.Outcast, kitchenPos, newIngrInfo.handPos);
                    await Effect.CallEffects(ingr.effects, EffectType.Outcast, newIngrInfo.ingredient, game, args);
                }

                game.player.pickHistory.Add(ingr.Copy());

                //trigger when picked effects
                foreach (var kitchenIngr in game.player.kitchen.GetAllNonNullIngredients())
                {
                    if (kitchenIngr == newIngrInfo.ingredient) continue;

                    var ingredientPickArgs = new EffectArgs.IngredientPickArgs(EffectType.AfterYouPickAnIngerdientInKitchen, newIngrInfo.ingredient);

                    await Effect.CallEffects(kitchenIngr.effects, EffectType.AfterYouPickAnIngerdientInKitchen, kitchenIngr, game, ingredientPickArgs);
                }

                foreach (var handIngr in game.player.hand.GetAllNonNullIngredients())
                {
                    if (handIngr == newIngrInfo.ingredient) continue;

                    var ingredientPickArgs = new EffectArgs.IngredientPickArgs(EffectType.AfterYouPickAnIngerdientInHand, newIngrInfo.ingredient);

                    await Effect.CallEffects(handIngr.effects, EffectType.AfterYouPickAnIngerdientInHand, handIngr, game, ingredientPickArgs);
                }

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

                await this.FillEmptySpots(game);
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

            await this.FillEmptySpots(game);
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
        public List<Ingredient> GetAllNonNullIngredients()
        {
            List<Ingredient> ret = new List<Ingredient>();

            foreach (var ingr in _options) if (ingr != null) ret.Add(ingr);

            return ret;
        }
        public List<int> GetAllIndexes()
        {
            List<int> ret = new List<int>();

            for (int i = 0; i < this._options.Count; i++) if (this._options[i] != null) ret.Add(i);

            return ret;
        }
        
    }
}
