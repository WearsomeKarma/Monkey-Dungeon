using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.Menus
{
    public class GreetingMenu_Layer : MainMenu_Layer
    {
        public GreetingMenu_Layer(MainMenuScene parentScene) 
            : base(parentScene)
        {
            Add_StaticObject(
                new GameObject(this, new Vector3(-Game.Width/2,-Game.Height/2,0), "mainMenu")
                );

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -100, 0),
                    new Vector2(200, 100),
                    () => MainMenuScene.SetLayer<NewGame_Layer>(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Create Game"
                    ));
            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -250,0),
                    new Vector2(200,100),
                    () => MainMenuScene.SetLayer<FindGame_Layer>(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Multiplayer"
                    ));
            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -400, 0),
                    new Vector2(200, 100),
                    () => Game.Close(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Quit"
                    ));
        }
    }
}
