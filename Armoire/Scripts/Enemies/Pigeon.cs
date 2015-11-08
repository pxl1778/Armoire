using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Armoire
{
    class Pigeon : Enemy
    {
        public Pigeon(int x, int y)
            : base(x, y)
        {
            speed = 2f;
            rect.Width *= 5;
            rect.Height *= 5;
            scale = 5;
            armor.Push(new Helmet(0, 0, new Random()));
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        175,
                                                                        33,
                                                                        42), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0);
            if(armor.Count != 0)
            {
                if(armor.Peek() is Helmet)
                {
                    armor.Peek().Draw(sb, this);
                }
            }
        }
    }
}