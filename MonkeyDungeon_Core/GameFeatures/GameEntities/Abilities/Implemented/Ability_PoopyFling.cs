using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_PoopyFling : Ability_Ranged
    {
        public Ability_PoopyFling() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_POOPY_FLING, 
                  MD_VANILLA_RESOURCES.RESOURCE_HEALTH, 
                  MD_VANILLA_STATS.STAT_STINKINESS, 
                  MD_VANILLA_PARTICLES.POOPY_FLING, 
                  Combat_Damage_Type.Poison, true
                  )
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return Resource_Value * 0.05f;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_PoopyFling();
        }
    }
}
