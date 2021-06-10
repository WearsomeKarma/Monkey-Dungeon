namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions
{
    public static class MD_VANILLA_RACES
    {
        public static readonly GameEntity_Attribute_Name_Race RACE_MONKEY = new GameEntity_Attribute_Name_Race("Monkey");
        public static readonly GameEntity_Attribute_Name_Race RACE_GOBLIN = new GameEntity_Attribute_Name_Race("Goblin");

        public static readonly GameEntity_Attribute_Name_Race[] STRINGS = new GameEntity_Attribute_Name_Race[]
        {
            RACE_MONKEY,
            RACE_GOBLIN
        };

        //TODO: refactor for classes seperation from races.
        public static readonly GameEntity_Attribute_Name_Race CLASS_WARRIOR = new GameEntity_Attribute_Name_Race("Warrior");
        public static readonly GameEntity_Attribute_Name_Race CLASS_WIZARD = new GameEntity_Attribute_Name_Race("Wizard");
        public static readonly GameEntity_Attribute_Name_Race CLASS_ARCHER = new GameEntity_Attribute_Name_Race("Archer");
        public static readonly GameEntity_Attribute_Name_Race CLASS_CLERIC = new GameEntity_Attribute_Name_Race("Cleric");
        public static readonly GameEntity_Attribute_Name_Race CLASS_KNIGHT = new GameEntity_Attribute_Name_Race("Knight");
        public static readonly GameEntity_Attribute_Name_Race CLASS_MONK = new GameEntity_Attribute_Name_Race("Monk");

        public static readonly GameEntity_Attribute_Name_Race[] CLASSES = new GameEntity_Attribute_Name_Race[] 
        {
            CLASS_WARRIOR,
            CLASS_WIZARD,
            CLASS_ARCHER,
            CLASS_CLERIC,
            CLASS_KNIGHT,
            CLASS_MONK
        };
    }
}
