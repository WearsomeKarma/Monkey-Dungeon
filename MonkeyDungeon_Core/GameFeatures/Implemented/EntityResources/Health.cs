using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Core.GameFeatures.Implemented.StatusEffects;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources
{
    public class Health : GameEntity_Resource
    {
        public Health(double baseValue, double max, double min, double replenishRate, double progressionRate)
            : base(MD_VANILLA_RESOURCES.RESOURCE_HEALTH, baseValue, max, min, replenishRate, progressionRate)
        {
        }
        
        protected override void Handle_Depleted()
        {
            Entity.StatusEffect_Manager.Add_StatusEffect(new StatusEffect_Dead());
        }

        protected override void Handle_Add_To_Entity(GameEntity_Resource_Manager resource_Manager)
        {
            resource_Manager.Add_Resource<Health>(this);
        }

        public override GameEntity_Resource Clone()
        {
            return new Health(Get_BaseValue(), Max_Value, Min_Value, Rate_Replenish, Max_Value.Scaling_Rate);
        }
    }
}