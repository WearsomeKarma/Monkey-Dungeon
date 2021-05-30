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
    public class MMH_Set_Entity : Multiplayer_Message_GameStateHandler
    {
        public MMH_Set_Entity(GameState gameState)
            : base(gameState, MD_VANILLA_MMH.MMH_SET_ENTITY)
        { }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int relayId = recievedMessage.Relay_ID;
            GameEntity_ID entityId = recievedMessage.ENTITY_ID;
            GameEntity_Attribute_Name factoryTag = recievedMessage.ATTRIBUTE;

            if(IsInvalid_Message(relayId, entityId))
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameEntity e = GameState_Machine.Set_Entity(entityId, relayId, factoryTag);
        }

        private bool IsInvalid_Message(int relayId, GameEntity_ID entityId)
        {
            GameEntity entity = GameState_Machine.Get_Entity(entityId);

            return
                (entityId >= MD_PARTY.MAX_PARTY_SIZE)
                ||
                (
                entity != null
                &&
                entity.Relay_ID_Of_Owner == relayId
                );
        }
    }
}
