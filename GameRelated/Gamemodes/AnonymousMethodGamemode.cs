using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.Gamemodes
{
    public partial class Gamemode
    {
        public class AnonymousMethodGamemode : Gamemode
        {
            private GamemodeSettings settings;
            public AnonymousMethodGamemode(string title, string description, GamemodeSettings gs) : base(title, description)
            {
                this.settings = gs;
            }

            public override Task ApplyGamemodeSettings(Game game)
            {
                this.settings(game);

                return Task.CompletedTask;
            }
        }

        public delegate void GamemodeSettings(Game game);
    }    
}
