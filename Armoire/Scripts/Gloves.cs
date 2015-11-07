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
        public Gloves(int x, int y, Random r)
        {
            position = new Vector2(x, y);
            selectionRect = new Rectangle((27 * r.Next(0, 2))-1, 90, 24, 18);
            color = Color.FromNonPremultiplied(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), 500);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, DirectionState ds)
        {
            position = new Vector2(MainManager.Instance.gameMan.player.pos.X-3, MainManager.Instance.gameMan.player.pos.Y - 4 + 15);
            if (ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.FlipHorizontally, 0);
            }
        }

        public void Draw(SpriteBatch sb, float frame, DirectionState ds)
        {
            if (frame == 0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 4 + 15);
            }
            if (frame == 1)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 6 + 15);
            }
            if (frame == 2)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 2 + 15);
            }
            if (ds == DirectionState.right)
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
