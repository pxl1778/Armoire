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
        public DiscardManager discardMan;
        public Game1 main;

        public MainManager(Game1 main)
        {
            
            this.main = main;
        }

        public static void init(Game1 main)
        {
            instance = new MainManager(main);
            instance.uiMan = new UIManager();
            instance.inputMan = new InputManager();
            instance.drawMan = new DrawManager();
            instance.gameMan = new GameManager();
            instance.discardMan = new DiscardManager();
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
