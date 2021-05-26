using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public static class MD_VANILLA_STATS
    {
        public static readonly string STAT_STRENGTH     = "Strength";
        public static readonly string STAT_AGILITY      = "Agility";
        public static readonly string STAT_SMARTYPANTS  = "Smartypants";
        public static readonly string STAT_STINKINESS   = "Stinkiness";

        public static readonly string[] STRINGS = new string[] 
        {
            STAT_STRENGTH,
            STAT_AGILITY,
            STAT_SMARTYPANTS,
            STAT_STINKINESS
        };
    }
}
