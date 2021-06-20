using System;
using MonkeyDungeon_Core;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon.GameFeatures.Multiplayer
{
    /// <summary>
    ///     Interfaces between the Client_UI Local_Reciever and the GameState Local_Reciever.
    /// </summary>
    public class Local_Session
    {
        internal readonly Local_Receiver Client_Endpoint;
        internal readonly MonkeyDungeon_Server Server_Instance;

        public Local_Session(Local_Receiver clientEndpoint, Local_Receiver serverEndpoint)
        {
            Client_Endpoint = clientEndpoint;
            Server_Instance = new MonkeyDungeon_Server(serverEndpoint);

            Client_Endpoint.Set_Local_Endpoint(m =>
            {
                Write_Client_Tag("[SENT from Client]");
                Write_Data(m.ToString());

                Handle_Local_Endpoint(m, serverEndpoint);
                Write_Server_Tag("[PROCESSED]");
            });
            serverEndpoint.Set_Local_Endpoint(m =>
            {
                Write_Server_Tag("[SENT from Server]");
                Write_Data(m.ToString());

                Handle_Local_Endpoint(m, Client_Endpoint);
                Write_Client_Tag("[PROCESSED]");
            });
        }

        internal void On_Update_Frame(double deltaTime)
        {
            Client_Endpoint.CheckFor_NewMessages();

            Client_Endpoint.Flush_Messages();

            Server_Instance.On_Update_Frame(deltaTime);
        }

        private void Handle_Local_Endpoint(Multiplayer_Message message, Local_Receiver endpoint)
        {
            endpoint.Local_Inbox.Enqueue(message);
        }

        private void Write_Client_Tag(string m)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(m);
        }

        private void Write_Server_Tag(string m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m);
        }

        private void Write_Data(string m)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("{0}", m);
        }
    }
}