using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented
{
    public class Ability_ApeShit : GameEntity_ServerSide_Ability
    {
        public Ability_ApeShit() 
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_APE_SHIT, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                MD_VANILLA_STAT_NAMES.STAT_STRENGTH
                )
        {
        }

        protected override double Handle_Get__Resource_Cost__Ability()
        {
            return 0;
            float cost = 8 - ((Attached_Entity.Level > 12) ? 4 : Attached_Entity.Level / 3);
            return cost;
        }

        public override GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new Ability_ApeShit();
        }
    }
}
