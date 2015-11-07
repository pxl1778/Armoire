using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Armoire
{
    enum PlayerState
    {
        jumping, charging, dashing, walking, idle
    }

    enum DirectionState
    {
        left, right
    }

    class Player
    {
        //Fields
        private int width;
        private int height;
        private Rectangle rect;
        public Vector2 pos;
        public Vector2 velocity;
        public Vector2 acceleration;
        public Vector2 steeringForce;
        public Vector2 forward;
        public float maxForce;

        public PlayerState pState;
        public DirectionState dState;

        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        
        //Properties
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Rectangle Rect { get { return rect; } set { rect = value; } }

        public Player()
        {
            pos = new Vector2(MainManager.Instance.main.GraphicsDevice.Viewport.Width / 2, MainManager.Instance.main.GraphicsDevice.Viewport.Height / 2);
            velocity = new Vector2(0, 0);
            acceleration = new Vector2(0, 0);
            forward = new Vector2(1, 0);
            pState = PlayerState.idle;
            dState = DirectionState.right;
            width = 50;
            height = 70;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            frame = 0;
            maxForce = 20f;
        }

        public void Update()
        {
            pos += velocity;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            StateUpdate();
            CalculateSteeringForces();
            //physics...
            //Animation();
            
        }

        public void StateUpdate()
        {
            if(MainManager.Instance.inputMan.Jump)
            {
                pState = PlayerState.jumping;
            }
            if(MainManager.Instance.inputMan.MoveLeft)
            {
                dState = DirectionState.left;
                if(pState != PlayerState.jumping)
                {
                    pState = PlayerState.walking;
                }
            }
            if(MainManager.Instance.inputMan.MoveRight)
            {
                dState = DirectionState.right;
                if(pState != PlayerState.jumping)
                {
                    pState = PlayerState.walking;
                }
            }
        }

        /// <summary>
        /// Calculates the steering force to be added to the acceleration
        /// </summary>
        public void CalculateSteeringForces()
        {
            if (pState != PlayerState.jumping && MainManager.Instance.inputMan.Jump)
            {
                steeringForce.Y += 10;
            }
            if(MainManager.Instance.inputMan.MoveLeft)
            {
                steeringForce.X -= 3;
            }
            if(MainManager.Instance.inputMan.MoveRight)
            {
                steeringForce.X += 3;
            }
            acceleration += steeringForce;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.rectTexture, new Rectangle(10, 10, 40, 40), Color.Blue);
            sb.Draw(MainManager.Instance.drawMan.rectTexture, rect, Color.Blue);
            /*
            if(pState == PlayerState.walking && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.playerSpritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                26,
                                                48), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            }*/
            
        }

        public void Animation()
        {
            timeCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;
                if (frame > 2)
                    frame = 0;
                timeCounter -= timePerFrame;
            }
        }
    }
}
