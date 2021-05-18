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
        public void Register_Handler(Multiplayer_Message_Handler handler)
            => Handlers.Add(handler);

        public void Link(Multiplayer_Reciever reciever)
        {
            foreach (Multiplayer_Message_Handler mmh in Handlers)
                reciever.Register_Handler(mmh);
        }
    }
}
