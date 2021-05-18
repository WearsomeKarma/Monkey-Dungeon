using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Accept_Client : Multiplayer_Message_Wrapper
    {
        public MMW_Accept_Client() 
            : base(MD_VANILLA_MMH.MMH_ACCEPT_CLIENT)
        {
        }
    }
}
