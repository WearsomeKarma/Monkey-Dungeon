using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities.Implemented
{
    public class Ability_BanannaRang : GameEntity_Ability
    {
        Random rand = new Random();

        public Ability_BanannaRang() 
            : base
                (
                MD_VANILLA_ABILITY_NAMES.ABILITY_BANNANA_RANG, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA, 
                MD_VANILLA_STAT_NAMES.STAT_AGILITY, 
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH,
                Combat_Target_Type.One_Enemy,
                Combat_Damage_Type.Physical,
                Combat_Assault_Type.Ranged,
                MD_VANILLA_PARTICLE_NAMES.BANNANA_RANG
                )
        {
        }

        protected override double Handle_Get__Resource_Cost__Ability()
        {
            float cost = 6 - ((Internal_Parent.Level > 12) ? 3 : Internal_Parent.Level / 4);
            return cost;
        }

        public override GameEntity_Ability Clone__Ability()
        {
            return new Ability_BanannaRang();
        }
    }
}
