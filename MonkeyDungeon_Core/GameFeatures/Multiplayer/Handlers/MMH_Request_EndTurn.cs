using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Request_EndTurn : Multiplayer_Message_GameState_Handler
    {
        Combat_GameState Combat { get; set; }

        public MMH_Request_EndTurn(Combat_GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_REQUEST_ENDTURN)
        {
            Combat = gameState;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;
            if (Combat.Entity_Of_Current_Turn_Relay_Id != relayId)
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            Combat.Request_EndOfTurn();
        }
    }
}
