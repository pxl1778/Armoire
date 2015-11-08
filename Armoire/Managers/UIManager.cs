using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Armoire
{
    class UIManager
    {
        Stack<IScreen> screenStack;

        public UIManager()
        {
            this.screenStack = new Stack<IScreen>();
        }

        public void PushScreen(IScreen screen)
        {
            screenStack.Push(screen);
        }

        public void PopScreen()
        {
            screenStack.Pop();
        }

        public IScreen Top()
        {
            if (screenStack.Count == 0)
                return null;
            return screenStack.Peek();
        }

        public void Draw()
        {
            if (screenStack.Count > 0)
                screenStack.Peek().Draw();
        }

        public void Update()
        {
            if (screenStack.Count > 0)
                screenStack.Peek().Update();
        }
    }
}
