namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions
{
    public static class MD_VANILLA_RESOURCE_NAMES
    {
        public static readonly GameEntity_Attribute_Name_Resource RESOURCE_LEVEL = new GameEntity_Attribute_Name_Resource("Level");
        public static readonly GameEntity_Attribute_Name_Resource RESOURCE_ABILITYPOINTS = new GameEntity_Attribute_Name_Resource("Ability Points");

        public static readonly GameEntity_Attribute_Name_Resource RESOURCE_HEALTH = new GameEntity_Attribute_Name_Resource("Health");
        public static readonly GameEntity_Attribute_Name_Resource RESOURCE_MANA = new GameEntity_Attribute_Name_Resource("Mana");
        public static readonly GameEntity_Attribute_Name_Resource RESOURCE_STAMINA = new GameEntity_Attribute_Name_Resource("Stamina");

        public static readonly GameEntity_Attribute_Name_Resource[] STRINGS = new GameEntity_Attribute_Name_Resource[] 
        {
            RESOURCE_LEVEL,
            RESOURCE_ABILITYPOINTS,

            RESOURCE_HEALTH,
            RESOURCE_MANA,
            RESOURCE_STAMINA
        };
    }
}
