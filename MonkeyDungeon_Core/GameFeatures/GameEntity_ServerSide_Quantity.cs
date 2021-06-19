using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide_Quantity : GameEntity_Quantity<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_Quantity
            (
            GameEntity_Attribute_Name attributeName = null, 
            double min = Double.MinValue, 
            double max = Double.MaxValue, 
            double? value = null
            ) 
            : base
                (
                attributeName ?? GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, 
                min, 
                max, 
                value
                )
        {
        }
    }
}