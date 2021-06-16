using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_ApeShit : GameEntity_Ability
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
            float cost = 8 - ((Internal_Parent.Level > 12) ? 4 : Internal_Parent.Level / 3);
            return cost;
        }

        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_ApeShit();
        }
    }
}
