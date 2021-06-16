﻿using MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects.Implemented;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented
{
    public class Health : GameEntity_Resource
    {
        public Health(double min, double max, double? initalValue = null)
            : base(MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH, min, max, initalValue)
        {
        }
        
        protected override void Handle_Quantity_Depleted()
        {
            Internal_Parent.Add__GameEntity_StatusEffect(new StatusEffect_Dead());
        }

        public override GameEntity_Resource Clone__Resource()
        {
            return new Health(Min_Quantity, Max_Quantity, Value);
        }
    }
}