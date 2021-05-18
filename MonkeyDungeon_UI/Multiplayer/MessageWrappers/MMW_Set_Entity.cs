using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Set_Entity : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Entity(int sceneId, string factoryTag) 
            : base(MD_VANILLA_MMH.MMH_SET_ENTITY, sceneId, 0, 0, factoryTag)
        {
        }
    }
}
