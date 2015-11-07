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
    class Helmet : Armor
    {
        public Helmet(int x, int y)
        {
            Random r = new Random();
            position = new Vector2(x, y);
            selectionRect = new Rectangle((22 * r.Next(0, 4)), 49, 22, 16);
            color = Color.FromNonPremultiplied(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), 500);
        }

        public void Draw(SpriteBatch sb, int frame, DirectionState ds)
        {
            if(frame ==0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X , MainManager.Instance.gameMan.player.pos.Y-4);
            }
            if(frame == 1)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X , MainManager.Instance.gameMan.player.pos.Y-6);
            }
            if (frame == 2)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X , MainManager.Instance.gameMan.player.pos.Y-2);
            }
            if(ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.FlipHorizontally, 0);

            }
        }

        public void Draw(SpriteBatch sb, DirectionState ds)
        {
            position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y-4);
            if(ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.FlipHorizontally, 0);
            }

        }
    }
}
