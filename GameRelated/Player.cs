using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Player
    {
        public Hand hand;
        public Kitchen kitchen;

        public int curPoints;
        public List<Ingredient> buyHistory;
        public List<Ingredient> cookHistory;

        public Player()
        {
            this.hand = new Hand();
            this.kitchen = new Kitchen();

            this.curPoints = 0;
            this.buyHistory = new List<Ingredient>();
            this.cookHistory = new List<Ingredient>();
        }
    }
}
