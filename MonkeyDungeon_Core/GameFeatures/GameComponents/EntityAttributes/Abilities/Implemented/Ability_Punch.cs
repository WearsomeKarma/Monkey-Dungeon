using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented
{
    public class Ability_Punch : GameEntity_ServerSide_Ability
    {
        public Ability_Punch() 
            : base(
                  MD_VANILLA_ABILITY_NAMES.ABILITY_PUNCH, 
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                  MD_VANILLA_STAT_NAMES.STAT_STRENGTH,
                  MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH,
                  Combat_Target_Type.One_Enemy,
                  GameEntity_Damage_Type.Physical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override GameEntity_Damage<GameEntity_ServerSide> Handle__Calculate_Damage__Ability()
        {
            return new GameEntity_Damage<GameEntity_ServerSide>(GameEntity_Damage_Type.Physical, Handle_Get__Quantified_Output__Ability());
        }

        protected override double Handle_Get__Resource_Cost__Ability()
        {
            return 0;
        }

        public override GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new Ability_Punch();
        }
    }
}
