using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public static class MD_VANILLA_PARTICLES
    {
        public static readonly GameEntity_Attribute_Name CHAOS_BOLT = new GameEntity_Attribute_Name("Chaos_Bolt");
        public static readonly GameEntity_Attribute_Name POOPY_FLING = new GameEntity_Attribute_Name("Poopy_Fling");
        public static readonly GameEntity_Attribute_Name BANNANA_RANG = new GameEntity_Attribute_Name("Bannana_Rang");

        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[]
        {
            CHAOS_BOLT,
            POOPY_FLING,
            BANNANA_RANG
        };
    }
}
