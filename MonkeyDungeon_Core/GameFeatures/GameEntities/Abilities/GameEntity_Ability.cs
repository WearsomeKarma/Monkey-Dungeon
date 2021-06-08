using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability : GameEntity_Attribute
    {
        internal GameEntity_ID Owner_ID => Internal_Parent.GameEntity_ID;

        public GameEntity_Attribute_Name Resource_Name { get; private set; }
        public double Cost => Get_AbilityResourceCost();
        public int Cost_Ability_Points => Get_AbilityPointCost();
        public double Resource_Value => Internal_Parent?.Resource_Manager.Get_Resource(Resource_Name)?.Value ?? throw new Exception("TODO: replace excep");
        public double Resource_ValueStrict => Internal_Parent?.Resource_Manager.Get_Resource(Resource_Name)?.Value ?? throw new Exception("TODO: replace excep");

        internal GameEntity_Attribute_Name Stat_Name { get; private set; }
        public double Stat_Value => Internal_Parent?.Stat_Manager.Get_Stat(Stat_Name ?? GameEntity_Attribute_Name.DEFAULT)?.Value ?? throw new Exception("TODO: replace excep");

        public GameEntity_Attribute_Name Particle_Type { get; protected set; }

        public Combat_Ability_Target Target { get; protected set; }
        public Combat_Target_Type Target_Type { get; protected set; }
        public Combat_Damage_Type Damage_Type { get; protected set; }
        public Combat_Assault_Type Assault_Type { get; protected set; }
        public bool Has_Strict_Targets { get; protected set; }

        public GameEntity_Ability(
            GameEntity_Attribute_Name name,
            GameEntity_Attribute_Name resourceName,
            GameEntity_Attribute_Name statName,
            Combat_Target_Type targetType = Combat_Target_Type.Self_Or_No_Target,
            Combat_Damage_Type damageType = Combat_Damage_Type.Abstract,
            Combat_Assault_Type assaultType = Combat_Assault_Type.None,
            GameEntity_Attribute_Name particleType = null
            )
            : base(name)
        {
            Resource_Name = resourceName;
            Stat_Name = statName;

            Target_Type = targetType;
            Damage_Type = damageType;
            Assault_Type = assaultType;

            Particle_Type = particleType;
        }

        internal void Cast(Combat_Action action)
            => Handle_Cast(action);

        internal Combat_Redirection_Chance Calculate_Redirect_Chance
        (
            Combat_Action action,
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
        )
            => Handle_Calculate_Redirect_Chance(action, ownerPositionType, targetPositionType, baseChance);
        internal Combat_Resource_Offset Calculate_Damage(Combat_Action action)
            => Handle_Calculate_Damage(action);

        protected virtual void Handle_Cast                      (Combat_Action action) { }

        protected virtual Combat_Redirection_Chance Handle_Calculate_Redirect_Chance
            (
            Combat_Action action,
            GameEntity_Position_Type ownerPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => MD_VANILLA_COMBAT.NO_REDIRECT;
        protected virtual Combat_Resource_Offset Handle_Calculate_Damage (Combat_Action action)
            => new Combat_Resource_Offset(Damage_Type, Get_RelevantOutput());

        protected virtual double Get_RelevantOutput()           => Internal_Parent?.Stat_Manager.Get_Stat(Stat_Name)?.Value ?? 0;
        protected virtual double Get_AbilityResourceCost()      => 1;
        protected virtual int Get_AbilityPointCost()            => 1;

        public virtual GameEntity_Ability Clone()
        {
            GameEntity_Ability clone = new GameEntity_Ability(
                ATTRIBUTE_NAME,
                Resource_Name,
                Stat_Name
                );
            return clone;
        }

        public override string ToString()
        {
            return string.Format(
                "Name: {0} \tOutput: {1}",
                ATTRIBUTE_NAME,
                Get_RelevantOutput()
                );
        }
    }
}
