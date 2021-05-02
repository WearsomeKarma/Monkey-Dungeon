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
    public class Ability_StandGuard : GameEntity_Ability
    {
        public static readonly string NAME_STAND_GUARD = "Stand Guard";

        public Ability_StandGuard() 
            : base(NAME_STAND_GUARD, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH)
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_StandGuard();
        }
    }
}
