using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Multiplayer.Handlers;
using MonkeyDungeon_UI.Multiplayer.MessageWrappers;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_UI.UI_Events.Implemented;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;
using System;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class UI_Combat_Layer : GameScene_Layer
    {
        internal GameEntity_ClientSide_Ability Selected_Ability { get; private set; }

        private GameScene GameScene { get; set; }
        private World_Layer World_Layer { get; set; }
        
        private UI_GameEntity_Survey_Target_Buttons Target_Buttons { get; set; }
        
        private UI_Panning_Event UI_Announcement_Event { get; set; }

        private UI_AnnouncementMessage _uiAnnouncementMessage;
        private UI_EndTurnUiButton _uiEndTurnUiButton;
        private UI_StatusBar _uiStatusBar;

        private GameEntity_ClientSide_Ability[] abilityNames_ToRender;
        private UI_Button[] abilityButtons;
        
        private void Set_Ability_Names(GameEntity_ClientSide_Ability[] abilities)
        {
            for (int i = 0; i < abilityButtons.Length; i++)
            {
                bool state = abilities[i] != null && abilities.Length > i;
            
                abilityButtons[i].Text = state ? abilities[i].Ability_Name : GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME;
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
                _uiStatusBar = new UI_StatusBar(this, new Vector3(-Game.Width / 2, Game.Height / 2 - 375, 0))
                );

            Add_StaticObject(
                _uiEndTurnUiButton = new UI_EndTurnUiButton(this, new Vector3(Game.Width/2 - 130, -Game.Height/2+130, 0))
                );

            abilityButtons = new UI_Button[]
            {
                new UI_Button(
                    this,
                    new Vector3(-Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(GameEntity_Ability_Index.INDEX_ZERO),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new UI_Button(
                    this,
                    new Vector3(-100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(GameEntity_Ability_Index.INDEX_ONE),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new UI_Button(
                    this,
                    new Vector3(Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    (b) => Use_Ability(GameEntity_Ability_Index.INDEX_TWO), //TODO: fix
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    )
            };

            Target_Buttons = new UI_GameEntity_Survey_Target_Buttons
                (
                    this,
                    Get_UI_TargetPositions(Game),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton")
                );

            Target_Buttons.Button_Clicked += Select_Enemy;

            Add_StaticObject(
                _uiAnnouncementMessage = new UI_AnnouncementMessage(this, new Vector3(-Game.Width / 2 - 320, Game.Height / 4, 0))
                );

            UI_Announcement_Event = new UI_Announcement_Event(
                Game.EventScheduler,
                _uiAnnouncementMessage,
                _uiAnnouncementMessage.Position,
                new Vector3(0,_uiAnnouncementMessage.Position.Y,0)
                );
            
            Add_Handler_Expectation(
                new MMH_Begin_Turn(this),
                new MMH_Announcement(this)
                );

            Add_Buttons(abilityButtons);
            Add_Buttons(Target_Buttons.Get_Buttons());
        }

        protected override void Handle_UpdateLayer(FrameArgument e)
        {
            base.Handle_UpdateLayer(e);
            Inform_Ability();
        }

        private bool Validate_Action_Selected()
            => Selected_Ability != null;
        
        private bool Validate_Client_Action()
            => Validate_Action_Selected() && Selected_Ability.Ability_Usage_Finished;
        
        private void Inform_Ability()
        {
            if (Validate_Client_Action())
            {
                Relay_Client_Action();
                
                Reset_Selections();
                return;
            }
            
            Update_Target_Buttons();
        }

        private void Relay_Client_Action()
        {
            Relay_Message(
                new MMW_Combat_Set_Selected_Ability(Selected_Ability)
            );

            foreach (GameEntity_Position position in Selected_Ability.Target.Get_Reduced_Fields())
            {
                Relay_Message(
                    new MMW_Combat_Add_Target(
                        position
                        )
                    );
            }
        }
        
        internal void Inform_EndTurn()
        {
            Relay_Message(
                new MMW_Request_EndTurn()
                );
            _uiStatusBar.Dump_AbilityPoints();
        }

        internal void BeginTurn(GameEntity_ID entityId)
        {
            _uiEndTurnUiButton.RefreshButton();
            Target_Buttons.Set_Button_States(GameEntity_Team_ID.ID_NULL, false);
            Reset_Selections();
            GameEntity_ClientSide entity = World_Layer.Get_GameEntity(entityId);
            abilityNames_ToRender = entity.ABILITIES;
            Set_Ability_Names(abilityNames_ToRender);
            Focus_Entity(entity);
        }

        internal void Announce_ActionFailure(string combatAction_Ability_Name)
        {
            Console.WriteLine("action failure> " + combatAction_Ability_Name);
        }

        internal void Announce_Action(string combatAction_Ability_Name)
        {
            _uiAnnouncementMessage.Set_Announcement(combatAction_Ability_Name);
            EventScheduler.Invoke_Event(MD_VANILLA_UI_EVENT_NAMES.UI_EVENT_ANNOUNCEMENT);
        }

        internal void Focus_Entity(GameEntity_ClientSide entityDescription)
        {
            _uiStatusBar.Set_EntityFocus(entityDescription);
        }

        internal void Reset_Selections()
        {
            if (Selected_Ability != null)
                Selected_Ability.Target.Reset();
            
            Console.WriteLine("[UI_Combat_Layer.cs:205] -- Resetting selections.");
            
            Selected_Ability = null;
        }

        internal void Update_Target_Buttons()
        {
            //TODO: fully implement.
            Combat_Target_Type targetType = Selected_Ability?.Target_Type ?? Combat_Target_Type.Self_Or_No_Target;
            switch (targetType)
            {
                case Combat_Target_Type.Everything:
                case Combat_Target_Type.All_Enemies:
                case Combat_Target_Type.All_Friendlies:
                case Combat_Target_Type.Self_Or_No_Target:    
                    Target_Buttons.Set_Button_States(GameEntity_Team_ID.ID_NULL, false);
                    return;
                case Combat_Target_Type.Three_Enemies:
                case Combat_Target_Type.Two_Enemies:
                case Combat_Target_Type.One_Enemy: 
                    Set_Conscious_Target_Buttons(GameEntity_Team_ID.TEAM_TWO_ID);
                    return;
                default:
                    Set_Conscious_Target_Buttons(GameEntity_Team_ID.TEAM_ONE_ID);
                    return;
            }
        }

        private void Set_Conscious_Target_Buttons(GameEntity_Team_ID teamId)
        {
            GameEntity_Position.For_Each_Position(teamId, (p) =>
            {
                GameEntity_ClientSide entity = World_Layer.Get_GameEntity(p);
                Target_Buttons.Set_Button_State(p, !(entity.IsDismissed || entity.IsIncapacitated));
            });
        }

        internal void Initalize_Buttons(UI_Button[] buttons, int comparingLength, Func<int, string> buttonTextHandler)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                bool isButtonValid = comparingLength > i;
                Set_Button(buttons[i], isButtonValid, (isButtonValid) ? buttonTextHandler(i) : "");
            }
        }

        private void Set_Button(UI_Button uiButton, bool state, string text = "")
        {
            uiButton.Text = text;
            uiButton.SpriteComponent.Enabled = state;
            uiButton.Enabled = state;
        }

        internal void EndCombat()
        {

        }
        
        private void Use_Ability(GameEntity_Ability_Index abilityIndex)
        {
            Console.WriteLine("Use_Ability");
            Selected_Ability = abilityNames_ToRender[abilityIndex];
        }

        private void Select_Enemy(GameEntity_Position position, UI_Button_Target button)
        {
            Console.WriteLine("Select_Enemy");
            Selected_Ability?.Target.Add_Target(position);
        }

        private void Add_Buttons(UI_Button[] buttons)
        {
            foreach (UI_Button button in buttons)
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
