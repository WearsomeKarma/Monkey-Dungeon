using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.CombatObjects
{
    public class GameEntity_Ability
    {
        public string Ability_Name { get; private set; }

        internal GameEntity Entity { get; private set; }
        
        internal string Resource_Name { get; private set; }
        public double Cost => Get_AbilityResourceCost();
        public int Cost_Ability_Points => Get_AbilityPointCost();
        public double Resource_Value => Entity?.Resource_Manager.Get_Resource(Resource_Name)?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Resource_ValueStrict => Entity?.Resource_Manager.Get_Resource(Resource_Name)?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        internal string Stat_Name { get; private set; }
        public double Stat_Value => Entity?.Stat_Manager.Get_Stat(Stat_Name ?? "")?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Stat_StrictValue => Entity?.Stat_Manager.Get_Stat(Stat_Name ?? "")?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        public DamageType Ability_DamageType { get; private set; }

        public bool Requires_Target { get; private set; }
        
        public GameEntity_Ability(
            string name, 
            string resourceName,
            string statName, 
            DamageType relevantDamageType = DamageType.Abstract, 
            bool requiresTarget = false)
        {
            Ability_Name = name;
            Resource_Name = resourceName;
            Stat_Name = statName;
            Ability_DamageType = relevantDamageType;
            Requires_Target = requiresTarget;
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
        
        internal void EntityComponent_Use_Ability(Combat_Action combatAction)
        {
            Handle_AbilityUsage(combatAction);
        }

        protected virtual void Handle_AbilityUsage(Combat_Action combatAction)
        {

        }

        protected float ImplementedHandle_DealDamage(Combat_Action combatAction)
        {
            if (combatAction.HasTarget)
            {
                GameEntity target = Entity.Game.Get_Entity(combatAction.Target_ID);
                return target.Damage_This(new Combat_Damage(Ability_DamageType, Get_RelevantOutput()));
            }
            return 0;
        }

        protected double ImpementedHandle_Recover<T>(GameEntity target) where T : GameEntity_Resource
        {
            return target.Recover_This<T>(Get_RelevantOutput());
        }

        protected virtual double Get_RelevantOutput() => Entity?.Stat_Manager.Get_Stat(Stat_Name)?.Resource_Value ?? 0;
        protected virtual double Get_AbilityResourceCost() => 1;
        protected virtual int Get_AbilityPointCost() => 1;

        public virtual GameEntity_Ability Clone()
        {
            GameEntity_Ability clone = new GameEntity_Ability(
                Ability_Name,
                Resource_Name,
                Stat_Name,
                Ability_DamageType,
                Requires_Target
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
