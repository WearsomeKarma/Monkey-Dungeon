﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.CharacterStats
{
    public class Stinkiness : EntityStat
    {
        public Stinkiness(float baseValue, float maxProgresionRate) 
            : base(ENTITY_STATS.STINKINESS, baseValue, maxProgresionRate)
        {
        }
    }
}
