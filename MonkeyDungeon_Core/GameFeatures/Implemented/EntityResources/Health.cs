using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Core.GameFeatures.Implemented.StatusEffects;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources
{
    public class Health : GameEntity_Resource
    {
        public Health(float baseValue, float max, float min, float replenishRate, float progressionRate)
            : base(ENTITY_RESOURCES.HEALTH, baseValue, max, min, replenishRate, progressionRate)
        {
        }
        
        protected override void Handle_Depleted()
        {
            Entity.StatusEffect_Manager.Add_StatusEffect(new StatusEffect_Dead());
        }
    }
}