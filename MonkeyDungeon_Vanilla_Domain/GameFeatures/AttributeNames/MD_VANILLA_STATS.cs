using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public static class MD_VANILLA_STATS
    {
        public static readonly GameEntity_Attribute_Name STAT_STRENGTH     = new GameEntity_Attribute_Name("Strength");
        public static readonly GameEntity_Attribute_Name STAT_AGILITY      = new GameEntity_Attribute_Name("Agility");
        public static readonly GameEntity_Attribute_Name STAT_SMARTYPANTS  = new GameEntity_Attribute_Name("Smartypants");
        public static readonly GameEntity_Attribute_Name STAT_STINKINESS   = new GameEntity_Attribute_Name("Stinkiness");

        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[] 
        {
            STAT_STRENGTH,
            STAT_AGILITY,
            STAT_SMARTYPANTS,
            STAT_STINKINESS
        };
    }
}
