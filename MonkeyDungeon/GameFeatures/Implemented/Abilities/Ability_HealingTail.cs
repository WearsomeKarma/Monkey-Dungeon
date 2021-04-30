using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_HealingTail : Ability
    {
        public static readonly string NAME_HEALING_TAIL = "Healing Tail";

        public Ability_HealingTail() 
            : base(NAME_HEALING_TAIL, ENTITY_RESOURCES.MANA, ENTITY_STATS.SMARTYPANTS, DamageType.Abstract, true)
        {
        }

        protected override void Handle_AbilityUsage(CombatAction action)
        {
            throw new NotImplementedException();
        }
    }
}
