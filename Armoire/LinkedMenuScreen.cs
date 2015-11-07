using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Armoire
{
    public abstract class LinkedMenuItem
    {
        private LinkedMenuItem left;
        private LinkedMenuItem right;
        private LinkedMenuItem up;
        private LinkedMenuItem down;
        private LinkedMenuScreen screen;

        public LinkedMenuItem Left { get { return left; } set { this.left = value; } }
        public LinkedMenuItem Right { get { return right; } set { this.right = value; } }
        public LinkedMenuItem Up { get { return up; } set { this.up = value; } }
        public LinkedMenuItem Down { get { return down; } set { this.down = value; } }

        public Boolean Selected()
        {
           return screen.selectedItem == this;
        }

        public LinkedMenuItem(LinkedMenuScreen screen)
        {
            this.screen = screen;
        }

        public LinkedMenuItem(LinkedMenuScreen screen, LinkedMenuItem left, LinkedMenuItem right, LinkedMenuItem up, LinkedMenuItem down)
        {
            this.screen = screen;
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }

        public virtual void Update()
        {
            if(!Selected())
                return;

            if (MainManager.Instance.inputMan.MoveLeft && !MainManager.Instance.inputMan.PrevMoveLeft)
                screen.SelectItem(this.left);
            if (MainManager.Instance.inputMan.MoveRight && !MainManager.Instance.inputMan.PrevMoveRight)
                screen.SelectItem(this.right);
            if (MainManager.Instance.inputMan.MoveUp && !MainManager.Instance.inputMan.PrevMoveUp)
                screen.SelectItem(this.up);
            if (MainManager.Instance.inputMan.MoveDown && !MainManager.Instance.inputMan.PrevMoveDown)
                screen.SelectItem(this.down);

        }

        public virtual void Draw()
        {

        }

        //public void 
    }

    public abstract class LinkedMenuScreen : IScreen
    {
        public LinkedMenuItem selectedItem;
        //List<LinkedMenuItem> menuItems;

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }

        public void SelectItem(LinkedMenuItem item)
        {
            if(item != null)
                this.selectedItem = item;
        }
    }

    class TestMenu : LinkedMenuScreen
    {
        List<LinkedMenuItem> menuItems;

        public TestMenu()
        {
            LinkedMenuItem cb = new MainMenuButton(this);
            LinkedMenuItem eb = new ExitButton(this, null, null, cb, null);
            cb.Down = eb;
            menuItems = new List<LinkedMenuItem>();
            menuItems.Add(cb);
            menuItems.Add(eb);
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
