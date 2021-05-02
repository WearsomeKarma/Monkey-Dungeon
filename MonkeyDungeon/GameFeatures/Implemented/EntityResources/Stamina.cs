using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.GameFeatures.EntityResourceManagement;

namespace MonkeyDungeon.GameFeatures.Implemented.EntityResources
{
    public class Stamina : GameEntity_Resource
    {
        public Stamina(float baseValue, float max, float replenishRate, float progressionRate) 
            : base(ENTITY_RESOURCES.STAMINA, baseValue, max, 0, replenishRate, progressionRate)
        {
        }
    }
}
