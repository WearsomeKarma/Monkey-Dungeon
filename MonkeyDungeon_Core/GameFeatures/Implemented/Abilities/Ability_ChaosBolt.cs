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
    public class Ability_ChaosBolt : GameEntity_Ability
    {
        public static readonly string NAME_CHAOS_BOLT = "Chaos Bolt";

        public Ability_ChaosBolt() 
            : base(NAME_CHAOS_BOLT, ENTITY_RESOURCES.MANA, ENTITY_STATS.SMARTYPANTS, DamageType.Magical, true)
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ChaosBolt();
        }
    }
}
