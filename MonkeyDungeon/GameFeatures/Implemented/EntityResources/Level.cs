using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.EntityResources
{
    public class Level : EntityResource
    {
        public Level(float baseValue, float max, float min, float replenishRate, float progressionRate) 
            : base(ENTITY_RESOURCES.LEVEL, baseValue, max, min, replenishRate, progressionRate)
        {
        }

        protected override void HandleLevelChange()
        {
            if (Entity != null)
                foreach (EntityResource resource in Entity.Get_Resources())
                    resource.PerformLevelChange();
        }
    }
}
