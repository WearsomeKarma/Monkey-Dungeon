﻿using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_PoopyFling : GameEntity_Ability
    {
        public Ability_PoopyFling() 
            : base(
                  MD_VANILLA_ABILITY_NAMES.ABILITY_POOPY_FLING, 
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH, 
                  MD_VANILLA_STAT_NAMES.STAT_STINKINESS,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Poison,
                  Combat_Assault_Type.Ranged,
                  MD_VANILLA_PARTICLE_NAMES.POOPY_FLING
                  )
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return Resource_Value * 0.05f;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_PoopyFling();
        }
    }
}
