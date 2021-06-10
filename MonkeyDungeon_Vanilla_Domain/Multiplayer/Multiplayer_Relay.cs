using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public abstract class Multiplayer_Relay
    {
        public static readonly int UNBOUND_RELAY_ID = -1;

        public readonly string ENDPOINT_ADDRESS;
        public readonly int PORT;
        public Multiplayer_Relay_ID Relay_ID { get; internal set; }
        public bool Is_Unbound_Relay => Relay_ID == UNBOUND_RELAY_ID;
        
        internal int Message_Count { get; set; }

        internal Multiplayer_Relay_Manager Manager { get; set; }

        private List<Multiplayer_Message_Handler> Message_Reception_Table { get; set; }
        private List<string> Message_Reception_Table_Types { get; set; }
        /// <summary>
        /// These types are expected to be linked to an endpoint for proper gameplay behavior.
        /// </summary>
        public string[] Expected_Types => Message_Reception_Table_Types.ToArray();
        public void Register_Handler(params Multiplayer_Message_Handler[] messageHandlers)
        {
            foreach (Multiplayer_Message_Handler mmh in messageHandlers)
            {
                Message_Reception_Table.Add(mmh);
                mmh.Relay = this;
            }

            //add to the list of expecting types for endpoint linking.
            foreach (string s in Message_Reception_Table_Types.ToArray())
                if (!Message_Reception_Table_Types.Contains(s))
                    Message_Reception_Table_Types.Add(s);
        }

        protected Queue<Multiplayer_Message> QueuedMessages { get; set; }

        public Multiplayer_Relay(string endpoint_address, int port)
        {
            ENDPOINT_ADDRESS = endpoint_address;
            PORT = port;

            Relay_ID = Multiplayer_Relay_ID.ID_NULL;

            Message_Reception_Table = new List<Multiplayer_Message_Handler>();
            Message_Reception_Table_Types = new List<string>();

            QueuedMessages = new Queue<Multiplayer_Message>();
        }

        protected void Recieve_Message(Multiplayer_Message message)
        {
            foreach (Multiplayer_Message_Handler mmr in Message_Reception_Table)
                mmr.Assess_Message(message);
        }

        public void CheckFor_NewMessages()
        {
            Multiplayer_Message message;
            while ((message = Read_New_Message()) != Multiplayer_Message.MESSAGE_NULL)
            {
                Message_Count++;
                message.Message_ID = Message_Count;
                message.Relay_ID = Relay_ID;
                Recieve_Message(message);
            }
        }

        private Multiplayer_Message Read_New_Message()
        {
            Multiplayer_Message msg = Handle_Read_New_Message();
            msg.Relay_ID = Relay_ID;
            return msg;
        }
        protected abstract Multiplayer_Message Handle_Read_New_Message();

        public void Queue_Message(Multiplayer_Message message)
        {
            Message_Count++;
            message.Message_ID = Message_Count;
            QueuedMessages.Enqueue(message);
        }

        public abstract void Flush_Messages();
    }
}
