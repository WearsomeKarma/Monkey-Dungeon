using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.Menus
{
    public class GreetingMenu_Layer : MainMenu_Layer
    {
        GameObject background;

        internal GreetingMenu_Layer(MonkeyDungeon_Game_Client monkeyGame, MainMenuScene parentScene) 
            : base(monkeyGame, parentScene)
        {
            Add_StaticObject(
                background = new GameObject(this, new Vector3(-Game.Width/2,-Game.Height/2,0), "mainMenu")
                );

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -100, 0),
                    new Vector2(200, 100),
                    (b) => MainMenuScene.SetLayer<NewGame_Layer>(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Create Game"
                    ));
            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -250,0),
                    new Vector2(200,100),
                    (b) => MainMenuScene.SetLayer<FindGame_Layer>(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Multiplayer"
                    ));
            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-100, -400, 0),
                    new Vector2(200, 100),
                    (b) => Game.Close(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Quit"
                    ));
        }

        protected override void Handle_Rescale()
        {
            Game.SpriteLibrary.GetSprite("mainMenu").SetSize(new Vector2(Game.Width, Game.Height));
            if (background != null)
                background.Position = new Vector3(-Game.Width / 2, -Game.Height / 2, 0);
        }
    }
}
