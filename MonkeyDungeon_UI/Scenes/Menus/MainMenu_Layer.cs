using isometricgame.GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.Menus
{
    public class MainMenu_Layer : SceneLayer
    {
        protected MainMenuScene MainMenuScene { get; private set; }
        protected MonkeyDungeon_Game_Client MonkeyGame { get; private set; }

        internal MainMenu_Layer(MonkeyDungeon_Game_Client monkeyGame, MainMenuScene parentScene) 
            : base(parentScene)
        {
            MainMenuScene = parentScene;
            MonkeyGame = monkeyGame;
        }
    }
}
