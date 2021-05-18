using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.Menus
{
    public class MainMenuScene : Scene
    {
        internal MainMenuScene(MonkeyDungeon_Game_Client monkeyGame) 
            : base(monkeyGame)
        {
            AddLayers(
                new GreetingMenu_Layer(monkeyGame, this),
                new NewGame_Layer(monkeyGame, this),
                new FindGame_Layer(monkeyGame, this)
                );
            SetLayer<GreetingMenu_Layer>();
        }

        internal void SetLayer<T>() where T : SceneLayer => EnableOnlyLayer<T>();
    }
}
