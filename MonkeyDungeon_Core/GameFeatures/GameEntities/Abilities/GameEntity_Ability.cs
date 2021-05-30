using MonkeyDungeon_Vanilla_Domain;
using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability : GameEntity_Attribute
    {
        internal int Entity_Scene_Id => Internal_Parent.Scene_GameObject_ID;

        public GameEntity_Attribute_Name Resource_Name { get; private set; }
        public double Cost => Get_AbilityResourceCost();
        public int Cost_Ability_Points => Get_AbilityPointCost();
        public double Resource_Value => Internal_Parent?.Resource_Manager.Get_Resource(Resource_Name)?.Value ?? throw new Exception("TODO: replace excep");
        public double Resource_ValueStrict => Internal_Parent?.Resource_Manager.Get_Resource(Resource_Name)?.Value ?? throw new Exception("TODO: replace excep");

        internal GameEntity_Attribute_Name Stat_Name { get; private set; }
        public double Stat_Value => Internal_Parent?.Stat_Manager.Get_Stat(Stat_Name ?? "")?.Value ?? throw new Exception("TODO: replace excep");

        public string Particle_Type { get; protected set; }

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

        protected virtual void Handle_Begin_Resolution          (Combat_Action action) { }
        protected virtual void Handle_Calculate_Hit_Bonus       (Combat_Action action) { }
        protected virtual void Handle_Calculate_Redirect_Chance (Combat_Action action) { }
        protected virtual void Handle_Calculate_Damage          (Combat_Action action) { }
        protected virtual void Handle_Finish_Resolution         (Combat_Action action) { }

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
