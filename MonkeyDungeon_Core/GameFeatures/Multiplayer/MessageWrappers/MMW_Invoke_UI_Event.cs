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
        public MMW_Invoke_UI_Event(string ui_event_tag) 
            : base(MD_VANILLA_MMH.MMH_INVOKE_UI_EVENT, 0, 0, 0, ui_event_tag)
        {
        }
    }
}
