using isometricgame.GameEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.GameStates
{
    public class Traveling_GameState : GameState
    {
        Timer timer = new Timer(1f);

        public Traveling_GameState(Action stateBegun, Action stateConcluded)
            : base(stateBegun, stateConcluded)
        {
        }

        protected override void Handle_BeginState(GameState_Machine gameWorld)
        {
            timer.Set();
        }

        protected override void Handle_EndState(GameState_Machine gameWorld)
        {

        }

        protected override void Handle_UpdateState(GameState_Machine gameWorld)
        {
            gameWorld.Request_Transition_ToState<Combat_GameState>();
        }
    }
}
