using MonkeyDungeon.GameFeatures.CombatObjects;
using MonkeyDungeon.GameFeatures.EntityResourceManagement;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_HealingTail : GameEntity_Ability
    {
        public static readonly string NAME_HEALING_TAIL = "Healing Tail";

        public Ability_HealingTail() 
            : base(NAME_HEALING_TAIL, ENTITY_RESOURCES.MANA, ENTITY_STATS.SMARTYPANTS, DamageType.Abstract, true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action action)
        {
            throw new NotImplementedException();
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_HealingTail();
        }
    }
}
