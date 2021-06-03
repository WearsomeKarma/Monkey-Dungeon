using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Update_Entity_Resource : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Entity_Resource(GameEntity_ID entityId, float resourcePercentage, GameEntity_Attribute_Name resourceName) 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_RESOURCE, entityId, resourcePercentage, 0, resourceName)
        {
        }
    }
}
