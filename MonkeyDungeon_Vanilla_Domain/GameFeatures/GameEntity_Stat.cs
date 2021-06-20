using System;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Stat<T> : GameEntity_Quantity<T> where T : GameEntity
    {        
        public GameEntity_Stat
            (
            GameEntity_Attribute_Name statName, 
            double? initalValue = null,
            double minQuantity = Double.MinValue, 
            double maxQuantity = Double.MaxValue
            )
            : base(statName, initalValue, minQuantity, maxQuantity)
        {
            
        }

        public virtual GameEntity_Stat<T> Clone__Stat()
        {
            return new GameEntity_Stat<T>
                (
                Attribute_Name, 
                Quantity__Minimal_Value, 
                Quantity__Maximal_Value,
                Quantity__Value
                );
        }
    }
}
