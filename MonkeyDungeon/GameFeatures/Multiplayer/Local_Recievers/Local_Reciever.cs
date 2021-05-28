using MonkeyDungeon_Core.GameFeatures.Multiplayer;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Multiplayer.Local_Recievers
{
    /// <summary>
    /// A relay endpoint for either the local UI or local gamestate (server)
    /// </summary>
    public class Local_Reciever : Multiplayer_Relay
    {
        private Action<Multiplayer_Message> Local_Endpoint_Delivery { get; set; }
        internal Queue<Multiplayer_Message> Local_Inbox { get; set; }

        public Local_Reciever()
            : base("0", 0) //sockets are not used, address and port are ignored.
        {
            Local_Inbox = new Queue<Multiplayer_Message>();
        }

        protected override Multiplayer_Message Handle_Read_New_Message()
            => (Local_Inbox.Count > 0) ? Local_Inbox.Dequeue() : Multiplayer_Message.MESSAGE_NULL;

        internal void Set_Local_Endpoint(Action<Multiplayer_Message> localEndpoint)
        {
            Local_Endpoint_Delivery = localEndpoint;
        }

        public override void Flush_Messages()
        {
            while (QueuedMessages.Count > 0)
                Local_Endpoint_Delivery(QueuedMessages.Dequeue());
        }
    }
}
