using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.GameFeatures;
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
        internal GameWorld_StateMachine GameWorld { get; set; }
        private bool IsGameState_Available => GameWorld.CurrentGameState != null;

        private UI_Base_Layer UI_Base_Layer { get; set; }
        private UI_Combat_Layer UI_Combat_Layer { get; set; }
        private UI_Shopping_Layer UI_Shopping_Layer { get; set; }
        private UI_StateBased_Layer[] UI_StateBased_Layers { get; set; }

        private World_Layer World_Layer { get; set; }

        private Combat_GameState Combat_GameState { get; set; }
        private Shopping_GameState Shopping_GameState { get; set; }
        private Traveling_GameState Traveling_GameState { get; set; }

        public GameScene(Game game) 
            : base(game)
        {
            GameWorld = new GameWorld_StateMachine(
                this,
                new GameStateHandler[]
                {
                    Traveling_GameState = new Traveling_GameState(
                        () => { }, 
                        () => { }),
                    Combat_GameState = new Combat_GameState(
                        () => Begin_Combat(Combat_GameState), 
                        () => End_Combat(), 
                        (controller) => Begin_CombatTurn(controller)),
                    Shopping_GameState = new Shopping_GameState(
                        () => { }, 
                        () => { })
                }
                );

            AddLayers(
                World_Layer = new World_Layer(this, Combat_GameState),
                UI_Base_Layer = new UI_Base_Layer(this),
                UI_Combat_Layer = new UI_Combat_Layer(this, Combat_GameState),
                UI_Shopping_Layer = new UI_Shopping_Layer(this)
                );
            EnableOnlyLayer<World_Layer>();
            EnableLayers<UI_Base_Layer>();

            UI_StateBased_Layers = new UI_StateBased_Layer[] { UI_Combat_Layer, UI_Shopping_Layer };
        }
        
        protected override void Handle_UpdateScene(FrameArgument e)
        {
            GameWorld.CheckFor_GameState_Transition(e.DeltaTime);
            base.Handle_UpdateScene(e);
        }

        private void Begin_Combat(Combat_GameState combat)
        {
            UI_Combat_Layer.BeginCombat();
        }

        private void Begin_CombatTurn(EntityController turnController)
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

        internal void Announce_Action(CombatAction action)
        {
            Console.WriteLine(
                action
                );
            //throw new NotImplementedException();
        }

        internal void Announce_ActionFailure(CombatAction action)
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
