using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Begin_Turn : Multiplayer_Message_Wrapper
    {
        public MMW_Begin_Turn(int entityId = 0) 
            : base(MD_VANILLA_MMH.MMH_BEGIN_TURN, entityId)
        {
        }
    }
}
