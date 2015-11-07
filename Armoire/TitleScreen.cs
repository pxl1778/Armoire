using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Armoire
{
    class StartGameButton : LinkedMenuItem
    {
        public StartGameButton(LinkedMenuScreen screen) : base(screen) { }
        public StartGameButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if(!this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Game", new Vector2(300, 300), Color.Black);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Game", new Vector2(300, 300), Color.CadetBlue);
        }

        public override void Update()
        {
            base.Update();

            if (!this.Selected())
                return;

            if(MainManager.Instance.inputMan.Jump && ! MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.gameMan = new GameManager();
                MainManager.Instance.gameMan.gState = GameState.game;
                MainManager.Instance.uiMan.PopScreen();
            }
        }
    }

    class StartEditorButton : LinkedMenuItem
    {
        public StartEditorButton(LinkedMenuScreen screen) : base(screen) { }
        public StartEditorButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if (!this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Map Editor", new Vector2(300, 320), Color.Black);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Map Editor", new Vector2(300, 320), Color.CadetBlue);
        }

        public override void Update()
        {
            base.Update();

            if (!this.Selected())
                return;

            if (MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.gameMan = new GameManager();
                MainManager.Instance.gameMan.gState = GameState.map_editor;
                MainManager.Instance.uiMan.PopScreen();
            }
        }
    }

    class HelpButton : LinkedMenuItem
    {
        public HelpButton(LinkedMenuScreen screen) : base(screen) { }
        public HelpButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if (!this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "How to Play", new Vector2(300, 340), Color.Black);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "How to Play", new Vector2(300, 340), Color.CadetBlue);
        }
    }

    class TitleScreen : LinkedMenuScreen
    {
        List<LinkedMenuItem> menuItems;

        public TitleScreen()
        {
            // Create all our menu items
            menuItems = new List<LinkedMenuItem>();
            StartGameButton sgb = new StartGameButton(this);
            StartEditorButton seb = new StartEditorButton(this);
            HelpButton hb = new HelpButton(this);

            // Link up the menu items
            sgb.Down = seb;
            seb.Up = sgb;
            seb.Down = hb;
            hb.Up = seb;
            
            // Add the menu items to our collection
            menuItems.Add(seb);
            menuItems.Add(sgb);
            menuItems.Add(hb);

            // Select start game by default
            this.SelectItem(sgb);
        }

        public override void Draw()
        {
            MainManager.Instance.main.GraphicsDevice.Clear(Color.DarkKhaki);
            base.Draw();

            MainManager.Instance.main.spriteBatch.Begin();
            MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Armoire", new Vector2(50, 50), Color.Black, 0, new Vector2(0,0), 5, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

            foreach (LinkedMenuItem i in menuItems)
                i.Draw();

            MainManager.Instance.main.spriteBatch.End();
        }

        public override void Update()
        {
            base.Update();

            foreach (LinkedMenuItem i in menuItems)
                i.Update();
        }
    }
}
