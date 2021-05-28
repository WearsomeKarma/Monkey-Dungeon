using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int entityId = recievedMessage.ENTITY_ID;
            string factoryTag = recievedMessage.STRING_VALUE;

            if(IsInvalid_Message(relayId, entityId))
            {
                Handle_Invalid_Message(recievedMessage);
                return;
            }

            GameEntity e = GameState_Machine.Set_Entity(entityId, relayId, factoryTag);
        }

        private bool IsInvalid_Message(int relayId, int entityId)
        {
            GameEntity entity = GameState_Machine.Get_Entity(entityId);

            return
                (entityId >= GameState_Machine.MAX_TEAM_SIZE)
                ||
                (
                entity != null
                &&
                entity.Relay_ID_Of_Owner == relayId
                );
        }
    }
}
