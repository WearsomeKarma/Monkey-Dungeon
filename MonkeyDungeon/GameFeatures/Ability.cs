using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class Ability
    {
        public string Ability_Name { get; private set; }

        internal EntityComponent Entity { get; private set; }
        
        internal string Resource_Name { get; private set; }
        public double Cost => Get_AbilityResourceCost();
        public int Cost_Ability_Points => Get_AbilityPointCost();
        public double Resource_Value => Entity?.Get_Resource(Resource_Name)?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Resource_ValueStrict => Entity?.Get_Resource(Resource_Name)?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        internal string Stat_Name { get; private set; }
        public double Stat_Value => Entity?.Get_Stat(Stat_Name ?? "")?.Resource_Value ?? throw new Exception("TODO: replace excep");
        public double Stat_StrictValue => Entity?.Get_Stat(Stat_Name ?? "")?.Resource_StrictValue ?? throw new Exception("TODO: replace excep");

        public DamageType Ability_DamageType { get; private set; }

        public bool Requires_Target { get; private set; }
        
        public Ability(
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

        internal void Attach_ToEntity(EntityComponent entity)
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
        
        internal void EntityComponent_Use_Ability(CombatAction combatAction)
        {
            Handle_AbilityUsage(combatAction);
        }

        protected virtual void Handle_AbilityUsage(CombatAction combatAction)
        {

        }

        protected float ImplementedHandle_DealDamage(CombatAction combatAction)
        {
            if (combatAction.HasTarget)
                return combatAction.Target.Damage_This(new Damage(Ability_DamageType, Get_RelevantOutput()));
            return 0;
        }

        protected double ImpementedHandle_Recover<T>(EntityComponent target) where T : EntityResource
        {
            return target.Recover_This<T>(Get_RelevantOutput());
        }

        protected virtual double Get_RelevantOutput() => Entity?.Get_Stat(Stat_Name)?.Resource_Value ?? 0;
        protected virtual double Get_AbilityResourceCost() => 1;
        protected virtual int Get_AbilityPointCost() => 1;

        public virtual Ability Clone()
        {
            Ability clone = new Ability(
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
