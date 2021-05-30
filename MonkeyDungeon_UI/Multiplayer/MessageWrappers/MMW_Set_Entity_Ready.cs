using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Set_Entity_Ready : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Entity_Ready(GameEntity_ID entityId, int ival=0) 
            : base(MD_VANILLA_MMH.MMH_SET_ENTITY_READY, entityId, 0, ival)
        {
        }
    }
}
