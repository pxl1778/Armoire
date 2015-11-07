using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Armoire
{
    class InputManager
    {
        // Consts
        private const float move_threshold = 0.20f;

        private GamePadState prvGamePadState;
        private GamePadState curGamePadState;
        private KeyboardState prvKeyboardState;
        private KeyboardState curKeyboardState;
        private MouseState prvMouseState;
        private MouseState curMouseState;

        public void Update()
        {
            this.prvGamePadState = this.curGamePadState;
            this.prvKeyboardState = this.curKeyboardState;
            this.prvMouseState = this.curMouseState;
            this.curGamePadState = GamePad.GetState(PlayerIndex.One);
            this.curKeyboardState = Keyboard.GetState();
            this.curMouseState = Mouse.GetState();
        }

        public GamePadState CurGamePadState
        {
            get
            {
                return this.curGamePadState;
            }
        }

        public GamePadState PrevGamePadState
        {
            get
            {
                return this.prvGamePadState;
            }
        }

        public KeyboardState CurKeyboardState
        {
            get
            {
                return this.curKeyboardState;
            }
        }

        public KeyboardState PrevKeyboardState
        {
            get
            {
                return this.prvKeyboardState;
            }
        }

        public MouseState PrevMouseState
        {
            get
            {
                return this.prvMouseState;
            }
        }

        public MouseState CurMouseState
        {
            get
            {
                return this.curMouseState;
            }
        }

        public Boolean Jump
        {
            get
            {
                return this.CurGamePadState.IsButtonDown(Buttons.A) || this.CurKeyboardState.IsKeyDown(Keys.Space);
            }
        }

        public Boolean PrevJump
        {
            get
            {
                return this.PrevGamePadState.IsButtonDown(Buttons.A) || this.PrevKeyboardState.IsKeyDown(Keys.Space);
            }
        }

        public Boolean Charge
        {
            get
            {
                return this.CurGamePadState.IsButtonDown(Buttons.B) || this.CurKeyboardState.IsKeyDown(Keys.D);
            }
        }

        public Boolean PrevCharge
        {
            get
            {
                return this.PrevGamePadState.IsButtonDown(Buttons.B) || this.PrevKeyboardState.IsKeyDown(Keys.D);
            }
        }

        public Boolean MoveLeft
        {
            get
            {
                return this.CurGamePadState.ThumbSticks.Left.X < -move_threshold || this.CurGamePadState.IsButtonDown(Buttons.DPadLeft) || this.CurKeyboardState.IsKeyDown(Keys.Left);
            }
        }

        public Boolean MoveRight
        {
            get
            {
                return this.CurGamePadState.ThumbSticks.Left.X > move_threshold || this.CurGamePadState.IsButtonDown(Buttons.DPadRight) || this.CurKeyboardState.IsKeyDown(Keys.Right);
            }
        }

        public Boolean MoveUp
        {
            get
            {
                return this.CurGamePadState.ThumbSticks.Left.Y > move_threshold || this.CurGamePadState.IsButtonDown(Buttons.DPadUp) || this.CurKeyboardState.IsKeyDown(Keys.Up);
            }
        }

        public Boolean MoveDown
        {
            get
            {
                return this.CurGamePadState.ThumbSticks.Left.Y < -move_threshold || this.CurGamePadState.IsButtonDown(Buttons.DPadDown) || this.CurKeyboardState.IsKeyDown(Keys.Down);
            }
        }

        public Boolean PrevMoveLeft
        {
            get
            {
                return this.PrevGamePadState.ThumbSticks.Left.X < -move_threshold || this.PrevGamePadState.IsButtonDown(Buttons.DPadLeft) || this.PrevKeyboardState.IsKeyDown(Keys.Left);
            }
        }

        public Boolean PrevMoveRight
        {
            get
            {
                return this.PrevGamePadState.ThumbSticks.Left.X > move_threshold || this.PrevGamePadState.IsButtonDown(Buttons.DPadRight) || this.PrevKeyboardState.IsKeyDown(Keys.Right);
            }
        }

        public Boolean PrevMoveUp
        {
            get
            {
                return this.PrevGamePadState.ThumbSticks.Left.Y > move_threshold || this.PrevGamePadState.IsButtonDown(Buttons.DPadUp) || this.PrevKeyboardState.IsKeyDown(Keys.Up);
            }
        }

        public Boolean PrevMoveDown
        {
            get
            {
                return this.PrevGamePadState.ThumbSticks.Left.Y < -move_threshold || this.PrevGamePadState.IsButtonDown(Buttons.DPadDown) || this.PrevKeyboardState.IsKeyDown(Keys.Down);
            }
        }

        public Boolean Pause
        {
            get
            {
                return (this.CurGamePadState.IsButtonDown(Buttons.Start) && this.PrevGamePadState.IsButtonUp(Buttons.Start))  || (this.CurKeyboardState.IsKeyDown(Keys.Escape) && this.PrevKeyboardState.IsKeyUp(Keys.Escape));
            }
        }
    }
}
