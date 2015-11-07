using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Armoire
{
    class MainManager
    {
        // Singleton instance
        private static MainManager instance;

        //Fields
        public UIManager uiMan;
        public InputManager inputMan;
        public DrawManager drawMan;
        public GameManager gameMan;
        public Game1 main;

        public MainManager(Game1 main)
        {
            uiMan = new UIManager();
            inputMan = new InputManager();
            drawMan = new DrawManager();
            gameMan = new GameManager();
            this.main = main;
        }

        public static void init(Game1 main)
        {
            instance = new MainManager(main);
        }

        public static MainManager Instance
        {
            get
            {
                return instance;
            }

        }
    }
}
