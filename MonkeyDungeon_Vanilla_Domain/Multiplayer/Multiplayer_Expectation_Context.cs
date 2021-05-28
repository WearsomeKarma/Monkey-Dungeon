using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    /// <summary>
    /// This is what the UI/Client and GameState/Server attaches their
    /// multiplayer message handlers to. This is then linked to the
    /// recievers.
    /// </summary>
    public class Multiplayer_Expectation_Context
    {
        internal readonly List<Multiplayer_Message_Handler> Handlers = new List<Multiplayer_Message_Handler>();
        public void Register_Handler(params Multiplayer_Message_Handler[] handlers)
        {
            foreach (Multiplayer_Message_Handler mmh in handlers)
                Handlers.Add(mmh);
        }

        public void Link(Multiplayer_Relay reciever)
        {
            foreach (Multiplayer_Message_Handler mmh in Handlers)
                reciever.Register_Handler(mmh);
        }
    }
}
