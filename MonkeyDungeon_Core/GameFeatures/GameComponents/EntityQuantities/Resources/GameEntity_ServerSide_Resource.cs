using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources
{
    public class GameEntity_ServerSide_Resource : GameEntity_Resource<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide Attached_Entity => base.Attached_Entity;
    
        public GameEntity_ServerSide_Resource
            (
            GameEntity_Attribute_Name resourceName,
            double? initalValue = null,
            double min = Double.MinValue, 
            double max = Double.MaxValue
            ) 
            : base
                (
                resourceName, 
                initalValue,
                min, 
                max
                )
        {
        }

        internal void Attach_To__Entity__ServerSide_Resource(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_Resource()
            => Detach_From__Entity__Attribute();
    }
}