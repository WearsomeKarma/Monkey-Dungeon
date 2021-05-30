using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_BanannaRang : Ability_Ranged
    {
        Random rand = new Random();

        public Ability_BanannaRang() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_BANNANA_RANG, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_AGILITY, 
                  MD_VANILLA_PARTICLES.BANNANA_RANG, 
                  Combat_Damage_Type.Physical, 
                  true
                  )
        {
        }

        protected override double Get_AbilityResourceCost()
        {
            float cost = 6 - ((Entity.Level > 12) ? 3 : Entity.Level / 4);
            return cost;
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_BanannaRang();
        }
    }
}
