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
            sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, MainManager.Instance.gameMan.player.armorScale + .2f, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb, float frame, DirectionState ds)
        {
            float s = MainManager.Instance.gameMan.player.armorScale + .2f;
            if (frame == 0)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y + 5*s);
            }
            if (frame == 1)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y + 3*s);
            }
            if (frame == 2)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X -3, MainManager.Instance.gameMan.player.pos.Y + 7*s);
            }
            if (frame == 3)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X-3, MainManager.Instance.gameMan.player.pos.Y +5*s);
            }
            if (frame == 4)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X-3, MainManager.Instance.gameMan.player.pos.Y +5*s);
            }
            if (ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, MainManager.Instance.gameMan.player.armorScale + .2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, MainManager.Instance.gameMan.player.armorScale + .2f, SpriteEffects.FlipHorizontally, 0);

            }
        }

        public void Draw(SpriteBatch sb, DirectionState ds)
        {
            float s = MainManager.Instance.gameMan.player.armorScale + .2f;
            position = new Vector2(MainManager.Instance.gameMan.player.pos.X - 1, MainManager.Instance.gameMan.player.pos.Y +6 * s);
            if (MainManager.Instance.inputMan.Charge)
            {
                position = new Vector2(MainManager.Instance.gameMan.player.pos.X, MainManager.Instance.gameMan.player.pos.Y +11 * s);
            }
            if (MainManager.Instance.gameMan.player.pState == PlayerState.dashing && ds == DirectionState.left)
            {
                position.X += 5;
            }
            if (ds == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position, selectionRect, color, 0, Vector2.Zero, MainManager.Instance.gameMan.player.armorScale + .2f, SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, position - new Vector2(5, 0), selectionRect, color, 0, Vector2.Zero, MainManager.Instance.gameMan.player.armorScale + .2f, SpriteEffects.FlipHorizontally, 0);
            }
        }

    }
}
