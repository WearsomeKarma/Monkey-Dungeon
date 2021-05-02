using MonkeyDungeon.GameFeatures.EntityResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.EntityResources
{
    public class Mana : GameEntity_Resource
    {
        public Mana(float baseValue, float max, float replenishRate, float progressionRate) 
            : base(ENTITY_RESOURCES.MANA, baseValue, max, 0, replenishRate, progressionRate)
        {
        }
    }
}
