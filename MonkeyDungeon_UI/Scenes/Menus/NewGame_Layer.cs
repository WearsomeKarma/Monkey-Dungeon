using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Multiplayer.MessageWrappers;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_UI.Scenes.Menus
{
    public class NewGame_Layer : MainMenu_Layer
    {
        UI_EntityObject player;
        GameScene gameScene;
        UI_TextField playerName;
        MonkeyDungeon_Game_Client monkeyGame;

        //TODO: Move this to a more centralized area.
        private readonly uint classCount = 6;
        private uint classSelection = 0;
        private readonly GameEntity_Attribute_Name[] CLASSES = MD_VANILLA_RACE_NAMES.CLASSES;
        
        internal NewGame_Layer(MonkeyDungeon_Game_Client monkeyGame, MainMenuScene parentLayer)
            : base(monkeyGame, parentLayer)
        {
            this.monkeyGame = monkeyGame;
            gameScene = (GameScene)Game.SceneManagementService.GetScene("gameScene");

            Add_StaticObject(
                new GameObject(this, new Vector3(-Game.Width / 2, -Game.Height / 2, 0), "creationMenu")
                );

            Add_StaticObject(
                playerName = new UI_TextField(
                    this,
                    new Vector3(-Game.Width / 2 + 20, Game.Height / 2 - 160, 0),
                    new Vector2(200, 100),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Player Name:"
                    )
                );

            Add_StaticObject(
                new UI_Button(
                    this,
                    new Vector3(-Game.Width / 4, 0, 0),
                    new Vector2(200, 100),
                    (b) => Decrememnt_Class_Selection(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "<"
                    )
                );

            Add_StaticObject(
                new UI_Button(
                    this,
                    new Vector3(Game.Width / 4, 0, 0),
                    new Vector2(200, 100),
                    (b) => Increment_Class_Selection(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ">"
                    )
                );


            Add_StaticObject(
                new UI_Button(
                    this,
                    new Vector3(-Game.Width / 2 + 20, -Game.Height / 2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => { MainMenuScene.SetLayer<GreetingMenu_Layer>(); },
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Main Menu"
                    )
                );


            Add_StaticObject(
                new UI_Button(
                    this,
                    new Vector3(Game.Width / 2 - 220, -Game.Height / 2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => CreatePlayer_BeginGame(),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    "Begin"
                    )
                );

            //throw new NotImplementedException();
            Add_StaticObject(
                player = new UI_EntityObject(
                    this,
                    new Vector3(0, Game.Height / 4, 0)
                    )
                );
        }

        private void Increment_Class_Selection()
        {
            classSelection = Adjust_Player_TypeIndex(classSelection, 1, classCount);
            Update_Player();
        }

        private void Decrememnt_Class_Selection()
        {
            classSelection = Adjust_Player_TypeIndex(classSelection, classCount - 1, classCount);
            Update_Player();
        }

        private uint Adjust_Player_TypeIndex(uint val, uint adjustment, uint limit)
            => (val + adjustment) % limit;

        private void Update_Player()
        {
            player.UniqueIdentifier_TypeIndex = classSelection;
        }

        private void CreatePlayer_BeginGame()
        {
            monkeyGame.Create_Local_Game();
            monkeyGame.Link_Endpoint();

            MMW_Set_Entity[] setPlayerCommands = new MMW_Set_Entity[]
            {
                //TODO: Fix non-centralized primitives
                new MMW_Set_Entity(GameEntity_ID.ID_ZERO, CLASSES[classSelection]),
                new MMW_Set_Entity(GameEntity_ID.ID_ONE, CLASSES[2]),
                new MMW_Set_Entity(GameEntity_ID.ID_TWO, CLASSES[3]),
                new MMW_Set_Entity(GameEntity_ID.ID_THREE, CLASSES[1])
            };

            MMW_Set_Entity_Ready[] readyPlayerCommands = new MMW_Set_Entity_Ready[]
            {
                new MMW_Set_Entity_Ready(GameEntity_ID.ID_ZERO),
                new MMW_Set_Entity_Ready(GameEntity_ID.ID_ONE),
                new MMW_Set_Entity_Ready(GameEntity_ID.ID_TWO),
                new MMW_Set_Entity_Ready(GameEntity_ID.ID_THREE)
            };

            //send the set player / ready-up player commands.
            //TODO: Centralize max player count.
            for(int i=0;i<4;i++)
            {
                monkeyGame.Client_RecieverEndpoint_UI.Queue_Message(setPlayerCommands[i]);
                monkeyGame.Client_RecieverEndpoint_UI.Queue_Message(readyPlayerCommands[i]);
            }
            //monkeyGame.SceneManagementService.SetScene("gameScene");
        }

        protected override void Handle_Enabled()
        {
            base.Handle_Enabled();
        }

        protected override void Handle_RenderLayer(RenderService renderService, FrameArgument e)
        {
            base.Handle_RenderLayer(renderService, e);
            Game.TextDisplayer.DrawText(renderService, CLASSES[classSelection], "font", 9 * CLASSES[classSelection].NAME.Length, 50);
        }
    }
}
