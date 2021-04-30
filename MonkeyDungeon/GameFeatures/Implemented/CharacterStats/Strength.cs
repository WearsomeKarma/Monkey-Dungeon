using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.CharacterStats
{
    public class Strength : EntityStat
    {
        public Strength(float baseValue, float maxProgresionRate) 
            : base(ENTITY_STATS.STRENGTH, baseValue, maxProgresionRate)
        {
        }
    }
}
