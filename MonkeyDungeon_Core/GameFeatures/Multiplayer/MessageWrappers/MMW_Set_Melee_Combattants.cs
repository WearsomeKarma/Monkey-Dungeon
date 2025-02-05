﻿using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Set_Melee_Combattants : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Melee_Combattants(GameEntity_ID ally_side_id, GameEntity_ID enemy_side_id) 
            : base(MD_VANILLA_MMH.MMH_SET_MELEE_COMBATTANTS, ally_side_id, 0, enemy_side_id)
        {
        }
    }
}
