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
    public class Ability_StandGuard : GameEntity_Ability
    {
        public Ability_StandGuard() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_STAND_GUARD, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_STRENGTH
                  )
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_StandGuard();
        }
    }
}
