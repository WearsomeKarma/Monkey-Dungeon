using isometricgame.GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.Menus
{
    public class MainMenu_Layer : SceneLayer
    {
        protected MainMenuScene MainMenuScene { get; private set; }

        public MainMenu_Layer(MainMenuScene parentScene) 
            : base(parentScene)
        {
            MainMenuScene = parentScene;
        }
    }
}
