using Microsoft.Xna.Framework;

namespace Armoire
{
    public class Camera
    {
        #region Fields
        private Game1 mainMan;
        private Vector2 position;
        private float viewportWidth;
        private float viewportHeight;
        private float moveSpeed;
        private float rotation;
        private float scale;
        private Vector2 origin;
        private Rectangle focus;
        private Vector2 screenCenter;
        private Matrix transform;
        #endregion

        #region Properties
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 ScreenCenter { get { return screenCenter; } set { screenCenter = value; } }
        public float ViewportWidth { get { return viewportWidth; } set { viewportWidth = value; } }
        public float ViewportHeight { get { return viewportHeight; } set { viewportHeight = value; } }
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public float Scale { get { return scale; } set { scale = value; } }
        public Vector2 Origin { get { return origin; } set { origin = value; } }
        public Rectangle Focus { get { return focus; } set { focus = value; } }
        public Matrix Transform { get { return transform; } set { transform = value; } }
        #endregion
        public Camera(Game1 mainMan, Rectangle focus)
        {
            this.mainMan = mainMan;
            this.focus = focus;
            Initialize();
        }

        public void Initialize()
        {
            viewportHeight = mainMan.GraphicsDevice.Viewport.Height;
            viewportWidth = mainMan.GraphicsDevice.Viewport.Width;
            screenCenter = new Vector2(viewportWidth / 2, viewportHeight / 2);
            moveSpeed = .5f;
            transform = Matrix.Identity;
            position = new Vector2(0, 0);
            scale = 1f;
        }

        public void Update()
        {
            origin.X = (viewportWidth / 2) / scale;
            origin.Y = (viewportHeight / 2) / scale;
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                        Matrix.CreateTranslation(new Vector3(origin.X, origin.Y, 0)) *
                        Matrix.CreateScale(new Vector3(scale, scale, 0));
            position.X =  MainManager.Instance.gameMan.player.pos.X;
            position.Y =  MainManager.Instance.gameMan.player.pos.Y;
        }
    }
}