using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public class Multiplayer_Message_Wrapper
    {
        public readonly Multiplayer_Message MESSAGE;

        public Multiplayer_Message_Wrapper(
            string messageType = null,
            int entityId=0,
            float fval=0,
            int ival=0,
            string sval=""
            )
        {
            MESSAGE = new Multiplayer_Message(
                messageType,
                entityId,
                fval,
                ival,
                sval
                );
        }

        public static implicit operator Multiplayer_Message(Multiplayer_Message_Wrapper mmw)
            => mmw.MESSAGE;
    }
}
