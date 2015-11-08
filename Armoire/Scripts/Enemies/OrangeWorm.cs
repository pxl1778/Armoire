using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Armoire
{
    class OrangeWorm : Enemy
    {
        public OrangeWorm(int x, int y) : base(x, y) 
        {
            speed = 1f;
            rect.Height *= 2;
            rect.Width *= 2;
            scale = 2;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        152,
                                                                        34,
                                                                        25), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }
    }
}
