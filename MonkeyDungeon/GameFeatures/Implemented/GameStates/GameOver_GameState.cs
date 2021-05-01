using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.GameStates
{
    public class GameOver_GameState : GameState
    {
        public GameOver_GameState(Action stateBegun, Action stateConcluded) 
            : base(stateBegun, stateConcluded)
        {
        }
    }
}
