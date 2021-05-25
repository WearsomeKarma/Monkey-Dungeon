using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Request_EndTurn : Multiplayer_Message_Wrapper
    {
        public MMW_Request_EndTurn() 
            : base(MD_VANILLA_MMH.MMH_REQUEST_ENDTURN, 0, 0, 0, "")
        {
        }
    }
}
