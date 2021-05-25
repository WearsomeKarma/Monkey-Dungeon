using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.Multiplayer
{
    public static class MD_VANILLA_UI
    {
        public static readonly string UI_EVENT_ANNOUNCEMENT = "UI_Event_Announcement";
        public static readonly string UI_EVENT_MELEE = "UI_Event_Melee";
        public static readonly string UI_EVENT_RANGED_ATTACK = "UI_Event_Ranged_Attack";
        
        public static readonly string[] STRINGS = new string[]
        {
            UI_EVENT_ANNOUNCEMENT,
            UI_EVENT_MELEE,
            UI_EVENT_RANGED_ATTACK
        };
    }
}
