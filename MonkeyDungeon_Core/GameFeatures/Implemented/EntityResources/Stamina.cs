using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources
{
    public class Stamina : GameEntity_Resource
    {
        public Stamina(double baseValue, double max, double replenishRate, double progressionRate) 
            : base(ENTITY_RESOURCES.STAMINA, baseValue, max, 0, replenishRate, progressionRate)
        {
        }

        protected override void Handle_Add_To_Entity(GameEntity_Resource_Manager resource_Manager)
        {
            resource_Manager.Add_Resource<Stamina>(this);
        }

        public override GameEntity_Resource Clone()
        {
            return new Stamina(Get_BaseValue(), Max_Value, Rate_Replenish, Max_Value.Scaling_Rate);
        }
    }
}
