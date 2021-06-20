using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide_Quantity : GameEntity_Quantity<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_Quantity
            (
            GameEntity_Attribute_Name attributeName = null,
            double? value = null, 
            double min = Double.MinValue, 
            double max = Double.MaxValue
            ) 
            : base
                (
                attributeName ?? GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, 
                value,
                min, 
                max
                )
        {
        }

        public static GameEntity_ServerSide_Quantity Get__Generic_At_Zero__ServerSide_Quantity()
            => new GameEntity_ServerSide_Quantity(GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, 0);
    }
}