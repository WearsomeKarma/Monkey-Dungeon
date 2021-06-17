using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability : GameEntity_Attribute
    {
        internal GameEntity_ID              Ability_Owner__GameEntity_ID         => Internal_Parent.GameEntity_ID;

        public GameEntity_Attribute_Name    Ability__Particle_Name               { get; protected set; }
        
        public int                          Ability__Point_Cost                  => Handle_Get__Point_Cost__Ability();
        
        public GameEntity_Attribute_Name    Ability__Affecting_Resource          { get; protected set; }
        public GameEntity_Attribute_Name    Ability__Primary_Resource_Name       { get; private set; }
        public double                       Ability__Resource_Cost               => Handle_Get__Resource_Cost__Ability();
        public double?                      Ability__Primary_Resource_Value      => Internal_Parent?
                                                                                    .Get__Resource__GameEntity<GameEntity_Resource>
                                                                                        (Ability__Primary_Resource_Name)
                                                                                    ?.Value;
        
        internal GameEntity_Attribute_Name  Ability__Primary_Stat_Name            { get; private set; }
        public double?                      Ability__Primary_Stat_Value          => Internal_Parent?
                                                                                    .Get__Stat__GameEntity<GameEntity_Stat>
                                                                                        (
                                                                                        Ability__Primary_Stat_Name 
                                                                                        ?? 
                                                                                        GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME
                                                                                        )
                                                                                    ?.Value;

        
        public Combat_Target_Type           Ability__Combat_Target_Type                 { get; protected set; }
        public Combat_Damage_Type           Ability__Combat_Damage_Type                 { get; protected set; }
        public Combat_Assault_Type          Ability__Combat_Assault_Type                { get; protected set; }
        
        public bool                         Ability__Combat_Enforces_Strict_Targetting  { get; protected set; }

        
        public GameEntity_Ability
            (
            GameEntity_Attribute_Name name,
            GameEntity_Attribute_Name abilityPrimaryResourceName,
            GameEntity_Attribute_Name abilityPrimaryStatName,
            GameEntity_Attribute_Name abilityAffectingResource = null,
            Combat_Target_Type abilityCombatTargetType = Combat_Target_Type.Self_Or_No_Target,
            Combat_Damage_Type abilityCombatDamageType = Combat_Damage_Type.Abstract,
            Combat_Assault_Type abilityCombatAssaultType = Combat_Assault_Type.None,
            GameEntity_Attribute_Name abilityParticleName = null
            )
            : base(name)
        {
            Ability__Primary_Resource_Name = abilityPrimaryResourceName;
            Ability__Primary_Stat_Name = abilityPrimaryStatName;

            Ability__Affecting_Resource = abilityAffectingResource ?? GameEntity_Attribute_Name.NULL_ATTRIBUTE_NAME;
            Ability__Combat_Target_Type = abilityCombatTargetType;
            Ability__Combat_Damage_Type = abilityCombatDamageType;
            Ability__Combat_Assault_Type = abilityCombatAssaultType;

            Ability__Particle_Name = abilityParticleName;

            Ability__Combat_Enforces_Strict_Targetting = true;
        }

        
        
        internal void Cast__Ability(Combat_Action action)
            => Handle_Cast__Ability(action);
        protected virtual void Handle_Cast__Ability(Combat_Action action) { }

        
        
        internal Combat_Redirection_Chance Calculate_Redirect_Chance__Ability
            (
            Combat_Action action,
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => Handle_Ability_Calculate_Redirect_Chance(action, ownerPositionType, targetPositionType, baseChance);
        
        protected virtual Combat_Redirection_Chance Handle_Ability_Calculate_Redirect_Chance
        (
            Combat_Action action,
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => MD_VANILLA_COMBAT.NO_REDIRECT;
        
        
        
        internal Combat_Resource_Offset Calculate_Damage__Ability(Combat_Action action)
            => Handle_Calculate_Damage__Ability(action);
        
        protected virtual Combat_Resource_Offset Handle_Calculate_Damage__Ability (Combat_Action action)
            => new Combat_Resource_Offset(Ability__Combat_Damage_Type, Handle_Get__Nullable_Output__Ability() ?? 0);

        
        
        protected virtual double? Handle_Get__Nullable_Output__Ability()
            => Ability__Primary_Stat_Value;
        protected virtual double Handle_Get__Quantified_Output__Ability()
            => Ability__Primary_Stat_Value ?? 0;
        
        
        
        protected virtual double Handle_Get__Resource_Cost__Ability()
            => 1;
        
        
        
        protected virtual int Handle_Get__Point_Cost__Ability()
            => 1;

        
        
        public virtual GameEntity_Ability Clone__Ability()
        {
            GameEntity_Ability clone = new GameEntity_Ability(
                Attribute_Name,
                Ability__Primary_Resource_Name,
                Ability__Primary_Stat_Name
                );
            return clone;
        }

        
        
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
