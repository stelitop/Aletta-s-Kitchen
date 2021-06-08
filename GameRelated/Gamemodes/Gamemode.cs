using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated.Gamemodes
{
    public abstract partial class Gamemode
    {
        public string title;
        public string description;

        public Gamemode()
        {
            title = description = string.Empty;
        }
        public Gamemode(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
        public abstract Task ApplyGamemodeSettings(Game game);
    }
}
