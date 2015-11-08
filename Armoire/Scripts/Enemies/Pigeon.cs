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
        public Pigeon(int x, int y, Random r)
            : base(x, y)
        {
            speed = 2f;
            scale = 5;
            rect = new Rectangle((int)pos.X, (int)pos.Y, (int)(33 * scale), (int)(42 * scale));
            armor.Push(new Helmet(0, 0, r));
        }

        public override void Draw(SpriteBatch sb)
        {
            if(dir == -1)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        175,
                                                                        33,
                                                                        42), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        175,
                                                                        33,
                                                                        42), Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.FlipHorizontally, 0);
            }
            
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