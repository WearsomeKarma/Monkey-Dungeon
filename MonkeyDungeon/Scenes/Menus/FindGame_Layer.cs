using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon.Prefabs.UI;
using MonkeyDungeon.Scenes.GameScenes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.Menus
{
    public class FindGame_Layer : MainMenu_Layer
    {
        TextField serverAddress;
        GameScene gameScene;

        public FindGame_Layer(MainMenuScene parentScene) 
            : base(parentScene)
        {
            gameScene = (GameScene)Game.SceneManagementService.GetScene("gameScene");

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-Game.Width / 2 + 20, -Game.Height / 2 + 20, 0),
                    new Vector2(200, 100),
                    () => { MainMenuScene.SetLayer<GreetingMenu_Layer>(); },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Main Menu"
                    )
                );


            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(Game.Width / 2 - 220, -Game.Height / 2 + 20, 0),
                    new Vector2(200, 100),
                    () => { },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Connect"
                    )
                );

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-Game.Width / 2 + 20, Game.Height / 2 - 120, 0),
                    new Vector2(200, 100),
                    () => { },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Host"
                    )
                );

            Add_StaticObject(
                serverAddress = new TextField(
                    this,
                    new Vector3(-100,-50,0),
                    new Vector2(200,100),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Server Address:"
                    )
                );
        }
    }
}
