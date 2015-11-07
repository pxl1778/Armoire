using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{
    enum GameState
    {
        title, game, paused, map_editor
    }

    class GameManager
    {
        //Fields
        public Player player;
        public List<Platform> platforms;
        public List<Armor> armor;
        public GameState gState;

        public GameManager()
        {
            player = new Player();
            platforms = new List<Platform>();
            armor = new List<Armor>();
            Initialize();
        }

        public void Initialize()
        {
            //platforms.Add(new Platform(0, 400, 500, 300));
            BinaryReader br = new BinaryReader(new FileStream("Content/demo.map", FileMode.Open));
            if (br.ReadUInt32() != 0x49474A50)
                throw new Exception("Invalid map file magic!");

            int platformCount = br.ReadInt32();
            for (int i = 0; i < platformCount; i++ )
            {
                platforms.Add(new Platform(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
            }
            br.Close();

            armor.Add(new Helmet(250, 250));
        }

        public void Update()
        {
            player.Update();

            MainManager.Instance.uiMan.Update();

            // Do pause menu
            if (MainManager.Instance.inputMan.Pause)
                if (gState != GameState.paused)
                {
                    //gState = GameState.paused;
                    MainManager.Instance.uiMan.PushScreen(new PauseScreen());
                }
                else
                {
                    //gState = GameState.game;
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
            foreach(Armor a in armor)
            {
                a.Draw(sb);
            }
        }
    }
}
