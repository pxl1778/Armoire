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
    class ChestPlate : Armor
    {
        public ChestPlate(int x, int y, Random r)
        {
            position = new Vector2(x, y);
            selectionRect = new Rectangle((27 * r.Next(0, 4)), 65, 20, 25);
            color = Color.FromNonPremultiplied(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), 500);
        }

        public override void DrawPickup(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, int frame, DirectionState ds)
        {
            if (frame == 0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y - 4 + 8);
            }
            if (frame == 1)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y - 6 + 8);
            }
            if (frame == 2)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y - 2 + 8);
            }
            if (frame == 3)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y - 4+8);
            }
            if (frame == 4)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y - 4+8);
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

        public void Draw(SpriteBatch sb, DirectionState ds)
        {
            position = new Vector2(MainManager.Instance.gameMan.player.pos.X , MainManager.Instance.gameMan.player.pos.Y - 2 + 8);
            if (MainManager.Instance.inputMan.Charge)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X +4, MainManager.Instance.gameMan.player.pos.Y - 4 + 15);
            }
            if (MainManager.Instance.gameMan.player.pState == PlayerState.dashing && ds == DirectionState.left)
            {
                position.X += 5;
            }
            if (ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position - new Vector2(2, 0), selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
