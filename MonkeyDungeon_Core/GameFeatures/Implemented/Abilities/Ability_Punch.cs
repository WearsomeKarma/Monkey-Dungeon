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
    public class Ability_Punch : Ability_Melee
    {
        public Ability_Punch() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_STRENGTH, 
                  DamageType.Physical, 
                  true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action action)
        {
            base.Handle_AbilityUsage(action);
            GameEntity target = Entity.Game.Get_Entity(action.Target_ID);

            target.Damage_This(
                new Combat_Damage(
                    DamageType.Physical, 
                    Get_RelevantOutput() * 0.25
                    ));
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
