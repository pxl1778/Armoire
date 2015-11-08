﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Armoire
{
    class DrawManager
    {
        //Fields
        public Texture2D rectTexture;
        public Texture2D spritesheet;
        public Texture2D uiSpritesheet;
        public Texture2D titleScreenImage;
        public Texture2D levelBackground;
        public List<Texture2D> armorTextures;
        public SpriteFont gameFont;

        public DrawManager()
        {
            rectTexture = new Texture2D(MainManager.Instance.main.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch sb)
        {
            MainManager.Instance.gameMan.Draw(sb);
        }
    }
}
