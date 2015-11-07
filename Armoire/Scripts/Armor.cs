using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{

    abstract class Armor
    {
        public Vector2 position;
        public Rectangle selectionRect;
        public Color color;

        public Armor()
        {
            
        }

        public virtual void DrawPickup(SpriteBatch sb)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
