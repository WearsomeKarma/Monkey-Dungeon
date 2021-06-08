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
    public class MMW_Update_Entity_Ability : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Entity_Ability(GameEntity_ID entityId, GameEntity_Ability_Index abilityIndex, GameEntity_Attribute_Name abilityName) 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ENTITY_ABILITY, entityId, 0, abilityIndex, abilityName)
        {
        }
    }
}
