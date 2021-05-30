using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats
{
    public class GameEntity_Stat : GameEntity_Quantity
    {
        public event Action<double> Quantity_Changed;
        
        public GameEntity_Stat(string statName, double minQuantity, double maxQuantity, double? initalValue = null)
            : base(statName, minQuantity, maxQuantity)
        {
            if(initalValue != null)
                Set_Value((double)initalValue);
        }

        public GameEntity_Stat Clone()
        {
            return new GameEntity_Stat
                (
                ATTRIBUTE_NAME, 
                Min_Quantity, 
                Max_Quantity,
                Value
                );
        }

        protected override void Handle_Post_Offset_Value(double newValue)
        {
            Quantity_Changed?.Invoke(Value);
        }

        protected override void Handle_Post_Set_Value(double newMax)
        {
            Quantity_Changed?.Invoke(this);
        }
    }
}
