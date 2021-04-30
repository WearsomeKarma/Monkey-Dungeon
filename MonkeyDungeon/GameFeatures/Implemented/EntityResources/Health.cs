using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using MonkeyDungeon.GameFeatures.Implemented.StatusEffects;

namespace MonkeyDungeon.GameFeatures.Implemented.EntityResources
{
    public class Health : EntityResource
    {
        public Health(float baseValue, float max, float min, float replenishRate, float progressionRate)
            : base(ENTITY_RESOURCES.HEALTH, baseValue, max, min, replenishRate, progressionRate)
        {
        }
        
        protected override void Handle_Depleted()
        {
            Entity.Add_StatusEffect(new StatusEffect_Dead());
        }
    }
}