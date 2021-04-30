using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.Prefabs.Entities;
using MonkeyDungeon.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class UI_Combat_Layer : UI_StateBased_Layer
    {
        private EntityController turnController;

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

        public UI_Combat_Layer(Scene parentScene, Combat_GameState combat)
            : base(parentScene, typeof(Combat_GameState))
        {
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
                    () => UseAbility(0),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(-100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    () => UseAbility(1),
                    Game.SpriteLibrary.ExtractRenderUnit("button"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width / 3f - 100, -Game.Height/2 + 20, 0),
                    new Vector2(200, 100),
                    () => UseAbility(2),
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
                    () => SelectEnemy(0),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height / 8f,0),
                    new Vector2(50,50),
                    () => SelectEnemy(1),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width/4f,Game.Height * 3f / 8f,0),
                    new Vector2(50,50),
                    () => SelectEnemy(2),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    ),
                new Button(
                    this,
                    new Vector3(Game.Width * 3 / 8,Game.Height/4,0),
                    new Vector2(50,50),
                    () => SelectEnemy(3),
                    Game.SpriteLibrary.ExtractRenderUnit("targetButton"),
                    ""
                    )
            };

            AddButtons(abilityButtons);
            AddButtons(targetEnemyButtons);
        }

        protected override void Handle_RenderLayer(RenderService renderService, FrameArgument e)
        {
            SetState_TargetEnemy_Buttons(turnController.PendingCombatAction?.Requires_Target ?? false);
            base.Handle_RenderLayer(renderService, e);
        }

        internal void BeginCombat()
        {
            Set_Ability_Names(Combat.Entity_OfCurrentTurn.Get_Ability_Names());
            Initalize_Buttons(abilityButtons, abilityNames.Length, (i) => abilityNames[i]);
            Initalize_Buttons(targetEnemyButtons, Combat.Enemies.Length, (i) => "");
        }

        internal void BeginTurn(EntityController turnController)
        {
            this.turnController = turnController;
            statusBar.SetTarget(turnController.Entity);
            Set_Ability_Names(turnController.Entity.Get_Ability_Names());
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

        private void UseAbility(int index)
        {
            turnController.Setup_CombatAction_Ability(abilityNames[index]);
        }

        private void SelectEnemy(int index)
        {
            turnController.Setup_CombatAction_Target(Combat.Enemies[index]);
        }

        private void AddButtons(Button[] buttons)
        {
            foreach (Button button in buttons)
                Add_StaticObject(button);
        }

        private void SetState_TargetEnemy_Buttons(bool state)
        {
            EntityComponent[] enemies = Combat.Enemies;
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
