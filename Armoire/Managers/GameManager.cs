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
        public List<Platform> platforms;
        public GameState gState;

        public GameManager()
        {
            player = new Player();
            platforms = new List<Platform>();
            InitializePlatforms();
        }

        public void InitializePlatforms()
        {
            platforms.Add(new Platform(0, 400, 500, 300));
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
                    MainManager.Instance.uiMan.PushScreen(new PauseScreen());
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
            foreach(Platform p in platforms)
            {
                p.Draw(sb);
            }
        }
    }
}
