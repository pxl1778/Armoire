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
            selectionRect = new Rectangle((30 * r.Next(0, 4)), 97, 28, 25);
            color = Color.FromNonPremultiplied(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255), 500);
        }

        
        public override void DrawPickup(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
        }
         

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, DirectionState ds)
        {
            position = new Vector2(MainManager.Instance.gameMan.player.pos.X-3, MainManager.Instance.gameMan.player.pos.Y - 4 + 9);
            if (MainManager.Instance.gameMan.player.pState == PlayerState.charging)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X , MainManager.Instance.gameMan.player.pos.Y - 4 + 15);
            }
            if (MainManager.Instance.gameMan.player.velocity.Y >= 0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X-1, MainManager.Instance.gameMan.player.pos.Y - 2 + 8);
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

        public void Draw(SpriteBatch sb, float frame, DirectionState ds)
        {
            if (frame == 0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 4 + 9);
            }
            if (frame == 1)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 6 + 9);
            }
            if (frame == 2)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y - 2 + 9);
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
