using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Armoire
{
    class Player
    {
        //Fields
        private int width;
        private int height;
        private Rectangle rect;
        public Vector2 pos;
        public Vector2 velocity;
        
        //Properties
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Rectangle Rect { get { return rect; } set { rect = value; } }

        public Player()
        {
            pos = new Vector2(MainManager.Instance.main.GraphicsDevice.Viewport.Width / 2, MainManager.Instance.main.GraphicsDevice.Viewport.Height / 2);
            velocity = new Vector2(0, 0);
            width = 50;
            height = 70;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.rectTexture, rect, Color.Blue);
        }
    }
}
