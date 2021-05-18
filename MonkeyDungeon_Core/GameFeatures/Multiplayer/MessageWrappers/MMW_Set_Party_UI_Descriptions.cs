using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Set_Party_UI_Descriptions : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Party_UI_Descriptions(int isPlayers, params string[] races) 
            : base(MD_VANILLA_MMH.MMH_SET_PARTY_UI_DESCRIPTIONS, 0, 0, isPlayers, String.Join(" ", races))
        {
        }
    }
}
