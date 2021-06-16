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
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_CHAOS_BOLT, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 
                MD_VANILLA_STAT_NAMES.STAT_SMARTYPANTS,
                Combat_Target_Type.One_Enemy,
                Combat_Damage_Type.Magical,
                Combat_Assault_Type.Ranged,
                MD_VANILLA_PARTICLE_NAMES.CHAOS_BOLT
                )
        {
        }

        protected override Combat_Redirection_Chance Handle_Ability_Calculate_Redirect_Chance(Combat_Action action,
            GameEntity_Position_Type ownerPositionType, GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance)
        {
            return new Combat_Redirection_Chance((GameEntity_Position_Swap_Type)(rand.Next(2)+ 1), 0.25);
        }

        protected override Combat_Resource_Offset Handle_Calculate_Damage__Ability(Combat_Action action)
        {
            return new Combat_Resource_Offset(Combat_Damage_Type.Magical, Handle_Get__Quantified_Output__Ability() * 1.25);
        }

        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_ChaosBolt();
        }
    }
}
