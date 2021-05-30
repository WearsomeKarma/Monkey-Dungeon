using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_ChaosBolt : GameEntity_Ability
    {
        private Random rand = new Random();

        public Ability_ChaosBolt() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_CHAOS_BOLT, 
                  MD_VANILLA_RESOURCES.RESOURCE_MANA, 
                  MD_VANILLA_STATS.STAT_SMARTYPANTS,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Magical,
                  Combat_Assault_Type.Ranged,
                  MD_VANILLA_PARTICLES.CHAOS_BOLT
                  )
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ChaosBolt();
        }
    }
}
