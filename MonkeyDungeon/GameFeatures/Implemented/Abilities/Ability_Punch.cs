using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_Punch : Ability
    {
        public static readonly string NAME_PUNCH = "Punch";

        public Ability_Punch() 
            : base(NAME_PUNCH, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH, DamageType.Physical, true)
        {
        }

        protected override void Handle_AbilityUsage(CombatAction action)
        {
            ImplementedHandle_DealDamage(action);
        }

        protected override float Get_AbilityResourceCost()
        {
            return 0;
        }
    }
}
