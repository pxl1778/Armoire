using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Armoire
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Camera cam;
        public GameTime gameTime;
        bool upperRightNow = true;
        Vector2 upperRight;
        Vector2 lowerLeft;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            MainManager.init(this);
            base.Initialize();
            
            this.IsMouseVisible = true;
            cam = new Camera(this, MainManager.Instance.gameMan.player.Rect);
            cam.Position = MainManager.Instance.gameMan.player.pos;
            MainManager.Instance.uiMan.PushScreen(new TitleScreen());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MainManager.Instance.drawMan.spritesheet = Content.Load<Texture2D>("Assets/spritesheet.png");
            MainManager.Instance.drawMan.levelBackground = Content.Load<Texture2D>("Assets/level1.png");
            MainManager.Instance.drawMan.gameFont = Content.Load<SpriteFont>("GameFont");
          
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // DEBUG "STUFF"
            MainManager.Instance.inputMan.Update();

            cam.Scale += MainManager.Instance.inputMan.CurGamePadState.Triggers.Right / 10;
            cam.Scale -= MainManager.Instance.inputMan.CurGamePadState.Triggers.Left / 10;

            cam.Update();

            if (MainManager.Instance.inputMan.CurGamePadState.IsButtonDown(Buttons.Y) && MainManager.Instance.inputMan.PrevGamePadState.IsButtonUp(Buttons.Y))
                if(MainManager.Instance.gameMan.player.gloves.Count > 0)
                    MainManager.Instance.discardMan.DiscardArmor(MainManager.Instance.gameMan.player.gloves.Pop());

            if (MainManager.Instance.inputMan.CurGamePadState.IsButtonDown(Buttons.X) && MainManager.Instance.inputMan.PrevGamePadState.IsButtonUp(Buttons.X))
                if (MainManager.Instance.gameMan.player.chestplates.Count > 0)
                    MainManager.Instance.discardMan.DiscardArmor(MainManager.Instance.gameMan.player.chestplates.Pop());

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            // SUPER HACKY LEVEL EDITOR STUFF
            if (MainManager.Instance.gameMan.gState == GameState.map_editor)
            {
                if (MainManager.Instance.inputMan.CurMouseState.LeftButton == ButtonState.Pressed && MainManager.Instance.inputMan.PrevMouseState.LeftButton != ButtonState.Pressed)
                {
                    if (upperRightNow)
                    {
                        upperRight = (MainManager.Instance.inputMan.CurMouseState.Position.ToVector2() / cam.Scale) + cam.Position - cam.Origin;
                        upperRightNow = false;
                    }
                    else
                    {
                        Vector2 lowerLeft = (MainManager.Instance.inputMan.CurMouseState.Position.ToVector2() / cam.Scale) + cam.Position - cam.Origin;
                        MainManager.Instance.gameMan.platforms.Add(new Platform((int)upperRight.X, (int)upperRight.Y, (int)(lowerLeft.X - upperRight.X), (int)(lowerLeft.Y - upperRight.Y)));
                        System.Console.WriteLine("X: {0} Y: {1}", MainManager.Instance.inputMan.CurMouseState.X + cam.Position.X, MainManager.Instance.inputMan.CurMouseState.Y + cam.Position.Y);
                        upperRightNow = true;
                    }
                }
                else if (MainManager.Instance.inputMan.CurMouseState.RightButton == ButtonState.Pressed && MainManager.Instance.inputMan.PrevMouseState.RightButton != ButtonState.Pressed)
                {

                    Vector2 pos = (MainManager.Instance.inputMan.CurMouseState.Position.ToVector2() / cam.Scale) + cam.Position - cam.Origin;
                    foreach(Platform p in MainManager.Instance.gameMan.platforms)
                    {
                        if (p.Collide(pos))
                        { 
                           MainManager.Instance.gameMan.platforms.Remove(p);
                           break;
                        }
                    }
                }
                else if(MainManager.Instance.inputMan.CurKeyboardState.IsKeyDown(Keys.G) && MainManager.Instance.inputMan.PrevKeyboardState.IsKeyUp(Keys.G))
                {
                    Vector2 pos = (MainManager.Instance.inputMan.CurMouseState.Position.ToVector2() / cam.Scale) + cam.Position - cam.Origin;
                    MainManager.Instance.gameMan.armorPickups.Add(new Gloves((int)pos.X, (int)pos.Y, MainManager.Instance.gameMan.player.rand));
                }
                else if (MainManager.Instance.inputMan.CurKeyboardState.IsKeyDown(Keys.C) && MainManager.Instance.inputMan.PrevKeyboardState.IsKeyUp(Keys.C))
                {
                    Vector2 pos = (MainManager.Instance.inputMan.CurMouseState.Position.ToVector2() / cam.Scale) + cam.Position - cam.Origin;
                    MainManager.Instance.gameMan.armorPickups.Add(new ChestPlate((int)pos.X, (int)pos.Y, MainManager.Instance.gameMan.player.rand));
                }
            }

            this.gameTime = gameTime;
            MainManager.Instance.gameMan.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkKhaki);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.Transform);

            // Temporary stuff for drawing another shape
            //Rectangle rect = new Rectangle(10, 10, 40, 40);

            MainManager.Instance.drawMan.Draw(MainManager.Instance.main.spriteBatch);
            MainManager.Instance.discardMan.Draw(MainManager.Instance.main.spriteBatch);
            spriteBatch.End();

            // Draw UI last because it uses a different spriteBatch
            MainManager.Instance.uiMan.Draw();
            spriteBatch.Begin();
            spriteBatch.Draw(MainManager.Instance.drawMan.rectTexture, new Rectangle(10, 10, 35, 30), Color.White);
               spriteBatch.DrawString(MainManager.Instance.drawMan.gameFont, MainManager.Instance.gameMan.player.armorLevel.ToString(), new Vector2(10, 10), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
