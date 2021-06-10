using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

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

        protected override Combat_Redirection_Chance Handle_Calculate_Redirect_Chance(Combat_Action action,
            GameEntity_Position_Type ownerPositionType, GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance)
        {
            return new Combat_Redirection_Chance((Combat_Redirect_Type)(rand.Next(2)+ 1), 0.25);
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage(Combat_Action action)
        {
            return new Combat_Resource_Offset(Combat_Damage_Type.Magical, Get_RelevantOutput() * 1.25);
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ChaosBolt();
        }
    }
}
