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
        public Stack<Armor> armor;
        public Vector2 pos;
        public double counter;
        EnemyState eState;
        public int dir;
        public int speed;
        public float playerDistance;
        public int attackRange;
        public Rectangle rect;

        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        public Enemy(int x, int y)
        {
            counter = 0;
            pos = new Vector2(x, y);
            dir = 1;
            eState = EnemyState.idle;
            speed = 1;
            fps = 7.0;
            timePerFrame = 1.0 / fps;
            attackRange = 130;
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

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.spritesheet, pos, new Rectangle(
                                                                        frame * 34,
                                                                        125,
                                                                        34,
                                                                        25), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        public void Hit()
        {

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
                }
                else
                {
                    pos.X += speed;
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
