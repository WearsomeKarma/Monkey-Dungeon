using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_HealingTail : GameEntity_Ability
    {
        public Ability_HealingTail() 
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_HEALING_TAIL, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 
                MD_VANILLA_STAT_NAMES.STAT_SMARTYPANTS,
                Combat_Target_Type.One_Enemy,
                Combat_Damage_Type.Magical,
                Combat_Assault_Type.Melee
                )
        {
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage__Ability(Combat_Action action)
        {
            double damage = Handle_Get__Quantified_Output__Ability() * 0.5;
            double heal = damage * 0.15;

            Parent_EntityServerSide.Offset__Resource__GameEntity<GameEntity_Resource>(MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH, heal);

            return new Combat_Resource_Offset(Combat_Damage_Type.Magical, damage);
        }

        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_HealingTail();
        }
    }
}
