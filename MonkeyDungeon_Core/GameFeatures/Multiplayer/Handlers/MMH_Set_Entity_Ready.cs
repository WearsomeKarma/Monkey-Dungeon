using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    /// <summary>
    /// This message handler is responsible for establishing character 
    /// information recieved from the client.
    /// INT - 0: ready | !0: not ready.
    /// </summary>
    public class MMH_Set_Entity_Ready : Multiplayer_Message_GameStateHandler
    {
        public MMH_Set_Entity_Ready(GameState gameState) 
            : base(gameState, MD_VANILLA_MMH.MMH_SET_ENTITY_READY)
        {
        }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;
            GameEntity_ID entityId = recievedMessage.ENTITY_ID;
            bool state = recievedMessage.INT_VALUE == 0;
            
            bool validMsg = GameState_Machine.IsMatching_Relay_Id(entityId, relayId);
            if (!validMsg)
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameState_Machine.PlayerRoster.Set_Ready_To_Start(entityId, state);
        }
    }
}
