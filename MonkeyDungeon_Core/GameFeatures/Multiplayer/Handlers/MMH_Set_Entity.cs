using MonkeyDungeon_Core.GameFeatures.GameEntities;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.Handlers
{
    /// <summary>
    /// This message handler is responsible for establishing character 
    /// information recieved from the client.
    /// INT - Unique_ID.
    /// STIRNG - Factory_Tag.
    /// </summary>
    public class MMH_Set_Entity : Multiplayer_Message_GameState_Handler
    {
        public MMH_Set_Entity(GameState gameState)
            : base(gameState, MD_VANILLA_MMH.MMH_SET_ENTITY)
        { }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            Multiplayer_Relay_ID relayId = recievedMessage.Relay_ID;
            GameEntity_ID messageEntityID = recievedMessage.Local_Entity_ID;
            GameEntity_Attribute_Name factoryTag = recievedMessage.ATTRIBUTE;
            
            if(messageEntityID.IsRelay_Bound && IsInvalid_Message(relayId, messageEntityID))
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameEntity e = GameState_Machine.Set_Entity(messageEntityID, relayId, factoryTag);
        }
        
        private bool IsInvalid_Message(Multiplayer_Relay_ID relayId, GameEntity_ID entityId)
        {
            return
                (entityId >= MD_PARTY.MAX_PARTY_SIZE)
                ||
                entityId.Relay_ID != relayId
                ;
        }
    }
}
