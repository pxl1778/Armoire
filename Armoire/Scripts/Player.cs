﻿using System;
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
        public Stack<ChestPlate> chestplates;
        public Stack<Gloves> gloves;

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
            chestplates = new Stack<ChestPlate>();
            Initialize();
        }

        public void Initialize()
        {
            helmets.Push(new Helmet(0, 0));
        }

        public void Update()
        {
            //Check for changing states
            CalculateSteeringForces();
            StateUpdate();
            //physics...
            velocity += acceleration;
            velocity = Vector2.Clamp(velocity, maxSpeed * -1, maxSpeed);
            //CalculateSteeringForces();
            CheckCollision();
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
            //CheckCollision();
            steeringForce = new Vector2(0, 0);
        }

        /// <summary>
        /// Checks how the player interacts with the world around it
        /// </summary>
        public void CheckCollision()
        {
            foreach (Platform p in MainManager.Instance.gameMan.platforms)
            {

                if ((p.Collide(new Vector2(rect.X, rect.Y + rect.Height + velocity.Y)) || p.Collide(new Vector2(rect.X + rect.Width, rect.Y + rect.Height + velocity.Y))))// && pState != PlayerState.jumping)
                {
                    acceleration.Y = 0;
                    velocity.Y = 0;
                    //pos.Y = p.rect.Y-rect.Height;
                    canJump = true;
                }
                if ((p.Collide(new Vector2(rect.X + velocity.X, rect.Y + rect.Height)) || p.Collide(new Vector2(rect.X + rect.Width + velocity.X, rect.Y + rect.Height))))// && pState != PlayerState.jumping)
                {
                    acceleration.X = 0;
                    velocity.X = 0;
                    //pos.Y = p.rect.Y - rect.Height;
                    canJump = false;
                }
                if ((p.Collide(new Vector2(rect.X + velocity.X, rect.Y + rect.Height + velocity.Y)) || p.Collide(new Vector2(rect.X + rect.Width + velocity.X, rect.Y + rect.Height + velocity.Y))))// && pState != PlayerState.jumping)
                {
                    acceleration.X = 0;
                    velocity.X = 0;
                    //pos.Y = p.rect.Y - rect.Height;
                    canJump = true;
                }
                
                /*
                if(p.rect.Intersects(new Rectangle((int)Math.Ceiling(rect.X + velocity.X), (int)Math.Ceiling(rect.Y + velocity.Y), rect.Width, rect.Height)) && pState != PlayerState.jumping) //(p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y)) || p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y + rect.Height)))
                {
                    acceleration.X = 0;
                    acceleration.Y = 0;
                    velocity.X = 0;
                    velocity.Y = 0;
                    //pState = PlayerState.idle;
                    //pos.X = p.rect.X - rect.Width;
                    canJump = true;
                }
                if (p.rect.Intersects(new Rectangle(rect.X, (int)(rect.Y + velocity.Y), rect.Width, rect.Height)) && pState != PlayerState.jumping) //(p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y)) || p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y + rect.Height)))
                {
                    //acceleration.X = 0;
                    acceleration.Y = 0;
                    //velocity.X = 0;
                    velocity.Y = 0;
                    //pState = PlayerState.idle;
                    //pos.X = p.rect.X - rect.Width;
                    canJump = true;
                }
                if (p.rect.Intersects(new Rectangle((int)(rect.X + velocity.X), rect.Y, rect.Width, rect.Height)) && pState != PlayerState.jumping) //(p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y)) || p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y + rect.Height)))
                {
                    acceleration.X = 0;
                    //acceleration.Y = 0;
                    velocity.X = 0;
                    //velocity.Y = 0;
                    //pState = PlayerState.idle;
                    //pos.X = p.rect.X - rect.Width;
                    canJump = true;
                }
                if (p.rect.Intersects(new Rectangle((int)(rect.X + velocity.X), (int)(rect.Y + velocity.Y), rect.Width, rect.Height)) && pState != PlayerState.jumping) //(p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y)) || p.Collide(new Vector2(rect.X + rect.Width + 1, rect.Y + rect.Height)))
                {
                    acceleration.X = 0;
                    acceleration.Y = 0;
                    velocity.X = 0;
                    velocity.Y = 0;
                    //pState = PlayerState.idle;
                    //pos.X = p.rect.X - rect.Width;
                    canJump = true;
                }*/
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (pState == PlayerState.walking && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                26,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                helmets.Peek().Draw(sb, frame, dState);
            }
            if(pState == PlayerState.walking && dState == DirectionState.left && velocity.Y <=0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
                helmets.Peek().Draw(sb, frame, dState);
            }
            if(pState == PlayerState.idle && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                helmets.Peek().Draw(sb, dState);
            }
            if (pState == PlayerState.idle &&  dState == DirectionState.left && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
                helmets.Peek().Draw(sb, dState);
            }
            if(velocity.Y>0 && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                helmets.Peek().Draw(sb, dState);
            }
            if (velocity.Y > 0 && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
                helmets.Peek().Draw(sb, dState);
            }
            if (pState == PlayerState.jumping && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                helmets.Peek().Draw(sb, dState);
            }
            if (pState == PlayerState.jumping && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                24,
                                                45), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);
                helmets.Peek().Draw(sb, dState);
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
