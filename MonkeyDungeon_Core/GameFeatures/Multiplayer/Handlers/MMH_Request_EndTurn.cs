using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Request_EndTurn : Multiplayer_Message_GameStateHandler
    {
        public MMH_Request_EndTurn(GameState_Machine gameStateMachine) 
            : base(gameStateMachine, MD_VANILLA_MMH.MMH_REQUEST_ENDTURN)
        {
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            //TODO: Check sender

            GameState_Machine.Get_GameState<Combat_GameState>().Request_EndOfTurn();
        }
    }
}
