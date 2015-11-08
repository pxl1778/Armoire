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
        public bool canDash;
        public Stack<Helmet> helmets;
        public Stack<ChestPlate> chestplates;
        public Stack<Gloves> gloves;
        public Random rand;
        public float armorScale;
        public int armorLevel;
        public bool invincible;
        public double invincibleCounter;

        public PlayerState pState;
        public DirectionState dState;

        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        double chargeCounter;
        
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
            gloves = new Stack<Gloves>();
            rand = new Random();
            chargeCounter = 0.0;
            armorScale = 0;
            armorLevel = 3;
            invincible = false;
            Initialize();
        }

        public void Initialize()
        {
            helmets.Push(new Helmet(0, 0, rand));
            chestplates.Push(new ChestPlate(250, 250, rand));
            gloves.Push(new Gloves(250, 250, rand));
            armorScale += 1.0f;
        }

        public void Update()
        {
            //Check for changing states and Calculating Forces
            CalculateSteeringForces();
            StateUpdate();
            if(pState == PlayerState.charging)
            {
                chargeCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
                if(chargeCounter > .4)
                {
                    canDash = true;
                }
                else
                {
                    canDash = false;
                }
            }
            if(invincibleCounter < 1)
            {
                invincibleCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                invincible = false;
            }
            //physics
            velocity += acceleration;
            if(pState != PlayerState.dashing)
            {
                velocity = Vector2.Clamp(velocity, maxSpeed * -1, maxSpeed);
            }
            else
            {
                velocity = Vector2.Clamp(velocity, maxSpeed * -3, maxSpeed * 3);
            }
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
            else if(pState != PlayerState.jumping && pState != PlayerState.dashing)
            {
                pState = PlayerState.idle;
            }

            if(MainManager.Instance.inputMan.Charge)
            {
                pState = PlayerState.charging;
            }
            
        }

        /// <summary>
        /// Calculates the steering force to be added to the acceleration
        /// </summary>
        public void CalculateSteeringForces()
        {
            if (pState != PlayerState.jumping && MainManager.Instance.inputMan.Jump && !MainManager.Instance.inputMan.PrevJump && canJump)
            {
                steeringForce.Y -= 10;
                pState = PlayerState.jumping;
                canJump = false;
            }
            else
            {
                steeringForce.Y += .1f;
            }
            if(MainManager.Instance.inputMan.MoveLeft && pState != PlayerState.charging)
            {
                steeringForce.X -= 1;
            }
            else if(MainManager.Instance.inputMan.MoveRight && pState != PlayerState.charging)
            {
                steeringForce.X += 1;
            }
            if(!MainManager.Instance.inputMan.Charge && canDash && dState == DirectionState.right)
            {
                acceleration.X += 100;
                canDash = false;
                chargeCounter = 0;
                pState = PlayerState.dashing;
            }
            if (!MainManager.Instance.inputMan.Charge && canDash && dState == DirectionState.left)
            {
                acceleration.X -= 100;
                canDash = false;
                chargeCounter = 0;
                pState = PlayerState.dashing;
            }
            else
            {
                velocity.X *= decceleration;
            }
            steeringForce = Vector2.Clamp(steeringForce, new Vector2(-maxForce, -maxForce), new Vector2(maxForce, maxForce));
            acceleration += steeringForce;
            steeringForce = new Vector2(0, 0);
        }

        /// <summary>
        /// Checks how the player interacts with the world around it
        /// </summary>
        public void CheckCollision()
        {
            List<Armor> armorToRemove = new List<Armor>();
            foreach (Armor a in MainManager.Instance.gameMan.armorPickups)
            {
                if (rect.Intersects(new Rectangle((int)a.position.X, (int)a.position.Y, 1, 1)))
                {
                    if (a is Gloves)
                    {
                        gloves.Push((Gloves)a);
                        armorToRemove.Add(a);
                    }
                    else if (a is ChestPlate)
                    {
                        chestplates.Push((ChestPlate)a);
                        armorToRemove.Add(a);
                    }
                    else if (a is Helmet)
                    {
                        helmets.Push((Helmet)a);
                        armorToRemove.Add(a);
                    }
                    armorScale += .5f;
                    armorLevel++;
                    width = (int)(23 * armorScale);
                    height = (int)(45 * armorScale);
                    pos.X -= 12;
                    pos.Y -= 23;
                    rect = new Rectangle(rect.X - 12, rect.Y - 23, width, height);
                    MainManager.Instance.main.cam.Scale = 1.0f / armorScale;
                }
            }

            foreach(Armor a in armorToRemove)
                MainManager.Instance.gameMan.armorPickups.Remove(a);
            
            foreach (Enemy e in MainManager.Instance.gameMan.enemies)
            {
                if(e.rect.Intersects(rect) && invincible == false)
                {
                    invincible = true;
                    if(pState == PlayerState.dashing)
                    {
                        e.Hit();
                    }
                    else
                    {
                        Hit(e.dir);
                    }
                }
            }
            MainManager.Instance.gameMan.enemies.Remove(MainManager.Instance.gameMan.toRemove);

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

        public void Hit(int direction)
        {
            armorLevel--;
            velocity.X += 5 * -direction;
            if(gloves.Count != 0)
            {
                MainManager.Instance.discardMan.DiscardArmor(gloves.Pop());
            }
            else if(helmets.Count != 0)
            {
                MainManager.Instance.discardMan.DiscardArmor(helmets.Pop());
            }
            else if(chestplates.Count != 0)
            {
                MainManager.Instance.discardMan.DiscardArmor(chestplates.Pop());
            }
            else
            {
                MainManager.Instance.uiMan.PushScreen(new GameOverScreen());
            }
            invincibleCounter = 0;
        }

        public void Draw(SpriteBatch sb)
        {
            if (pState == PlayerState.walking && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if(helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, frame, dState);
                }
                if(chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, frame, dState);
                }
                if(gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, frame, dState);
                }
            }
            if(pState == PlayerState.walking && dState == DirectionState.left && velocity.Y <=0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                frame * 25,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, frame, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, frame, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, frame, dState);
                }
            }
            if(pState == PlayerState.idle && dState == DirectionState.right && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                22,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.idle &&  dState == DirectionState.left && velocity.Y <= 0)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                0,
                                                0,
                                                22,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if(velocity.Y>0 && dState == DirectionState.right && pState != PlayerState.charging)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                127,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (velocity.Y > 0 && dState == DirectionState.left && pState != PlayerState.charging)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                127,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.jumping && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.jumping && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                50,
                                                0,
                                                23,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if(pState == PlayerState.charging && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                154,
                                                0,
                                                28,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.charging && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                154,
                                                0,
                                                28,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.dashing && dState == DirectionState.right)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                190,
                                                0,
                                                35,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.None, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
            if (pState == PlayerState.dashing && dState == DirectionState.left)
            {
                sb.Draw(MainManager.Instance.drawMan.spritesheet, new Vector2(rect.X, rect.Y), new Rectangle(
                                                190,
                                                0,
                                                35,
                                                45), Color.White, 0, Vector2.Zero, (float)armorScale, SpriteEffects.FlipHorizontally, 0);
                if (helmets.Count != 0)
                {
                    helmets.Peek().Draw(sb, dState);
                }
                if (chestplates.Count != 0)
                {
                    chestplates.Peek().Draw(sb, dState);
                }
                if (gloves.Count != 0)
                {
                    gloves.Peek().Draw(sb, dState);
                }
            }
        }

        public void Animation()
        {
            timeCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;
                if (frame > 4)
                    frame = 0;
                timeCounter -= timePerFrame;
            }
        }
    }
}
