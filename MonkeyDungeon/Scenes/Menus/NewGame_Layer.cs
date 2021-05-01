using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.Components.Implemented.PlayerClasses;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.ActingEntities;
using MonkeyDungeon.Prefabs.Entities;
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
    public class NewGame_Layer : MainMenu_Layer
    {
        Player player;
        GameScene gameScene;
        TextField playerName;
        MonkeyGame monkeyGame;

        public NewGame_Layer(MainMenuScene parentLayer) 
            : base(parentLayer)
        {
            gameScene = (GameScene)Game.SceneManagementService.GetScene("gameScene");
            monkeyGame = parentLayer.Game as MonkeyGame;

            Add_StaticObject(
                new GameObject(this, new Vector3(-Game.Width/2, -Game.Height/2, 0), "creationMenu")
                );

            Add_StaticObject(
                playerName = new TextField(
                    this,
                    new Vector3(-Game.Width/2+20, Game.Height/2 - 160, 0),
                    new Vector2(200, 100),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Player Name:"
                    )
                );

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(-Game.Width/4,0,0),
                    new Vector2(200,100),
                    () => { player.ClassId = (player.ClassId + Player.CLASSES.Length - 1) % Player.CLASSES.Length; },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "<"
                    )
                );

            Add_StaticObject(
                new Button(
                    this,
                    new Vector3(Game.Width / 4, 0, 0),
                    new Vector2(200, 100),
                    () => { player.ClassId = (player.ClassId + 1) % Player.CLASSES.Length; },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ">"
                    )
                );


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
                    () => CreatePlayer_BeginGame(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Begin"
                    )
                );

            Add_StaticObject(
                player = new Player(
                    this,
                    new Vector3(0, Game.Height/4, 0),
                    new EntityComponent()
                    )
                );
            player.EntityComponent = new EntityComponent();
        }

        private void CreatePlayer_BeginGame()
        {
            gameScene.GameWorld.PlayerRoster = new EntityRoster(new EntityComponent[]
            {
                monkeyGame.EntityComponentFactory.New_EntityComponent(Player.CLASSES[player.ClassId], new ActingEntity_Player()),
                monkeyGame.EntityComponentFactory.New_EntityComponent(ArcherClass.CLASS_NAME, new ActingEntity_Player()),
                monkeyGame.EntityComponentFactory.New_EntityComponent(ClericClass.CLASS_NAME, new ActingEntity_Player()),
                monkeyGame.EntityComponentFactory.New_EntityComponent(WizardClass.CLASS_NAME, new ActingEntity_Player()),
            });
            Game.SceneManagementService.SetScene("gameScene");
        }

        protected override void Handle_RenderLayer(RenderService renderService, FrameArgument e)
        {
            base.Handle_RenderLayer(renderService, e);
            Game.TextDisplayer.DrawText(renderService, Player.CLASSES[player.ClassId], "font", 9 * Player.CLASSES[player.ClassId].Length, 50);
        }
    }
}
