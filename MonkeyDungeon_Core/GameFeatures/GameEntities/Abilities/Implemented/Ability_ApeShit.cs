using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_ApeShit : GameEntity_Ability
    {
        public Ability_ApeShit() 
            : base(MD_VANILLA_ABILITYNAMES.ABILITY_APE_SHIT, MD_VANILLA_RESOURCES.RESOURCE_STAMINA, MD_VANILLA_STATS.STAT_STRENGTH)
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return 0;
            float cost = 8 - ((Internal_Parent.Level > 12) ? 4 : Internal_Parent.Level / 3);
            return cost;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ApeShit();
        }
    }
}
