using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.Implemented.Abilities
{
    public class Ability_StandGuard : Ability
    {
        public static readonly string NAME_STAND_GUARD = "Stand Guard";

        public Ability_StandGuard() 
            : base(NAME_STAND_GUARD, ENTITY_RESOURCES.STAMINA, ENTITY_STATS.STRENGTH)
        {
        }
    }
}
