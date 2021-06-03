﻿using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_Punch : GameEntity_Ability
    {
        public Ability_Punch() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_STRENGTH,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Physical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return 0;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_Punch();
        }
    }
}
