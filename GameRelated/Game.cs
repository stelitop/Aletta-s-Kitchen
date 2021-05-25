using Aletta_s_Kitchen.GameRelated.GoalTypes;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Game
    {
        public int curRound;
        public IngredientPool pool;
        public Player player;

        public Queue<Goal> goals;

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();

            this.goals = new Queue<Goal>();
        }

        public async Task Start(IngredientPool pool)
        {
            this.curRound = 1;            
            this.pool = new IngredientPool(pool);

            this.player = new Player();

            await this.player.kitchen.FillEmptySpots(this);
        }

        public void NextRound()
        {
            this.curRound++;

            if (this.goals.Count > 0)
            {
                while (this.curRound >= this.goals.Peek().round)
                {
                    if (this.curRound > this.goals.Peek().round)
                    {
                        this.goals.Dequeue();
                    }
                    else if (this.curRound == this.goals.Peek().round)
                    {
                        bool result = this.goals.Peek().IsGoalFulfilled(this);

                        if (!result)
                        {
                            this.EndGame();
                            return;
                        }
                    }

                    if (this.goals.Count == 0) break;
                }
            }            
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public async Task<int> ChooseAHandSpot()
        {
            throw new NotImplementedException();
        }
    }
}
