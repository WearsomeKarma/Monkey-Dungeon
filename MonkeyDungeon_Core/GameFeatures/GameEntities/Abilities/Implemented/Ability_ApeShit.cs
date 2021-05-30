using MonkeyDungeon_Vanilla_Domain.GameFeatures;

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
            float cost = 8 - ((Entity.Level > 12) ? 4 : Entity.Level / 3);
            return cost;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ApeShit();
        }
    }
}
