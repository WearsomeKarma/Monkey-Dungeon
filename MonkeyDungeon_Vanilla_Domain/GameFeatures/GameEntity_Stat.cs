namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Stat<T> : GameEntity_Quantity<T> where T : GameEntity
    {        
        public GameEntity_Stat(GameEntity_Attribute_Name statName, double minQuantity, double maxQuantity, double? initalValue = null)
            : base(statName, minQuantity, maxQuantity)
        {
            if(initalValue != null)
                Set__Value__Quantity((double)initalValue);
        }

        public GameEntity_Stat<T> Clone__Stat()
        {
            return new GameEntity_Stat<T>
                (
                Attribute_Name, 
                Min_Quantity, 
                Max_Quantity,
                Value
                );
        }
    }
}
