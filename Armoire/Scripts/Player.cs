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
        jumping, charging, dashing, walking, idle, falling
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
        public Vector2 maxSpeed;
        public float maxForce;
        public float decceleration;
        public bool canJump;
        public Stack<Helmet> helmets;
    

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
            width = 23;
            height = 45;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            frame = 0;
            maxForce = 20f;
            maxSpeed = new Vector2(3f, 5f);
            decceleration = .9f;
            canJump = true;
            helmets = new Stack<Helmet>();
        }

        public void Update()
        {
            //Check for changing states
            CalculateSteeringForces();
            StateUpdate();
            //physics...
            velocity += acceleration;
            velocity = Vector2.Clamp(velocity, maxSpeed * -1, maxSpeed);
            pos += velocity;
            rect.Location = new Point((int)pos.X, (int)pos.Y);
            acceleration = new Vector2(0, 0);
            Animation();
        }

        public void StateUpdate()
        {
            if (velocity.Y < 0)
            {
                pState = PlayerState.falling;
            }
            if(MainManager.Instance.inputMan.MoveLeft)
            {
                dState = DirectionState.left;
                if(pState != PlayerState.jumping)
                {
                    pState = PlayerState.walking;
                }
            }
            else if(MainManager.Instance.inputMan.MoveRight)
            {
                dState = DirectionState.right;
                if(pState != PlayerState.jumping)
                {
                    pState = PlayerState.walking;
                }
            }
            else if(pState != PlayerState.jumping)
            {
                pState = PlayerState.idle;
            }
        }

        /// <summary>
        /// Calculates the steering force to be added to the acceleration
        /// </summary>
        public void CalculateSteeringForces()
        {
            if (pState != PlayerState.jumping && MainManager.Instance.inputMan.Jump && canJump)
            {
                steeringForce.Y -= 10;
                pState = PlayerState.jumping;
                canJump = false;
            }
            else
            {
                steeringForce.Y += .1f;
            }
            if(MainManager.Instance.inputMan.MoveLeft)
            {
                steeringForce.X -= 1;
            }
            else if(MainManager.Instance.inputMan.MoveRight)
            {
                steeringForce.X += 1;
            }
            else
            {
                velocity.X *= decceleration;
            }
            steeringForce = Vector2.Clamp(steeringForce, new Vector2(-maxForce, -maxForce), new Vector2(maxForce, maxForce));
            acceleration += steeringForce;
            CheckCollision();
            steeringForce = new Vector2(0, 0);
        }

        /// <summary>
        /// Checks how the player interacts with the world around it
        /// </summary>
        public void CheckCollision()
        {
            foreach (Platform p in MainManager.Instance.gameMan.platforms)
            {
                if ((p.Collide(new Vector2(rect.X, rect.Y + rect.Height + 1)) || p.Collide(new Vector2(rect.X + rect.Width, rect.Y + rect.Height + 1))) && pState != PlayerState.jumping)
                {
                    acceleration.Y = 0;
                    velocity.Y = 0;
                    pos.Y = p.rect.Y-rect.Height;
                    pState = PlayerState.idle;
                    canJump = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.rectTexture, new Rectangle(10, 10, 40, 40), Color.Blue);
            sb.Draw(MainManager.Instance.drawMan.rectTexture, rect, Color.Blue);

            if (pState == PlayerState.walking && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                26,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                
            }
            if(pState == PlayerState.walking && dState == DirectionState.left && velocity.Y <=0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }
            if(pState == PlayerState.idle && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            }
            if (pState == PlayerState.idle &&  dState == DirectionState.left && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }
            if(velocity.Y>0 && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            }
            if (velocity.Y > 0 && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }
            if (pState == PlayerState.jumping && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            }
            if (pState == PlayerState.jumping && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }
            
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
