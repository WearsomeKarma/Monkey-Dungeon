using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public abstract class Multiplayer_Message_Handler
    {
        internal readonly string[] ACCEPTED_MESSAGE_TYPES;
        
        public Multiplayer_Message_Handler(params string[] acceptedMessageTypes)
        {
            ACCEPTED_MESSAGE_TYPES = acceptedMessageTypes.ToArray();
        }

        internal void Assess_Message(Multiplayer_Message recievedMessage)
        {
            if (ACCEPTED_MESSAGE_TYPES.Contains(recievedMessage.MESSAGE_TYPE))
                Handle_Message(recievedMessage);
        }

        protected abstract void Handle_Message(Multiplayer_Message recievedMessage);
    }
}
