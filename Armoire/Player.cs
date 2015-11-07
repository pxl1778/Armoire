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
        private int x;
        private int y;
        private int width;
        private int height;
        private Rectangle rect;
        
        //Properties
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Rectangle Rect { get { return rect; } set { rect = value; } }

        public Player()
        {
            x = MainManager.Instance.main.GraphicsDevice.Viewport.Width / 2;
            y = MainManager.Instance.main.GraphicsDevice.Viewport.Height / 2;
            width = 50;
            height = 70;
        }
    }
}
