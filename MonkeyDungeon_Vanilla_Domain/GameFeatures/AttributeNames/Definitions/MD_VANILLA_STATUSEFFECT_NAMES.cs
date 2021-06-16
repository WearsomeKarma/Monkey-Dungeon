namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions
{
    public static class MD_VANILLA_STATUSEFFECT_NAMES
    {
        public static readonly GameEntity_Attribute_Name STATUSEFFECT_DEAD = new GameEntity_Attribute_Name("Dead");
        public static readonly GameEntity_Attribute_Name STATUSEFFECT_PETRIFIED = new GameEntity_Attribute_Name("Petrified");

        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[]
        {
            STATUSEFFECT_DEAD,
            STATUSEFFECT_PETRIFIED
        };
    }
}