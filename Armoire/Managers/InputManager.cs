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

        public GamePadState PrevGamepadState
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

        public Boolean Charge
        {
            get
            {
                return this.CurGamePadState.IsButtonDown(Buttons.B) || this.CurKeyboardState.IsKeyDown(Keys.D);
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

        public Boolean Pause
        {
            get
            {
                return (this.CurGamePadState.IsButtonDown(Buttons.Start) && this.PrevGamepadState.IsButtonUp(Buttons.Start));
            }
        }
    }
}
