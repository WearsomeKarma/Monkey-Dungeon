using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.CombatObjects;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class GameScene : Scene
    {
        internal GameState_Machine Game_StateMachine { get; set; }
        private bool IsGameState_Available => Game_StateMachine.CurrentGameState != null;

        private UI_Base_Layer UI_Base_Layer { get; set; }
        private UI_Combat_Layer UI_Combat_Layer { get; set; }
        private UI_Shopping_Layer UI_Shopping_Layer { get; set; }
        private UI_StateBased_Layer[] UI_StateBased_Layers { get; set; }

        private World_Layer World_Layer { get; set; }

        private Combat_GameState Combat_GameState { get; set; }
        private Shopping_GameState Shopping_GameState { get; set; }
        private Traveling_GameState Traveling_GameState { get; set; }
        private GameOver_GameState GameOver_GameState { get; set; }
        
        internal EventScheduler EventScheduler { get; set; }

        public GameScene(Game game) 
            : base(game)
        {
            EventScheduler = new EventScheduler();

            Game_StateMachine = new GameState_Machine(
                this,
                new GameState[]
                {
                    Traveling_GameState = new Traveling_GameState(
                        () => { Console.WriteLine("[GameState] Traveling"); }, 
                        () => { }),
                    Combat_GameState = new Combat_GameState(
                        () => Begin_Combat(Combat_GameState), 
                        () => End_Combat(), 
                        (controller) => Begin_CombatTurn(controller)),
                    Shopping_GameState = new Shopping_GameState(
                        () => { }, 
                        () => { }),
                    GameOver_GameState = new GameOver_GameState(
                        () => { Console.WriteLine("Game Over."); },
                        () => { }
                        )
                }
                );

            AddLayers(
                World_Layer = new World_Layer(this, Combat_GameState),
                UI_Combat_Layer = new UI_Combat_Layer(this, Combat_GameState),
                UI_Shopping_Layer = new UI_Shopping_Layer(this),
                UI_Base_Layer = new UI_Base_Layer(this)
                );
            EnableOnlyLayer<World_Layer>();
            EnableLayers<UI_Base_Layer>();

            UI_StateBased_Layers = new UI_StateBased_Layer[] { UI_Combat_Layer, UI_Shopping_Layer };
        }
        
        protected override void Handle_UpdateScene(FrameArgument e)
        {
            EventScheduler.Progress_Events(e.DeltaTime);
            if (!EventScheduler.IsActive)
                Game_StateMachine.CheckFor_GameState_Transition();
            base.Handle_UpdateScene(e);
        }

        private void Begin_Combat(Combat_GameState combat)
        {
            Console.WriteLine("[GameState] Begin combat.");
            UI_Combat_Layer.BeginCombat();
        }

        private void Begin_CombatTurn(GameEntity_Controller turnController)
        {
            if (!turnController.IsAutomonous)
            {
                EnableLayers<UI_Combat_Layer>();
                UI_Combat_Layer.BeginTurn(turnController);
                return;
            }
            DisableLayers<UI_Combat_Layer>();
        }

        private void End_Combat()
        {
            UI_Combat_Layer.EndCombat();

            DisableLayers<UI_Combat_Layer>();
        }

        internal void Act_MeleeAttack(int ownerId, int targetId)
        {
            World_Layer.Act_MeleeAttack(ownerId, targetId);
        }

        internal void Announce_Action(Combat_Action action)
        {
            UI_Base_Layer.Announce(action.CombatAction_Ability_Name);
            Console.WriteLine(
                action
                );
            //throw new NotImplementedException();
        }

        internal void Announce_ActionFailure(Combat_Action action)
        {
            Console.WriteLine(
                String.Format(
                    "Failure to use ability: {0}",
                    action.CombatAction_Ability_Name
                    )
                );
        }
    }
}
