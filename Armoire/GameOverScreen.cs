using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Armoire
{
    class RestartGameButton : LinkedMenuItem
    {
        public RestartGameButton(LinkedMenuScreen screen) : base(screen) { }
        public RestartGameButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }

        public override void Draw()
        {
            if (!this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Game", new Vector2(300, 300), Color.Black);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Start Game", new Vector2(300, 300), Color.CadetBlue);
        }

        public override void Update()
        {
            base.Update();

            if (!this.Selected())
                return;

            if (MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.gameMan = new GameManager();
                MainManager.Instance.gameMan.gState = GameState.game;
                MainManager.Instance.uiMan.PopScreen();
            }
        }
    }

    public class QuitToMenuButton : LinkedMenuItem
    {
        public QuitToMenuButton(LinkedMenuScreen screen) : base(screen) { }
        public QuitToMenuButton(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
            : base(screen, left, right, up, down) { }


        public override void Draw()
        {
            if (this.Selected())
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Main Menu", new Vector2(300, 320), Color.CadetBlue);
            else
                MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Main Menu", new Vector2(300, 320), Color.Black);
        }

        public override void Update()
        {
            base.Update();

            if (this.Selected() && MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump)
            {
                MainManager.Instance.uiMan.PopScreen();
                MainManager.Instance.uiMan.PushScreen(new TitleScreen());
            }
        }
    }

    class GameOverScreen : LinkedMenuScreen
    {
        List<LinkedMenuItem> menuItems;

        public GameOverScreen()
        {
            menuItems = new List<LinkedMenuItem>();

            // Create our buttons
            StartGameButton restartButton = new StartGameButton(this);
            QuitToMenuButton menuButton = new QuitToMenuButton(this);

            // Link them up
            restartButton.Down = menuButton;
            menuButton.Up = restartButton;

            // Add items to the menu list
            menuItems.Add(restartButton);
            menuItems.Add(menuButton);

            // Select the default item
            this.SelectItem(restartButton);
        }


        public override void Draw()
        {
            MainManager.Instance.main.GraphicsDevice.Clear(Color.DarkKhaki);
            base.Draw();

            MainManager.Instance.main.spriteBatch.Begin();
            MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "Game Over", new Vector2(50, 50), Color.Black, 0, new Vector2(0, 0), 5, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

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
