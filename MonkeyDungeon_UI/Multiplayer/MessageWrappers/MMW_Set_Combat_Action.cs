using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_UI.Multiplayer.MessageWrappers
{
    public class MMW_Set_Combat_Action : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Combat_Action(
            int actionTargetId, 
            string abilityName
            ) 
            : base(
                  MD_VANILLA_MMH.MMH_SET_COMBAT_ACTION, 
                  -1, 
                  0, 
                  actionTargetId, 
                  abilityName
                  )
        {
        }
    }
}
