using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_Punch : Ability_Melee
    {
        public Ability_Punch() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_PUNCH, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_STRENGTH, 
                  Combat_Damage_Type.Physical, 
                  true)
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            return 0;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_Punch();
        }
    }
}
