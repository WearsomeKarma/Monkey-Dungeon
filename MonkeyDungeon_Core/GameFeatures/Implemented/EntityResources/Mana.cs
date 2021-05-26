using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources
{
    public class Mana : GameEntity_Resource
    {
        public Mana(double baseValue, double max, double replenishRate, double progressionRate) 
            : base(MD_VANILLA_RESOURCES.RESOURCE_MANA, baseValue, max, 0, replenishRate, progressionRate)
        {
        }

        protected override void Handle_Add_To_Entity(GameEntity_Resource_Manager resource_Manager)
        {
            resource_Manager.Add_Resource<Mana>(this);
        }

        public override GameEntity_Resource Clone()
        {
            return new Mana(Get_BaseValue(), Max_Value, Rate_Replenish, Max_Value.Scaling_Rate);
        }
    }
}
