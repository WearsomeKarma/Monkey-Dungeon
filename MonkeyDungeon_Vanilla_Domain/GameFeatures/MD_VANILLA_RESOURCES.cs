using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public static class MD_VANILLA_RESOURCES
    {
        public static readonly string RESOURCE_LEVEL = "Level";
        public static readonly string RESOURCE_ABILITYPOINTS = "Ability Points";

        public static readonly string RESOURCE_HEALTH = "Health";
        public static readonly string RESOURCE_MANA = "Mana";
        public static readonly string RESOURCE_STAMINA = "Stamina";

        public static readonly string[] STRINGS = new string[] 
        {
            RESOURCE_LEVEL,
            RESOURCE_ABILITYPOINTS,

            RESOURCE_HEALTH,
            RESOURCE_MANA,
            RESOURCE_STAMINA
        };
    }
}
