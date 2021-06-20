using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public abstract class GameEntity_Ability<T> : GameEntity_Attribute<T> where T : GameEntity
    {
        public GameEntity_ID                Ability__Invoker__GameEntity_ID         => Attached_Entity.GameEntity__ID;

        public GameEntity_Attribute_Name    Ability__Particle_Name               { get; protected set; }
        
        public int                          Ability__Point_Cost                  => Handle_Get__Point_Cost__Ability();
        
        public GameEntity_Attribute_Name    Ability__Targeted_Resource          { get; protected set; }
        public GameEntity_Attribute_Name    Ability__Taxed_Resource_Name       { get; private set; }
        public double                       Ability__Taxed_Resource_Cost               => Handle_Get__Resource_Cost__Ability();
        public abstract double?             Get__Taxed_Resource_Cost__Ability();
        
        public GameEntity_Attribute_Name    Ability__Primary_Stat_Name            { get; private set; }
        public abstract double?             Get__Primary_Stat_Value__Ability();

        public virtual GameEntity_Attribute_Name Get__Dodging_Stat__Ability()
            => Ability__Primary_Stat_Name;

        public Combat_Target_Type           Ability__Combat_Target_Type                 { get; protected set; }
        public GameEntity_Damage_Type       Ability__Damage_Type                        { get; protected set; }
        public Combat_Assault_Type          Ability__Combat_Assault_Type                { get; protected set; }
        
        public bool                         Ability__Combat_Enforces_Strict_Targetting  { get; protected set; }

        
        public GameEntity_Ability
            (
            GameEntity_Attribute_Name name,
            GameEntity_Attribute_Name abilityTaxedResourceName,
            GameEntity_Attribute_Name abilityPrimaryStatName,
            GameEntity_Attribute_Name abilityTargetedResource = null,
            Combat_Target_Type abilityCombatTargetType = Combat_Target_Type.Self_Or_No_Target,
            GameEntity_Damage_Type abilityDamageType = GameEntity_Damage_Type.Abstract,
            Combat_Assault_Type abilityCombatAssaultType = Combat_Assault_Type.None,
            GameEntity_Attribute_Name abilityParticleName = null
            )
            : base(name)
        {
            Ability__Taxed_Resource_Name = abilityTaxedResourceName;
            Ability__Primary_Stat_Name = abilityPrimaryStatName;

            Ability__Targeted_Resource = abilityTargetedResource ?? GameEntity_Attribute_Name.NULL__ATTRIBUTE_NAME;
            Ability__Combat_Target_Type = abilityCombatTargetType;
            Ability__Damage_Type = abilityDamageType;
            Ability__Combat_Assault_Type = abilityCombatAssaultType;

            Ability__Particle_Name = abilityParticleName;

            Ability__Combat_Enforces_Strict_Targetting = true;
        }
        
        protected virtual void Handle__Cast__Ability() { }

        
        
        public Combat_Redirection_Chance Calculate__Redirect_Chance__Ability
            (
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => Handle__Ability_Calculate_Redirect_Chance__Ability(ownerPositionType, targetPositionType, baseChance);
        
        protected virtual Combat_Redirection_Chance Handle__Ability_Calculate_Redirect_Chance__Ability
        (
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => Combat_Redirection_Chance.NULL_REDIRECT;
        
        
        
        public GameEntity_Damage<T> Calculate__Damage__Ability()
            => Handle__Calculate_Damage__Ability();
        
        protected virtual GameEntity_Damage<T> Handle__Calculate_Damage__Ability ()
            => new GameEntity_Damage<T>(Ability__Damage_Type, Handle_Get__Nullable_Output__Ability() ?? 0);

        
        
        protected virtual double? Handle_Get__Nullable_Output__Ability()
            => Get__Primary_Stat_Value__Ability();
        protected virtual double Handle_Get__Quantified_Output__Ability()
            => Get__Primary_Stat_Value__Ability() ?? 0;
        
        
        
        protected virtual double Handle_Get__Resource_Cost__Ability()
            => 1;
        
        
        
        protected virtual int Handle_Get__Point_Cost__Ability()
            => 1;

        
        
        public override string ToString()
        {
            return string.Format(
                "Name: {0} \tOutput: {1}",
                Attribute_Name,
                Handle_Get__Nullable_Output__Ability()
                );
        }
    }
}
