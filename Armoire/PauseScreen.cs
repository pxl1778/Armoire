using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace Armoire
{

    public class MainMenuButton : LinkedMenuItem
    {
        public MainMenuButton(LinkedMenuScreen screen) : base(screen) { }
        public MainMenuButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }


        public override void Draw()
        {
            if (this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Main Menu", new Vector2(100, 100), Color.CadetBlue);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Main Menu", new Vector2(100, 100), Color.Black);
        }

        public override void Update()
        {
            base.Update();

            if(this.Selected() && MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.uiMan.PopScreen();
                MainManager.Instance.uiMan.PushScreen(new TitleScreen());
            }
        }
    }

    public class ExitButton : LinkedMenuItem
    {
        public ExitButton(LinkedMenuScreen screen) : base(screen) { }
        public ExitButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if (this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Exit", new Vector2(100, 125), Color.CadetBlue);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Exit", new Vector2(100, 125), Color.Black);
        }

        public override void Update()
        {
            if (!Selected())
                return;
            base.Update();
            if (MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.gameMan.gState = GameState.game;
                MainManager.Instance.uiMan.PopScreen();
            }
        }
    }

    public class SaveMapButton : LinkedMenuItem
    {
        public SaveMapButton(LinkedMenuScreen screen) : base(screen) { }
        public SaveMapButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if (this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Save Map", new Vector2(100, 150), Color.CadetBlue);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Save Map", new Vector2(100, 150), Color.Black);
        }

        public override void Update()
        {
            if (!Selected())
                return;
            base.Update();
            if (MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                BinaryWriter bw = new BinaryWriter(new FileStream("Maps/new.map", FileMode.Create));
                bw.Write(0x49474A50);
                bw.Write(MainManager.Instance.gameMan.platforms.Count);
                foreach (Platform p in MainManager.Instance.gameMan.platforms)
                {
                    bw.Write(p.rect.X);
                    bw.Write(p.rect.Y);
                    bw.Write(p.rect.Width);
                    bw.Write(p.rect.Height);
                }
                bw.Write(MainManager.Instance.gameMan.armorPickups.Count);
                foreach(Armor a in MainManager.Instance.gameMan.armorPickups)
                {
                    if (a is Gloves)
                        bw.Write((byte)1);
                    else if (a is ChestPlate)
                        bw.Write((byte)2);
                    else if (a is Helmet)
                        bw.Write((byte)3);
                    bw.Write((int)a.position.X);
                    bw.Write((int)a.position.Y);
                }
                bw.Close();
            }
        }
    }

    class PauseScreen : LinkedMenuScreen
    {

        List<LinkedMenuItem> menuItems;

        public PauseScreen()
        {
            LinkedMenuItem cb = new MainMenuButton(this);
            LinkedMenuItem eb = new ExitButton(this, null, null, cb, null);
            cb.Down = eb;
            menuItems = new List<LinkedMenuItem>();
            menuItems.Add(cb);
            menuItems.Add(eb);
            if(MainManager.Instance.gameMan.gState == GameState.map_editor)
            {
                LinkedMenuItem smb = new SaveMapButton(this);
                eb.Down = smb;
                smb.Up = eb;
                menuItems.Add(smb);
            }
            this.selectedItem = cb;
        }

        public override void Draw()
        {
            MainManager.Instance.main.spriteBatch.Begin();
            foreach (LinkedMenuItem i in menuItems)
            {
                i.Draw();
            }
            MainManager.Instance.main.spriteBatch.End();
        }

        public override void Update()
        {
            if(MainManager.Instance.inputMan.Charge && !MainManager.Instance.inputMan.PrevCharge)
            {
                MainManager.Instance.gameMan.gState = GameState.game;
                MainManager.Instance.uiMan.PopScreen();
            }
            foreach (LinkedMenuItem i in menuItems)
            {
                i.Update();
            }
        }
    }
}
