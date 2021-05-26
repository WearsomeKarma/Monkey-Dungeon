﻿using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats
{
    public class Smartypants : GameEntity_Stat
    {
        public Smartypants(float baseValue, float maxProgresionRate) 
            : base(MD_VANILLA_STATS.STAT_SMARTYPANTS, baseValue, maxProgresionRate)
        {
        }
    }
}
