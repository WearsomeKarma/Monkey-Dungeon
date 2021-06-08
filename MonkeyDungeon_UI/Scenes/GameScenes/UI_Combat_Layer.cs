using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Multiplayer.Handlers;
using MonkeyDungeon_UI.Multiplayer.MessageWrappers;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.Prefabs.UI.EntityData;
using MonkeyDungeon_UI.UI_Events.Implemented;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class UI_Combat_Layer : GameScene_Layer
    {
        internal GameEntity_Attribute_Name Selected_Ability { get; private set; }
        internal GameEntity_ID Selected_TargetIndex { get; private set; }

        private GameScene GameScene { get; set; }
        private World_Layer World_Layer { get; set; }

        private AnnouncementMessage announcementMessage;
        private UI_Panning_Event UI_Announcement_Event { get; set; }

        private Button[] abilityButtons;
        private Button[] targetEnemyButtons;
        private EndTurnButton endTurnButton;

        private StatusBar statusBar;

        private GameEntity_Attribute_Name[] abilityNames;
        //TODO: prim wrap
        private void Set_Ability_Names(GameEntity_Attribute_Name[] newAbilityNames)
        {
            for (int i = 0; i < abilityButtons.Length; i++)
            {
                bool state = newAbilityNames[i] != null && newAbilityNames.Length > i;
            
                abilityButtons[i].Text = state ? newAbilityNames[i] : GameEntity_Attribute_Name.DEFAULT;
                abilityButtons[i].Enabled = state;
                abilityButtons[i].SpriteComponent.Enabled = state;
            }
        }

        internal UI_Combat_Layer(GameScene parentScene)
            : base(parentScene, UI_GENERIC_LAYER_INDEX)
        {
            GameScene = parentScene;
            World_Layer = GameScene.World_Layer;

            Reset_Selections();
            
            Add_StaticObject(
                statusBar = new StatusBar(this, new Vector3(-Game.Width / 2, Game.Height / 2 - 375, 0))
                );

            Add_StaticObject(
                endTurnButton = new EndTurnButton(this, new Vector3(Game.Width/2 - 130, -Game.Height/2+130, 0))
                );

            abilityButtons = new Button[]
            {
                new Button(
                    this,
                    new Vector3(-Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(abilityNames[0]),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(-100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(abilityNames[1]),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(abilityNames[2]), //TODO: fix
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    )
            };
            
            targetEnemyButtons = new Button[]
            {
                new Button(
                    this,
                    new Vector3(Game.Width/8f,Game.Height/4,0),
                    new Vector2(50,50),
                    (b) => Select_Enemy(GameEntity_ID.ID_FOUR),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height / 8f,0),
                    new Vector2(50,50),
                    (b) => Select_Enemy(GameEntity_ID.ID_FIVE),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height * 3f / 8f,0),
                    new Vector2(50,50),
                    (b) => Select_Enemy(GameEntity_ID.ID_SIX),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width * 3 / 8,Game.Height/4,0),
                    new Vector2(50,50),
                    (b) => Select_Enemy(GameEntity_ID.ID_SEVEN),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    )
            };

            Add_StaticObject(
                announcementMessage = new AnnouncementMessage(this, new Vector3(-Game.Width / 2 - 320, Game.Height / 4, 0))
                );

            UI_Announcement_Event = new UI_Announcement_Event(
                Game.EventScheduler,
                announcementMessage,
                announcementMessage.Position,
                new Vector3(0,announcementMessage.Position.Y,0)
                );
            
            GameScene.MonkeyDungeon_Game_UI.Expectation_Context.Register_Handler(
                new MMH_Begin_Turn(this),
                new MMH_Announcement(this)
                );

            Add_Buttons(abilityButtons);
            Add_Buttons(targetEnemyButtons);
        }
        
        protected override void Handle_RenderLayer(RenderService renderService, FrameArgument e)
        {
            base.Handle_RenderLayer(renderService, e);
        }

        protected override void Handle_UpdateLayer(FrameArgument e)
        {
            base.Handle_UpdateLayer(e);
            Inform_Ability();
        }

        private void Inform_Ability()
        {
            if (Selected_Ability != GameEntity_Attribute_Name.DEFAULT && Selected_TargetIndex != GameEntity_ID.ID_NULL)
            {
                GameScene.MonkeyDungeon_Game_UI.Client_RecieverEndpoint_UI.Queue_Message(
                    new MMW_Set_Combat_Action(Selected_TargetIndex, Selected_Ability)
                    );
                Selected_Ability = null;
                Selected_TargetIndex = GameEntity_ID.ID_NULL;
            }
        }

        internal void Inform_EndTurn()
        {
            GameScene.MonkeyDungeon_Game_UI.Client_RecieverEndpoint_UI.Queue_Message(
                new MMW_Request_EndTurn()
                );
            statusBar.Dump_AbilityPoints();
        }

        internal void Enable_TurnControl()
        {
            //TODO: enable ability buttons and end turn.
        }

        internal void Disable_TurnControl()
        {
            //TODO: disable ability buttons and end turn.
        }

        internal void BeginCombat(UI_GameEntity_Descriptor clientPlayer)
        {
            Focus_Entity(clientPlayer);
        }

        internal void BeginTurn(GameEntity_ID entityId)
        {
            endTurnButton.RefreshButton();
            Reset_Selections();
            UI_GameEntity_Descriptor entity = World_Layer.Get_Description_From_Id(entityId);
            abilityNames = entity.ABILITY_NAMES;
            Set_Ability_Names(abilityNames);
            Focus_Entity(entity);
        }

        internal void Announce_ActionFailure(string combatAction_Ability_Name)
        {
            Console.WriteLine("action failure> " + combatAction_Ability_Name);
        }

        internal void Announce_Action(string combatAction_Ability_Name)
        {
            announcementMessage.Set_Announcement(combatAction_Ability_Name);
            EventScheduler.Invoke_Event(MD_VANILLA_UI.UI_EVENT_ANNOUNCEMENT);
        }

        internal void Focus_Entity(UI_GameEntity_Descriptor entityDescription)
        {
            statusBar.Set_EntityFocus(entityDescription);
        }

        internal void Reset_Selections()
        {
            Selected_Ability = GameEntity_Attribute_Name.DEFAULT;
            Selected_TargetIndex = GameEntity_ID.ID_NULL;
        }

        internal void Initalize_Buttons(Button[] buttons, int comparingLength, Func<int, string> buttonTextHandler)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                bool isButtonValid = comparingLength > i;
                Set_Button(buttons[i], isButtonValid, (isButtonValid) ? buttonTextHandler(i) : "");
            }
        }

        private void Set_Button(Button button, bool state, string text = "")
        {
            button.Text = text;
            button.SpriteComponent.Enabled = state;
            button.Enabled = state;
        }

        internal void EndCombat()
        {

        }
        
        private void Use_Ability(GameEntity_Attribute_Name abilityName)
        {
            Console.WriteLine("Use_Ability");
            Selected_Ability = abilityName;
        }

        private void Select_Enemy(GameEntity_ID index)
        {
            Console.WriteLine("Select_Enemy");
            Selected_TargetIndex = index;
        }

        private void Add_Buttons(Button[] buttons)
        {
            foreach (Button button in buttons)
                Add_StaticObject(button);
        }

        internal static Vector3[] Get_UI_TargetPositions(Game game)
        {
            return new Vector3[] 
            {
                new Vector3(game.Width / 8f,          game.Height / 4f,           0),
                new Vector3(game.Width / 4f,          game.Height / 8f,           0),
                new Vector3(game.Width / 4f,          game.Height * 3f / 8f,      0),
                new Vector3(game.Width * 3f / 8f,     game.Height / 4f,           0),
            };
        }
    }
}
