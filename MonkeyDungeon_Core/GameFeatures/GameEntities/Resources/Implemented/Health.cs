using MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Health : GameEntity_Resource
    {
        public Health(double min, double max, double? initalValue = null)
            : base(MD_VANILLA_RESOURCES.RESOURCE_HEALTH, min, max, initalValue)
        {
        }
        
        protected override void Handle_Quantity_Depleted()
        {
            Internal_Parent.StatusEffect_Manager.Add_StatusEffect(new StatusEffect_Dead());
        }

        public override GameEntity_Resource Clone()
        {
            return new Health(Min_Quantity, Max_Quantity, Value);
        }
    }
}