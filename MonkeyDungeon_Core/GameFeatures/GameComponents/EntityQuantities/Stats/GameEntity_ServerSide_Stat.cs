using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats
{
    public class GameEntity_ServerSide_Stat : GameEntity_Stat<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_Stat
            (
            GameEntity_Attribute_Name statName = null, 
            double? initalValue = null,
            double minQuantity = Double.MinValue, 
            double maxQuantity = Double.MaxValue
            ) 
            : base
                (
                statName ?? GameEntity_Attribute_Name.GENERIC__ATTRIBUTE_NAME,
                initalValue, 
                minQuantity, 
                maxQuantity
                )
        {
        }
        
        internal void Attach_To__Entity__ServerSide_Stat(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_Stat()
            => Detach_From__Entity__Attribute();

        public override GameEntity_Stat<GameEntity_ServerSide> Clone__Stat()
        {
            return new GameEntity_ServerSide_Stat
            (
                Attribute_Name, 
                Quantity__Minimal_Value, 
                Quantity__Maximal_Value,
                Quantity__Value
            );
        }
    }
}