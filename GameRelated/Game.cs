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

        public void Start()
        {
            throw new NotImplementedException();
        }
        public void Cook()
        {
            throw new NotImplementedException();
        }

    }
}
