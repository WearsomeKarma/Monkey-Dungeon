using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Update_Entity_Resource : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Entity_Resource(int entityId = 0, float resourcePercentage = 0, string resourceName = "") 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_RESOURCE, entityId, resourcePercentage, 0, resourceName)
        {
        }
    }
}
