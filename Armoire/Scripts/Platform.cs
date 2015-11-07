using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{
    class Platform
    {
        public Rectangle rect;

        public Platform(int x, int y, int width, int height)
        {
            rect = new Rectangle(x, y, width, height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.rectTexture, rect, Color.Red);
        }

        public bool Collide(Vector2 pos)
        {
            if(rect.X<pos.X && rect.X + rect.Width > pos.X && rect.Y <pos.Y && rect.Y + rect.Height > pos.Y)
            {
                return true;
            }
            return false;
        }
    }
}
