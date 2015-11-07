using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{
    enum GameState
    {
        title, game
    }

    class GameManager
    {
        //Fields
        public Player player;

        public GameState gState;

        public GameManager()
        {
            player = new Player();
        }

        public void Draw(SpriteBatch sb)
        {
            player.Draw(sb);
        }
    }
}
