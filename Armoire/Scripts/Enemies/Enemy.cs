using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Armoire
{
    enum EnemyState
    {
        idle, attacking
    }

    class Enemy
    {
        public Stack<Helmet> armor;
        public Vector2 pos;
        public double counter;
        EnemyState eState;
        public int dir;
        public float speed;
        public float playerDistance;
        public int attackRange;
        public float scale;
        public Rectangle rect;
        public bool invincible;
        public double invincibleCounter;

        public int frame;
        public double timeCounter;
        public double fps;
        public double timePerFrame;

        public Enemy(int x, int y)
        {
            counter = 0;
            armor = new Stack<Helmet>();
            pos = new Vector2(x, y);
            dir = 1;
            eState = EnemyState.idle;
            speed = .5f;
            fps = 7.0;
            timePerFrame = 1.0 / fps;
            attackRange = 130;
            invincible = false;
            invincibleCounter = 0;
            scale = 1;
            rect = new Rectangle((int)pos.X, (int)pos.Y, 34, 25);
        }

        public void Update()
        {
            counter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter > 2)
            {
                dir *= -1;
                counter = 0;
            }
            if (invincibleCounter < 1)
            {
                invincibleCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                invincible = false;
            }
            playerDistance = MainManager.Instance.gameMan.player.pos.X - pos.X;
            if(Math.Abs(playerDistance) < attackRange)
            {
                eState = EnemyState.attacking;
            }
            else
            {
                eState = EnemyState.idle;
            }
            Movement();
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            Animation();
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        125,
                                                                        34,
                                                                        25), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public void Hit()
        {
            if(armor.Count != 0)
            {
                MainManager.Instance.discardMan.DiscardArmor(armor.Pop());
            }
            else
            {
                MainManager.Instance.gameMan.toRemove = this;
            }
        }

        public void Movement()
        {
            if(eState == EnemyState.idle)
            {
                pos.X += speed * dir;
            }
            else
            {
                if(playerDistance <=0 )
                {
                    pos.X -= speed;
                    dir = 1;
                }
                else
                {
                    pos.X += speed;
                    dir = -1;
                }
            }
        }

        public void Animation()
        {
            timeCounter += MainManager.Instance.main.gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;
                if (frame > 3)
                    frame = 0;
                timeCounter -= timePerFrame;
            }
        }
    }
}
