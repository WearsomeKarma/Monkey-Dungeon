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
    public class MMW_Declare_Entity_Descriptions : Multiplayer_Message_Wrapper
    {
        public MMW_Declare_Entity_Descriptions(GameEntity_ID id, GameEntity_Attribute_Name factoryTag) 
            : base(MD_VANILLA_MMH.MMH_DECLARE_ENTITY_DESCRIPTION, id, 0, 0, factoryTag)
        {
        }
    }
}
