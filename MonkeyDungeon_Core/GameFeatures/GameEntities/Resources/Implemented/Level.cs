using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Level : GameEntity_Resource
    {
        public Level(double max, double? initalValue=null) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_LEVEL, 1, max, initalValue)
        {
        }

        protected override void Handle_Quantity_Change()
        {
            if (Internal_Parent != null)
            {
                throw new NotImplementedException();
            }
        }
    }
}
