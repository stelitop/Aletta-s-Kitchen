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

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();
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

            throw new NotImplementedException();
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
