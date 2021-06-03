
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Stats
{
    public class GameEntity_Stat : GameEntity_Quantity
    {        
        public GameEntity_Stat(GameEntity_Attribute_Name statName, double minQuantity, double maxQuantity, double? initalValue = null)
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
    }
}
