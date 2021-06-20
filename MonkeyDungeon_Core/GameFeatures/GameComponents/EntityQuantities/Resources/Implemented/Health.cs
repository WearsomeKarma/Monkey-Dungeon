using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented
{
    public class Health : GameEntity_ServerSide_Resource
    {
        public Health(double min, double max, double? initalValue = null)
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH, initalValue, min, max)
        {
        }
        
        protected override void Handle_Quantity_Depleted()
        {
            Attached_Entity.Add__GameEntity_StatusEffect(new StatusEffect_Dead());
        }

        public override GameEntity_Resource<GameEntity_ServerSide> Clone__Resource()
        {
            return new Health(Quantity__Minimal_Value, Quantity__Maximal_Value, Quantity__Value);
        }
    }
}