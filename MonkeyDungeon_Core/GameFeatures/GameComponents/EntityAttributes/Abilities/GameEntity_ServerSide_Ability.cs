using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities
{
    public class GameEntity_ServerSide_Ability : GameEntity_Ability<GameEntity_ServerSide>
    {
        public GameEntity_ServerSide_Ability
            (
            GameEntity_Attribute_Name name, 
            GameEntity_Attribute_Name abilityTaxedResourceName, 
            GameEntity_Attribute_Name abilityPrimaryStatName, 
            GameEntity_Attribute_Name abilityTargetedResource = null, 
            Combat_Target_Type abilityCombatTargetType = Combat_Target_Type.Self_Or_No_Target, 
            GameEntity_Damage_Type abilityDamageType = GameEntity_Damage_Type.Abstract, 
            Combat_Assault_Type abilityCombatAssaultType = Combat_Assault_Type.None, 
            GameEntity_Attribute_Name abilityParticleName = null) 
            : base
                (
                name, 
                abilityTaxedResourceName, 
                abilityPrimaryStatName, 
                abilityTargetedResource, 
                abilityCombatTargetType, 
                abilityDamageType, 
                abilityCombatAssaultType, 
                abilityParticleName
                )
        {
        }

        public override double? Get__Taxed_Resource_Cost__Ability()
        {
            return Attached_Entity?.Get__Resource__GameEntity<GameEntity_ServerSide_Resource>(
                Ability__Taxed_Resource_Name);
        }

        public override double? Get__Primary_Stat_Value__Ability()
        {
            return Attached_Entity?.Get__Stat__GameEntity<GameEntity_ServerSide_Stat>(Ability__Primary_Stat_Name);
        }

        internal void Attach_To__Entity__ServerSide_Ability(GameEntity_ServerSide entity)
            => Attach_To__Entity__Attribute(entity);
        internal void Detach_From__Entity__ServerSide_Ability()
            => Detach_From__Entity__Attribute();

        internal void Cast__ServerSide_Ability()
            => Handle__Cast__Ability();
        
        public virtual GameEntity_ServerSide_Ability Clone__ServerSide_Ability()
        {
            return new GameEntity_ServerSide_Ability
                (
                Attribute_Name,
                Ability__Taxed_Resource_Name,
                Ability__Primary_Stat_Name,
                Ability__Targeted_Resource,
                Ability__Combat_Target_Type,
                Ability__Damage_Type,
                Ability__Combat_Assault_Type,
                Ability__Particle_Name
                );
        }
    }
}