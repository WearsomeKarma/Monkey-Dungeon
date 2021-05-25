using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Multiplayer.MessageWrappers
{
    public class MMW_Set_Ranged_Particle : Multiplayer_Message_Wrapper
    {
        public MMW_Set_Ranged_Particle(int shooterId, int targetId, string particleType) 
            : base(MD_VANILLA_MMH.MMH_SET_RANGED_PARTICLE, shooterId, 0, targetId, particleType)
        {
        }
    }
}
