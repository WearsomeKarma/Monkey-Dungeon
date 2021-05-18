using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_Punch : MeleeAbility
    {
        public static readonly string NAME_PUNCH = "Punch";

        public Ability_Punch() 
            : base(NAME_PUNCH, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH, DamageType.Physical, true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action action)
        {
            base.Handle_AbilityUsage(action);
            action.Target.Damage_This(
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
