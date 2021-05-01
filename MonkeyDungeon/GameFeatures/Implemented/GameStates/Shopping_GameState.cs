using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.GameStates
{
    public class Shopping_GameState : GameState
    {
        public Shopping_GameState(Action stateBegun, Action stateConcluded) 
            : base(stateBegun, stateConcluded)
        {
        }

        protected override void Handle_BeginState(GameWorld_StateMachine gameWorld)
        {

        }
    }
}
