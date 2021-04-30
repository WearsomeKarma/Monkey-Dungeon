using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.Menus
{
    public class MainMenuScene : Scene
    {
        public MainMenuScene(Game game) 
            : base(game)
        {
            AddLayers(
                new GreetingMenu_Layer(this),
                new NewGame_Layer(this),
                new FindGame_Layer(this)
                );
            SetLayer<GreetingMenu_Layer>();
        }

        internal void SetLayer<T>() where T : SceneLayer => EnableOnlyLayer<T>();
    }
}
