using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats
{
    public class Agility : GameEntity_Stat
    {
        public Agility(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STATS.STAT_AGILITY, baseValue, maxProgresionRate)
        {
        }
    }
}
