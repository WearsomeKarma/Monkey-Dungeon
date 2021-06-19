using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    public class MMH_Request_EndTurn : Multiplayer_Message_GameState_Handler
    {
        GameState_Combat GameStateCombat { get; set; }

        public MMH_Request_EndTurn(GameState_Combat gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_REQUEST_ENDTURN)
        {
            GameStateCombat = gameState;
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;
            if (GameStateCombat.Entity_Of_Current_Turn_Relay_Id != relayId)
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameStateCombat.Request_EndOfTurn();
        }
    }
}
