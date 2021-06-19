using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats
{
    public class GameEntity_ServerSide_Stat : GameEntity_Stat<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_Stat
            (
            GameEntity_Attribute_Name statName = null, 
            double minQuantity = Double.MinValue, 
            double maxQuantity = Double.MaxValue, 
            double? initalValue = null
            ) 
            : base
                (
                statName ?? GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME, 
                minQuantity, 
                maxQuantity, 
                initalValue
                )
        {
        }
        
        internal void Attach_To__Entity__ServerSide_Stat(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_Stat()
            => Detach_From__Entity__Attribute();
    }
}