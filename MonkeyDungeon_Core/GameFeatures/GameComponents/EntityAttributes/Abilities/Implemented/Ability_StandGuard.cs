using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented
{
    public class Ability_StandGuard : GameEntity_ServerSide_Ability
    {
        public Ability_StandGuard() 
            : base(
                  MD_VANILLA_ABILITY_NAMES.ABILITY_STAND_GUARD, 
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                  MD_VANILLA_STAT_NAMES.STAT_STRENGTH
                  )
        {
        }

        public override GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new Ability_StandGuard();
        }
    }
}
