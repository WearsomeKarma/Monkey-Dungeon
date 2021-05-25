using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Announcement : Multiplayer_Message_Wrapper
    {
        public MMW_Announcement(string announcementMessage) 
            : base(MD_VANILLA_MMH.MMH_ANNOUNCEMENT, 0, 0, 0, announcementMessage)
        {
        }
    }
}
