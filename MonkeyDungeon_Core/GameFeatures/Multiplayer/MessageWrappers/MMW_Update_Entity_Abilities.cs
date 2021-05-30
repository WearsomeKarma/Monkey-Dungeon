using MonkeyDungeon_Vanilla_Domain;
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
        public MMW_Update_Entity_Abilities(GameEntity_ID entityId, GameEntity_Attribute_Name abilityName) 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITIES, entityId, 0, 0, abilityName)
        {
        }
    }
}
