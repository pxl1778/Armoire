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
        public List<Enemy> enemies;
        public List<Armor> armorPickups;
        public Enemy toRemove;
        public Random r;
        public GameState gState;

        public GameManager()
        {
            r = new Random();
            player = new Player(r);
            platforms = new List<Platform>();
            armor = new List<Armor>();
            enemies = new List<Enemy>();
            armorPickups = new List<Armor>();
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
            
            int armorPickupCount = br.ReadInt32();
            armorPickups = new List<Armor>();
            for (int i = 0; i < armorPickupCount; i++ )
            {
                switch(br.ReadByte())
                {
                    case 1:
                        Gloves g = new Gloves((int)br.ReadInt32(), (int)br.ReadInt32(), new Random());
                        armorPickups.Add(g);
                        break;
                    case 2:
                        ChestPlate c = new ChestPlate((int)br.ReadInt32(), (int)br.ReadInt32(), new Random());
                        armorPickups.Add(c);
                        break;
                    case 3:
                        Helmet h = new Helmet((int)br.ReadInt32(), (int)br.ReadInt32(), new Random());
                        armorPickups.Add(h);
                        break;
                }
            }

            int enemyCount = br.ReadInt32();
            for (int i = 0; i < enemyCount; i++ )
            {
                switch(br.ReadByte())
                {
                    case 1:
                        enemies.Add(new Pigeon(br.ReadInt32(), br.ReadInt32(), r));
                        break;
                    case 2:
                        enemies.Add(new OrangeWorm(br.ReadInt32(), br.ReadInt32()));
                        break;
                    case 3:
                        enemies.Add(new Enemy(br.ReadInt32(), br.ReadInt32()));
                        break;
                }
            }
            br.Close();
            enemies.Add(new Pigeon(1500, 650, r));
            armorPickups.Add(new Gloves(627, 403, player.rand));
        }

        public void Update()
        {
            player.Update();

            MainManager.Instance.uiMan.Update();
            foreach(Enemy e in enemies)
            {
                e.Update();
            }

            MainManager.Instance.discardMan.Update();

            // Do pause menu
            if (MainManager.Instance.inputMan.Pause)
                if (gState != GameState.paused)
                {
                    //gState = GameState.paused;
                    if(!(MainManager.Instance.uiMan.Top() is PauseScreen))
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
            sb.Draw(MainManager.Instance.drawMan.levelBackground, new Vector2(0, 0), new Rectangle(0, 0, 3300, 1650), Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            player.Draw(sb);
            if (gState == GameState.map_editor)
            {
                foreach (Platform p in platforms)
                {
                    p.Draw(sb);
                }
            }
            foreach(Armor a in armor)
            {
                a.Draw(sb);
            }
            foreach(Enemy e in enemies)
            {
                e.Draw(sb);
            }
            foreach(Armor ap in armorPickups)
            {
                ap.DrawPickup(sb);
            }
        }
    }
}
