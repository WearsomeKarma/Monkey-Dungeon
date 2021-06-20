using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented
{
    public class Level : GameEntity_ServerSide_Resource
    {
        public Level(double max, double? initalValue=null) 
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_LEVEL, initalValue, 1, max)
        {
        }

        protected override void Handle_Quantity_Change()
        {
            if (Attached_Entity != null)
            {
                throw new NotImplementedException();
            }
        }
    }
}
