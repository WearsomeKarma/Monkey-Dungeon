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
        public MMH_Set_Entity(GameState_Machine gameStateMachine)
            : base(gameStateMachine, MD_VANILLA_MMH.MMH_SET_ENTITY)
        { }

        protected override void Handle_Message(Multiplayer_Message recievedMessage)
        {
            int entityId = recievedMessage.ENTITY_ID;
            string factoryTag = recievedMessage.STRING_VALUE;

            GameEntity e = GameState_Machine.Set_Entity(entityId, factoryTag);
        }
    }
}
