namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions
{
    public static class MD_VANILLA_STAT_NAMES
    {
        public static readonly GameEntity_Attribute_Name_Stat STAT_STRENGTH     = new GameEntity_Attribute_Name_Stat("Strength");
        public static readonly GameEntity_Attribute_Name_Stat STAT_AGILITY      = new GameEntity_Attribute_Name_Stat("Agility");
        public static readonly GameEntity_Attribute_Name_Stat STAT_SMARTYPANTS  = new GameEntity_Attribute_Name_Stat("Smartypants");
        public static readonly GameEntity_Attribute_Name_Stat STAT_STINKINESS   = new GameEntity_Attribute_Name_Stat("Stinkiness");

        public static readonly GameEntity_Attribute_Name_Stat[] STRINGS = new GameEntity_Attribute_Name_Stat[] 
        {
            STAT_STRENGTH,
            STAT_AGILITY,
            STAT_SMARTYPANTS,
            STAT_STINKINESS
        };
    }
}
