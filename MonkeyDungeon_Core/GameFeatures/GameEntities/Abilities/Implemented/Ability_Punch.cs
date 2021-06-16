using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_Punch : GameEntity_Ability
    {
        public Ability_Punch() 
            : base(
                  MD_VANILLA_ABILITY_NAMES.ABILITY_PUNCH, 
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                  MD_VANILLA_STAT_NAMES.STAT_STRENGTH,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Physical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage__Ability(Combat_Action action)
        {
            return new Combat_Resource_Offset(Combat_Damage_Type.Physical, Handle_Get__Quantified_Output__Ability());
        }

        protected override double Handle_Get__Resource_Cost__Ability()
        {
            return 0;
        }

        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_Punch();
        }
    }
}
