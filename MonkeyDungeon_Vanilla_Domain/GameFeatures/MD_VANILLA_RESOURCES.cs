using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public static class MD_VANILLA_RESOURCES
    {
        public static readonly GameEntity_Attribute_Name RESOURCE_LEVEL = new GameEntity_Attribute_Name("Level");
        public static readonly GameEntity_Attribute_Name RESOURCE_ABILITYPOINTS = new GameEntity_Attribute_Name("Ability Points");

        public static readonly GameEntity_Attribute_Name RESOURCE_HEALTH = new GameEntity_Attribute_Name("Health");
        public static readonly GameEntity_Attribute_Name RESOURCE_MANA = new GameEntity_Attribute_Name("Mana");
        public static readonly GameEntity_Attribute_Name RESOURCE_STAMINA = new GameEntity_Attribute_Name("Stamina");

        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[] 
        {
            RESOURCE_LEVEL,
            RESOURCE_ABILITYPOINTS,

            RESOURCE_HEALTH,
            RESOURCE_MANA,
            RESOURCE_STAMINA
        };
    }
}
