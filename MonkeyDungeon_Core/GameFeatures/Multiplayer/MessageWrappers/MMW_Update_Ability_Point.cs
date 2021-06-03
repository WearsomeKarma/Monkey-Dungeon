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
    public class MMW_Update_Ability_Point : Multiplayer_Message_Wrapper
    {
        public MMW_Update_Ability_Point(GameEntity_ID entityId, int abilityPointCount) 
            : base(MD_VANILLA_MMH.MMH_UPDATE_ABILITY_POINT, entityId, 0, abilityPointCount)
        {
        }
    }
}
