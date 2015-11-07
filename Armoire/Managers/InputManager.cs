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
    }
}
