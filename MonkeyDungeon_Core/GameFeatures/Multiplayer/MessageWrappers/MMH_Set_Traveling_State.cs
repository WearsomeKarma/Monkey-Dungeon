using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMH_Set_Traveling_State : Multiplayer_Message_Wrapper
    {
        public MMH_Set_Traveling_State(bool state) 
            : base(MD_VANILLA_MMH.MMH_SET_TRAVELING_STATE, 0, 0, state ? 1 : 0)
        {
        }
    }
}
