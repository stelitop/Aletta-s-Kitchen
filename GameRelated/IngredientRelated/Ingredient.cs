using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.IngredientRelated
{
    public class Ingredient : IComparable<Ingredient>
    {
        public string name;
        public int points;
        public Rarity rarity;
        public Tribe tribe;
        public string text;

        public List<Effect> effects;

        public Ingredient()
        {
            this.name = "Default Ingredient";
            this.rarity = Rarity.None;
            this.tribe = Tribe.NoTribe;
            this.points = 1;
            this.text = string.Empty;
            this.effects = new List<Effect>();
        }

        public Ingredient(string name, int points, Rarity rarity, Tribe tribe = Tribe.NoTribe, string cardText = "")
        {
            this.name = name;
            this.points = points;
            this.rarity = rarity;
            this.tribe = tribe;
            this.text = cardText;
            this.effects = new List<Effect>();
        }

        public virtual bool CanBeBought(Game game, int kitchenPos)
        {
            return true;
        }

        public void Buy(Game game, int kitchenPos)
        {
            throw new NotImplementedException();
        }

        public virtual Ingredient Copy()
        {
            Ingredient ret = (Ingredient)Activator.CreateInstance(this.GetType());

            ret.name = this.name;
            ret.points = this.points;
            ret.tribe = this.tribe;

            return ret;
        }

        public virtual string GetInfo()
        {
            string ret = string.Empty;
            ret = $"{this.name} - {this.points}p";

            if (this.tribe != Tribe.NoTribe) ret += $" - {this.tribe}";
            ret += $" - { this.rarity}";
            if (!this.text.Equals(string.Empty)) ret += $" - {this.text}";
            return ret;
        }

        public int CompareTo(Ingredient other)
        {
            if (this.rarity > other.rarity) return -1;
            else if (this.rarity < other.rarity) return 1;

            if (this.points > other.points) return -1;
            else if (this.points < other.points) return 1;

            return this.name.CompareTo(other.name);
        }
    }
}
