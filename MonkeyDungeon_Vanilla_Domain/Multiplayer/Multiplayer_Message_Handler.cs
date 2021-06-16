using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public abstract class Multiplayer_Message_Handler
    {
        internal readonly GameEntity_Attribute_Name[] ACCEPTED_MESSAGE_TYPES;
     
        internal Multiplayer_Relay Relay { get; set; }
        
        public Multiplayer_Message_Handler(params GameEntity_Attribute_Name[] acceptedMessageTypes)
        {
            ACCEPTED_MESSAGE_TYPES = acceptedMessageTypes.ToArray();
        }

        internal void Assess_Message(Multiplayer_Message recievedMessage)
        {
            if (ACCEPTED_MESSAGE_TYPES.Contains(recievedMessage.MESSAGE_TYPE))
                Handle_Message(recievedMessage);
        }

        protected abstract void Handle_Message(Multiplayer_Message recievedMessage);

        protected virtual void Handle_Invalid_Message(Multiplayer_Message invalidMessage)
        {
            Relay.Queue_Message(
                new Multiplayer_Message(
                    Multiplayer_Message.MM_MESSAGE_INVALID,
                    invalidMessage.ENTITY_ID, 0,
                    invalidMessage.Message_ID
                    )
                );
        }
    }
}
