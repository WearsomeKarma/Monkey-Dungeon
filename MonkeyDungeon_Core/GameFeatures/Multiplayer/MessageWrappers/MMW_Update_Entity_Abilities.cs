using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Update_Entity_Abilities : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Entity_Abilities(int entityId = 0, params string[] abilities) 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITIES, entityId, 0, 0, String.Join(",", abilities))
        {
        }
    }
}
