using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented
{
    public class Ability_HealingTail : GameEntity_ServerSide_Ability
    {
        public Ability_HealingTail() 
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_HEALING_TAIL, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 
                MD_VANILLA_STAT_NAMES.STAT_SMARTYPANTS,
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH,
                Combat_Target_Type.One_Enemy,
                GameEntity_Damage_Type.Magical,
                Combat_Assault_Type.Melee
                )
        {
        }

        protected override GameEntity_Damage<GameEntity_ServerSide> Handle__Calculate_Damage__Ability()
        {
            double damage = Handle_Get__Quantified_Output__Ability() * 0.5;
            double heal = damage * 0.15;

            Attached_Entity.Offset__Resource__GameEntity<GameEntity_ServerSide_Resource>(MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH, heal);

            return new GameEntity_Damage<GameEntity_ServerSide>(GameEntity_Damage_Type.Magical, damage);
        }

        public override GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new Ability_HealingTail();
        }
    }
}
