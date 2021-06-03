namespace MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames
{
    public static class MD_VANILLA_RACES
    {
        public static readonly GameEntity_Attribute_Name RACE_MONKEY = new GameEntity_Attribute_Name("Monkey");
        public static readonly GameEntity_Attribute_Name RACE_GOBLIN = new GameEntity_Attribute_Name("Goblin");

        public static readonly GameEntity_Attribute_Name[] STRINGS = new GameEntity_Attribute_Name[]
        {
            RACE_MONKEY,
            RACE_GOBLIN
        };

        public static readonly GameEntity_Attribute_Name CLASS_WARRIOR = new GameEntity_Attribute_Name("Warrior");
        public static readonly GameEntity_Attribute_Name CLASS_WIZARD = new GameEntity_Attribute_Name("Wizard");
        public static readonly GameEntity_Attribute_Name CLASS_ARCHER = new GameEntity_Attribute_Name("Archer");
        public static readonly GameEntity_Attribute_Name CLASS_CLERIC = new GameEntity_Attribute_Name("Cleric");
        public static readonly GameEntity_Attribute_Name CLASS_KNIGHT = new GameEntity_Attribute_Name("Knight");
        public static readonly GameEntity_Attribute_Name CLASS_MONK = new GameEntity_Attribute_Name("Monk");

        public static readonly GameEntity_Attribute_Name[] CLASSES = new GameEntity_Attribute_Name[] 
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
