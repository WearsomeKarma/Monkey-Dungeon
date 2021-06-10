using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
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
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Magical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage(Combat_Action action)
        {
            double damage = Get_RelevantOutput() * 0.5;
            double heal = damage * 0.15;

            Parent_EntityServerSide.Resource_Manager.Get_Resource(MD_VANILLA_RESOURCES.RESOURCE_HEALTH).Offset_Value(heal);

            return new Combat_Resource_Offset(Combat_Damage_Type.Magical, damage);
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_HealingTail();
        }
    }
}
