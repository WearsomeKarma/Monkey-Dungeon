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
    public class Ability_HealingTail : GameEntity_Ability
    {
        public Ability_HealingTail() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_HEALING_TAIL, 
                  MD_VANILLA_RESOURCES.RESOURCE_MANA, 
                  MD_VANILLA_STATS.STAT_SMARTYPANTS, 
                  DamageType.Abstract, 
                  true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action action)
        {
            //throw new NotImplementedException();
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_HealingTail();
        }
    }
}
