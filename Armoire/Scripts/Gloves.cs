using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{
    class Gloves : Armor
    {
        public Gloves(int x, int y)
        {
            Random r = new Random();
            position = new Vector2(x, y);
            selectionRect = new Rectangle((22 * r.Next(0, 4)), 49, 22, 16);
            color = Color.FromNonPremultiplied(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), 500);
        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
