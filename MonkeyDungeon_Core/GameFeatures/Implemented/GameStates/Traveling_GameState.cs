using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.GameStates
{
    public class Traveling_GameState : GameState
    {
        public Traveling_GameState()
        {
        }

        protected override void Handle_Update_State(GameState_Machine gameWorld)
        {
            gameWorld.Request_Transition_ToState<Combat_GameState>();
        }
    }
}
