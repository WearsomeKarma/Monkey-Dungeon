using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_ChaosBolt : Ability
    {
        public static readonly string NAME_CHAOS_BOLT = "Chaos Bolt";

        public Ability_ChaosBolt() 
            : base(NAME_CHAOS_BOLT, ENTITY_RESOURCES.MANA, ENTITY_STATS.SMARTYPANTS, DamageType.Magical, true)
        {
        }
    }
}
