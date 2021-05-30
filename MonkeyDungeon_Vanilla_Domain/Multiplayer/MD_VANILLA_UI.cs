﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public static class MD_VANILLA_UI
    {
        public static readonly GameEntity_Attribute_Name UI_EVENT_ANNOUNCEMENT = new GameEntity_Attribute_Name("UI_Event_Announcement");
        public static readonly GameEntity_Attribute_Name UI_EVENT_MELEE = new GameEntity_Attribute_Name("UI_Event_Melee");
        public static readonly GameEntity_Attribute_Name UI_EVENT_RANGED_ATTACK = new GameEntity_Attribute_Name("UI_Event_Ranged_Attack");
        
        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[]
        {
            UI_EVENT_ANNOUNCEMENT,
            UI_EVENT_MELEE,
            UI_EVENT_RANGED_ATTACK
        };
    }
}
