using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.GameStates
{
    public class Traveling_GameState : GameStateHandler
    {
        Timer timer = new Timer(1f);

        public Traveling_GameState(Action stateBegun, Action stateConcluded)
            : base(stateBegun, stateConcluded)
        {
        }

        protected override void BeginState(GameWorld_StateMachine gameWorld)
        {
            timer.Set();
        }

        protected override void EndState(GameWorld_StateMachine gameWorld)
        {

        }

        protected override void HandleUpdateState(GameWorld_StateMachine gameWorld, double deltaTime)
        {
            timer.DeltaTime((float)deltaTime);
            if (timer.Finished)
                gameWorld.Request_Transition_ToState<Combat_GameState>();
        }
    }
}
