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
                  MD_VANILLA_ABILITYNAMES.ABILITY_KI_KICK, 
                  MD_VANILLA_RESOURCES.RESOURCE_STAMINA, 
                  MD_VANILLA_STATS.STAT_AGILITY,
                  Combat_Target_Type.One_Enemy,
                  Combat_Damage_Type.Physical,
                  Combat_Assault_Type.Melee
                  )
        {
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage(Combat_Action action)
        {
            return new Combat_Resource_Offset(Combat_Damage_Type.Physical, Get_RelevantOutput() * 1.25);
        }

        protected override double Get_AbilityResourceCost()
        {
            return 1.5;
        }
        
        public override GameEntity_Ability Clone()
        {
            return new Ability_KiKick();
        }
    }
}
