using System;
using System.Collections.Generic;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon.GameFeatures.Multiplayer
{
    /// <summary>
    /// A relay endpoint for either the local UI or local gamestate (server)
    /// </summary>
    public class Local_Receiver : Multiplayer_Relay
    {
        private Action<Multiplayer_Message> Handler_Local_Endpoint_Delivery { get; set; }
        internal Queue<Multiplayer_Message> Local_Inbox { get; set; }

        internal void Set_Local_Endpoint(Action<Multiplayer_Message> localEndpoint)
        {
            Handler_Local_Endpoint_Delivery = localEndpoint;
        }
        
        public Local_Receiver()
            : base("0", 0) //sockets are not used, address and port are ignored.
        {
            Local_Inbox = new Queue<Multiplayer_Message>();
        }

        protected override Multiplayer_Message Handle_Read_New_Message()
            => (Local_Inbox.Count > 0) ? Local_Inbox.Dequeue() : Multiplayer_Message.MESSAGE_NULL;

        public override void Flush_Messages()
        {
            while (QueuedMessages.Count > 0)
                Handler_Local_Endpoint_Delivery(QueuedMessages.Dequeue());
        }
    }
}
