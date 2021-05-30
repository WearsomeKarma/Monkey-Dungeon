using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Invoke_UI_Event : Multiplayer_Message_Wrapper
    {
        public MMW_Invoke_UI_Event(GameEntity_Attribute_Name ui_event_tag) 
            : base(MD_VANILLA_MMH.MMH_INVOKE_UI_EVENT, GameEntity_ID.ID_ZERO, 0, 0, ui_event_tag)
        {
        }
    }
}
