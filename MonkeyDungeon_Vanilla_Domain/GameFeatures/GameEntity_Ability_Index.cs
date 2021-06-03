namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Ability_Index
    {
        public static readonly GameEntity_Ability_Index INDEX_ZERO = new GameEntity_Ability_Index(0);
        public static readonly GameEntity_Ability_Index INDEX_ONE = new GameEntity_Ability_Index(1);
        public static readonly GameEntity_Ability_Index INDEX_TWO = new GameEntity_Ability_Index(2);

        public static readonly GameEntity_Ability_Index[] INDICES = new GameEntity_Ability_Index[] 
        {
            INDEX_ZERO,
            INDEX_ONE,
            INDEX_TWO
        };

        public readonly int INDEX;

        internal GameEntity_Ability_Index(int index)
        {
            INDEX = index;
        }

        public static implicit operator int(GameEntity_Ability_Index index)
            => index.INDEX;
    }
}
