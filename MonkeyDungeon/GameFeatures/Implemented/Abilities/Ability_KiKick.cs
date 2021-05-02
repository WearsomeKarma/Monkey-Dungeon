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
    public class Ability_KiKick : GameEntity_Ability
    {
        public static readonly string NAME_KI_KICK = "Ki Kick";

        public Ability_KiKick() 
            : base(NAME_KI_KICK, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.AGILITY, DamageType.Physical, true)
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_KiKick();
        }
    }
}
