using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Introduce_Entity : Multiplayer_Message_Wrapper
    {
        public MMW_Introduce_Entity(int entityId) 
            : base(MD_VANILLA_MMH.MMH_INTRODUCE_ENTITY, entityId)
        {
        }
    }
}
