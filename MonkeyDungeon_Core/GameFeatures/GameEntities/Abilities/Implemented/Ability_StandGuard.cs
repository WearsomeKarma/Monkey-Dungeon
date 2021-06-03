using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
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
