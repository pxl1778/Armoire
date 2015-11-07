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
        title, game, paused
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

        public void Update()
        {
            player.Update();

            MainManager.Instance.uiMan.Update();

            // Do pause menu
            if (MainManager.Instance.inputMan.Pause)
                if (gState != GameState.paused)
                {
                    gState = GameState.paused;
                    MainManager.Instance.uiMan.PushScreen(new TestMenu());
                }
                else
                {
                    gState = GameState.game;
                    MainManager.Instance.uiMan.PopScreen();
                }

        }

        public void Draw(SpriteBatch sb)
        {
            player.Draw(sb);
        }
    }
}
