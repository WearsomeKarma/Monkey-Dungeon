using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_PoopyFling : Ability_Ranged
    {
        public Ability_PoopyFling() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_POOPY_FLING, 
                  MD_VANILLA_RESOURCES.RESOURCE_HEALTH, 
                  MD_VANILLA_STATS.STAT_STINKINESS, 
                  MD_VANILLA_PARTICLES.POOPY_FLING, 
                  DamageType.Poison, true
                  )
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action combatAction)
        {
            base.Handle_AbilityUsage(combatAction);

            GameEntity target = Entity.Game.Get_Entity(combatAction.Target_ID);

            target.Damage_This(
                new Combat_Damage(
                    Ability_DamageType,
                    Get_RelevantOutput() * 0.25
                    )
                );
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
