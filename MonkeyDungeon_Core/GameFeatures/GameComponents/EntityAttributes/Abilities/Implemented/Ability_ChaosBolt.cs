using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities.Implemented
{
    public class Ability_ChaosBolt : GameEntity_ServerSide_Ability
    {
        private Random rand = new Random();

        public Ability_ChaosBolt() 
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_CHAOS_BOLT, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA, 
                MD_VANILLA_STAT_NAMES.STAT_SMARTYPANTS,
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH,
                Combat_Target_Type.One_Enemy,
                GameEntity_Damage_Type.Magical,
                Combat_Assault_Type.Ranged,
                MD_VANILLA_PARTICLE_NAMES.CHAOS_BOLT
                )
        {
        }

        protected override Combat_Redirection_Chance Handle__Ability_Calculate_Redirect_Chance__Ability(
            GameEntity_Position_Type ownerPositionType, GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance)
        {
            return new Combat_Redirection_Chance((GameEntity_Position_Swap_Type)(rand.Next(2)+ 1), 0.25);
        }

        protected override GameEntity_Damage<GameEntity_ServerSide> Handle__Calculate_Damage__Ability()
        {
            return new GameEntity_Damage<GameEntity_ServerSide>(GameEntity_Damage_Type.Magical, Handle_Get__Quantified_Output__Ability() * 1.25);
        }

        public override GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new Ability_ChaosBolt();
        }
    }
}
