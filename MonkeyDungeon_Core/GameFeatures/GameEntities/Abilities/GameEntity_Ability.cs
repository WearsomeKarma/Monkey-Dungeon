using System;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability
    {
        public string Ability_Name { get; private set; }

        internal GameEntity Entity { get; private set; }
        internal int Entity_Scene_Id => Entity.Scene_GameObject_ID;

        internal string Resource_Name { get; private set; }
        public double Cost => Get_AbilityResourceCost();
        public int Cost_Ability_Points => Get_AbilityPointCost();
        public double Resource_Value => Entity?.Resource_Manager.Get_Resource(Resource_Name)?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Resource_ValueStrict => Entity?.Resource_Manager.Get_Resource(Resource_Name)?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        internal string Stat_Name { get; private set; }
        public double Stat_Value => Entity?.Stat_Manager.Get_Stat(Stat_Name ?? "")?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Stat_StrictValue => Entity?.Stat_Manager.Get_Stat(Stat_Name ?? "")?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        public Combat_Ability_Target Target { get; protected set; }
        public Combat_Target_Type Target_Type { get; protected set; }
        public bool Has_Strict_Targets { get; protected set; }

        public GameEntity_Ability(
            string name,
            string resourceName,
            string statName
            )
        {
            Ability_Name = name;
            Resource_Name = resourceName;
            Stat_Name = statName;
        }

        internal void Attach_ToEntity(GameEntity entity)
        {
            if (Entity != null)
                Detach_FromEntity();
            Entity = entity;
            Handle_GainingNewEntity();
        }
        protected virtual void Handle_GainingNewEntity() { }

        internal void Detach_FromEntity()
        {
            Handle_LosingEntity();
            Entity = null;
        }
        protected virtual void Handle_LosingEntity() { }

        protected virtual void Handle_Begin_Resolution          (Combat_Action action) { }
        protected virtual void Handle_Calculate_Hit_Bonus       (Combat_Action action) { }
        protected virtual void Handle_Calculate_Redirect_Chance (Combat_Action action) { }
        protected virtual void Handle_Calculate_Damage          (Combat_Action action) { }
        protected virtual void Handle_Finish_Resolution         (Combat_Action action) { }

        protected virtual double Get_RelevantOutput() => Entity?.Stat_Manager.Get_Stat(Stat_Name)?.Resource_Value ?? 0;
        protected virtual double Get_AbilityResourceCost() => 1;
        protected virtual int Get_AbilityPointCost() => 1;

        public virtual GameEntity_Ability Clone()
        {
            GameEntity_Ability clone = new GameEntity_Ability(
                Ability_Name,
                Resource_Name,
                Stat_Name
                );
            return clone;
        }

        public override string ToString()
        {
            return string.Format(
                "Name: {0} \tOutput: {1}",
                Ability_Name,
                Get_RelevantOutput()
                );
        }
    }
}
