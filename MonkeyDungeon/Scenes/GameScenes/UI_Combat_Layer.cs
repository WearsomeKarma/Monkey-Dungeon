using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.EntityResourceManagement;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Prefabs.UI;
using OpenTK;
using System;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class UI_Combat_Layer : UI_StateBased_Layer
    {
        private GameScene GameScene { get; set; }

        private GameEntity_Controller turnController;

        private Button[] abilityButtons;
        private Button[] targetEnemyButtons;
        private Button endTurnButton;
        private StatusBar statusBar;

        private string[] abilityNames;
        private void Set_Ability_Names(string[] abilityNames)
        {
            this.abilityNames = abilityNames;

            for (int i = 0; i < abilityButtons.Length; i++)
            {
                bool state = abilityNames.Length > i;
                if (state)
                {
                    abilityButtons[i].Text = abilityNames[i];
                }
                abilityButtons[i].Enabled = true;
                abilityButtons[i].SpriteComponent.Enabled = true;
            }
        }

        private Combat_GameState Combat { get; set; }
        
        public UI_Combat_Layer(GameScene parentScene, Combat_GameState combat)
            : base(parentScene, typeof(Combat_GameState))
        {
            GameScene = parentScene;
            Combat = combat;

            Add_StaticObject(
                statusBar = new StatusBar(this, new Vector3(-Game.Width / 2, Game.Height / 2 - 375, 0))
                );

            abilityButtons = new Button[]
            {
                new Button(
                    this,
                    new Vector3(-Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    () => Use_Ability(0),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(-100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    () => Use_Ability(1),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    () => Use_Ability(2),
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
                    () => Select_Enemy(0),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height / 8f,0),
                    new Vector2(50,50),
                    () => Select_Enemy(1),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height * 3f / 8f,0),
                    new Vector2(50,50),
                    () => Select_Enemy(2),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width * 3 / 8,Game.Height/4,0),
                    new Vector2(50,50),
                    () => Select_Enemy(3),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    )
            };

            Add_Buttons(abilityButtons);
            Add_Buttons(targetEnemyButtons);
        }

        protected override void Handle_RenderLayer(RenderService renderService, FrameArgument e)
        {
            SetState_TargetEnemy_Buttons(turnController.PendingCombatAction?.Requires_Target ?? false);
            base.Handle_RenderLayer(renderService, e);
        }

        internal void BeginCombat()
        {
            Set_Ability_Names(Combat.Entity_OfCurrentTurn.Ability_Manager.Get_Ability_Names());
            Initalize_Buttons(abilityButtons, abilityNames.Length, (i) => abilityNames[i]);
            Initalize_Buttons(targetEnemyButtons, Combat.Enemies.Length, (i) => "");
        }

        internal void BeginTurn(GameEntity_Controller turnController)
        {
            this.turnController = turnController;
            statusBar.Update_StatusBar(
                turnController.Entity.Race,
                (float)turnController.Entity.Resource_Manager.Get_Resource_Percentage(ENTITY_RESOURCES.HEALTH),
                (float)turnController.Entity.Resource_Manager.Get_Resource_Percentage(ENTITY_RESOURCES.STAMINA),
                (float)turnController.Entity.Resource_Manager.Get_Resource_Percentage(ENTITY_RESOURCES.MANA)
                );
            Set_Ability_Names(turnController.Entity.Ability_Manager.Get_Ability_Names());
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
        
        private void Use_Ability(int index)
        {
            Console.WriteLine("Use_Ability");
            turnController.Setup_CombatAction_Ability(GameScene, Combat, abilityNames[index]);
        }

        private void Select_Enemy(int index)
        {
            Console.WriteLine("Select_Enemy");
            turnController.Setup_CombatAction_Target(Combat.Enemies[index]);
        }

        private void Add_Buttons(Button[] buttons)
        {
            foreach (Button button in buttons)
                Add_StaticObject(button);
        }

        private void SetState_TargetEnemy_Buttons(bool state)
        {
            GameEntity[] enemies = Combat.Enemies;
            for (int i = 0; i < enemies.Length; i++)
            {
                targetEnemyButtons[i].Enabled = state;
                targetEnemyButtons[i].SpriteComponent.Enabled = state;
            }
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
