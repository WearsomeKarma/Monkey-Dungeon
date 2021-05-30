using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_PoopyFling : GameEntity_Ability
    {
        public Ability_PoopyFling() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_POOPY_FLING, 
                  MD_VANILLA_RESOURCES.RESOURCE_HEALTH, 
                  MD_VANILLA_STATS.STAT_STINKINESS,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Poison,
                  Combat_Assault_Type.Ranged,
                  MD_VANILLA_PARTICLES.POOPY_FLING
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
