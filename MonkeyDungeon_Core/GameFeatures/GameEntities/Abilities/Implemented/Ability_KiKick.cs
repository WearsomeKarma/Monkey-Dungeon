using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_KiKick : GameEntity_Ability
    {
        public Ability_KiKick() 
            : base(
                  MD_VANILLA_ABILITY_NAMES.ABILITY_KI_KICK, 
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                  MD_VANILLA_STAT_NAMES.STAT_AGILITY,
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Physical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage__Ability(Combat_Action action)
        {
            return new Combat_Resource_Offset(Combat_Damage_Type.Physical, Handle_Get__Quantified_Output__Ability() * 1.25);
        }

        protected override double Handle_Get__Resource_Cost__Ability()
        {
            return 1.5;
        }
        
        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_KiKick();
        }
    }
}
