using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public class Player
    {
        public string name;

        public Hand hand;
        public Kitchen kitchen;

        public int curPoints;
        public List<Ingredient> pickHistory;
        public List<Ingredient> cookHistory;

        public List<List<Ingredient>> dishHistory;

        public Player()
        {
            this.name = "Player";

            this.hand = new Hand();
            this.kitchen = new Kitchen();

            this.curPoints = 0;
            this.pickHistory = new List<Ingredient>();
            this.cookHistory = new List<Ingredient>();
            this.dishHistory = new List<List<Ingredient>>();
        }        
    }
}
