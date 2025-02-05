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
    public class MMW_Set_Traveling_State : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Traveling_State(bool state) 
            : base(MD_VANILLA_MMH.MMH_SET_TRAVELING_STATE, GameEntity_ID.ID_ZERO, 0, state ? 1 : 0)
        {
        }
    }
}
