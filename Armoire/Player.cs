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
        public PlayerState pState;
        public DirectionState dState;
        
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
        }

        public void Update()
        {
            pos += velocity;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
            CalculateSteeringForces();
        }

        /// <summary>
        /// Calculates the steering force to be added to the acceleration
        /// </summary>
        public void CalculateSteeringForces()
        {
            if (pState != PlayerState.jumping)
            {
                steeringForce.Y += 10;
            }

        }


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MainManager.Instance.drawMan.rectTexture, new Rectangle(10, 10, 40, 40), Color.Blue);
            sb.Draw(MainManager.Instance.drawMan.rectTexture, rect, Color.Blue);
        }
    }
}
