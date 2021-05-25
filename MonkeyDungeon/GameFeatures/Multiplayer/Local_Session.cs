using MonkeyDungeon.GameFeatures.Multiplayer.Local_Recievers;
using MonkeyDungeon_Core;
using MonkeyDungeon_Core.GameFeatures.Multiplayer;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Multiplayer
{
    /// <summary>
    /// Interfaces between the Client_UI Local_Reciever and the GameState Local_Reciever.
    /// </summary>
    public class Local_Session
    {
        internal readonly Local_Reciever Client_Endpoint;
        internal readonly MonkeyDungeon_Game_Server Server_Instance;

        public Local_Session(Local_Reciever clientEndpoint, Local_Reciever serverEndpoint)
        {
            Client_Endpoint = clientEndpoint;
            Server_Instance = new MonkeyDungeon_Game_Server(serverEndpoint);

            Client_Endpoint.Set_Local_Endpoint((m) => {
                Handle_Local_Endpoint(m, serverEndpoint);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("[SENT from Client]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0}", m);
            });
            serverEndpoint.Set_Local_Endpoint((m) => {
                Handle_Local_Endpoint(m, Client_Endpoint);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[SENT from Server]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0}", m);
            });
        }

        internal void On_Update_Frame(double deltaTime)
        {
            Client_Endpoint.CheckFor_NewMessages();
            
            Client_Endpoint.Flush_Messages();

            Server_Instance.On_Update_Frame(deltaTime);
        }

        private void Handle_Local_Endpoint(Multiplayer_Message message, Local_Reciever endpoint)
        {
            endpoint.Local_Inbox.Enqueue(message);
        }
    }
}
