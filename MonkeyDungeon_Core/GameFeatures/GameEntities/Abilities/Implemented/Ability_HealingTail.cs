using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_HealingTail : GameEntity_Ability
    {
        public Ability_HealingTail() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_HEALING_TAIL, 
                  MD_VANILLA_RESOURCES.RESOURCE_MANA, 
                  MD_VANILLA_STATS.STAT_SMARTYPANTS,
                  Combat_Target_Type.One_Ally
                  )
        {
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_HealingTail();
        }
    }
}
