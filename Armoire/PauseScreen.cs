using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Armoire
{
    class PauseScreen : IScreen
    {

        public void Draw()
        {
            MainManager.Instance.main.spriteBatch.Begin();
            MainManager.Instance.main.spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, "we out here", new Vector2(100, 100), Color.Black);
            MainManager.Instance.main.spriteBatch.End();
        }
        
        public void Update()
        {

        }
    }
}
