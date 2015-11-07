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
        Camera cam;
        public GameTime gameTime;

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

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MainManager.Instance.drawMan.playerSpritesheet = Content.Load<Texture2D>("Assets/spritesheet.png");
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
   
            if (MainManager.Instance.inputMan.MoveLeft)
                MainManager.Instance.gameMan.player.velocity.X = -5;
            else if (MainManager.Instance.inputMan.MoveRight)
                MainManager.Instance.gameMan.player.velocity.X = 5;
            else
                MainManager.Instance.gameMan.player.velocity.X = 0;

            cam.Scale += MainManager.Instance.inputMan.CurGamePadState.Triggers.Right / 10;
            cam.Scale -= MainManager.Instance.inputMan.CurGamePadState.Triggers.Left / 10;


            cam.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            spriteBatch.End();

            // Draw UI last because it uses a different spriteBatch
            MainManager.Instance.uiMan.Draw();

            base.Draw(gameTime);
        }
    }
}
