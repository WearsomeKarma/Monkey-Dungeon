using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public abstract class Multiplayer_Reciever
    {
        public readonly string ENDPOINT_ADDRESS;
        public readonly int PORT;

        private List<Multiplayer_Message_Handler> Message_Reception_Table { get; set; }
        private List<string> Message_Reception_Table_Types { get; set; }
        /// <summary>
        /// These types are expected to be linked to an endpoint for proper gameplay behavior.
        /// </summary>
        public string[] Expected_Types => Message_Reception_Table_Types.ToArray();
        public void Register_Handler(params Multiplayer_Message_Handler[] messageHandlers)
        {
            foreach(Multiplayer_Message_Handler mmh in messageHandlers)
                Message_Reception_Table.Add(mmh);

            //add to the list of expecting types for endpoint linking.
            foreach (string s in Message_Reception_Table_Types.ToArray())
                if (!Message_Reception_Table_Types.Contains(s))
                    Message_Reception_Table_Types.Add(s);
        }

        protected Queue<Multiplayer_Message> QueuedMessages { get; set; }

        public Multiplayer_Reciever(string endpoint_address, int port)
        {
            ENDPOINT_ADDRESS = endpoint_address;
            PORT = port;

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
            while ((message = Read_New_Message()) != Multiplayer_Message.DEFAULT)
            {
                Recieve_Message(message);
            }
        }

        protected abstract Multiplayer_Message Read_New_Message();

        public void Queue_Message(Multiplayer_Message message)
            => QueuedMessages.Enqueue(message);

        public abstract void Flush_Messages();
    }
}
