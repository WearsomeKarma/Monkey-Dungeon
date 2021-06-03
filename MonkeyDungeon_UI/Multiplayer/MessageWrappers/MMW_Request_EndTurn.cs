using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Request_EndTurn : Multiplayer_Message_Wrapper
    {
        public MMW_Request_EndTurn() 
            : base(MD_VANILLA_MMH.MMH_REQUEST_ENDTURN, GameEntity_ID.ID_ZERO, 0, 0, GameEntity_Attribute_Name.DEFAULT)
        {
        }
    }
}
