namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Attribute_Name
    {
        public static GameEntity_Attribute_Name DEFAULT = new GameEntity_Attribute_Name("");

        public readonly string NAME;

        internal GameEntity_Attribute_Name(string name)
        {
            NAME = name;
        }

        public override string ToString()
            => NAME;

        public static implicit operator string(GameEntity_Attribute_Name gameEntity_AttributeName)
            => gameEntity_AttributeName.NAME ?? DEFAULT;
    }
}
