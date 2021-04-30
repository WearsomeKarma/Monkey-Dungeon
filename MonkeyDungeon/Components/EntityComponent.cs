using isometricgame.GameEngine;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.ActingEntities;
using MonkeyDungeon.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon.Components
{
    public class EntityComponent : GameComponent
    {
        public static readonly string RACE_NAME_PLAYER  = "Monkey";

        public string Name                              { get; set; }
        public string Race                              { get; internal set; }
        public string TypeName                          { get; internal set; }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }

        public EntityController EntityController        { get; internal set; }
        public void Set_ActingEntity                    (EntityController newEntity) { EntityController?.LoseControl(); newEntity?.GainControl(this); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Get_BaseValue() : 0; set => SetLevel(value); }
        
        private List<EntityResource> Resources          = new List<EntityResource>();
        public EntityResource[] Get_Resources           () => Resources.ToArray();
        public T Get_ResourceByType<T>                  (string resourceName = null) where T : EntityResource { foreach (T resource in Resources.OfType<T>()) if (resource.IsEnabled && (resourceName == null || resource.Resource_Name == resourceName)) return resource; return null; }
        public EntityResource Get_Resource              (string resourceName) => Get_ResourceByType<EntityResource>(resourceName);
        public void Add_Resource<T>                     (T resource) where T : EntityResource { Resources.Add(resource); resource.Attach_ToEntity(this); }
        public void Remove_Resources<T>                 () where T : EntityResource { foreach (T resource in Resources.ToArray()) Resources.Remove(resource); }
        public void Replace_Resource<T>                 (T resource) where T : EntityResource { Remove_Resources<T>(); Add_Resource(resource); }

        private List<EntityStat> Stats                  = new List<EntityStat>();
        public EntityStat[] Get_Stats                   () => Stats.ToArray();
        public T Get_Stat<T>                            (string statName=null) where T : EntityStat { foreach (T stat in Stats) return stat; return null; }
        public EntityStat Get_Stat                      (string statName) => Get_Stat<EntityStat>(statName);

        public void Add_Stat                            (EntityStat stat) { Stats.Add(stat); stat.Attach_ToEntity(this); }

        private List<Resistance> Resistances            = new List<Resistance>();
        public Resistance Get_Resistance                (DamageType typeOfDamage_Resisting) { foreach (Resistance resistance in Resistances) if (resistance.TypeOfDamage_Resisting == typeOfDamage_Resisting) return resistance; return null; }
        public float Get_Resistance_Value               (DamageType typeOfDamage_Resisting) { Resistance r = Get_Resistance(typeOfDamage_Resisting); return (r == null) ? 1 : r.ResistancePercentage; }
        
        //TODO: filter by detremental
        private List<StatusEffect> StatusEffects        = new List<StatusEffect>();
        public StatusEffect[] Get_StatusEffects         () => StatusEffects.ToArray();
        public void Add_StatusEffect                    (StatusEffect effect) { effect.GiveThisEffect_NewOwner(this); StatusEffects.Add(effect);  }
        public void Remove_StatusEffect                 (StatusEffect effect) { if (!StatusEffects.Contains(effect)) return; StatusEffects.Remove(effect); effect.MakeOwnerLess(); }
        public void Remove_All_StatusEffects            () { foreach (StatusEffect effect in StatusEffects) Remove_StatusEffect(effect); }
        public void Remove_All_StatusEffects_Except<T>  () where T : StatusEffect { foreach (StatusEffect effect in StatusEffects) if (effect is T) Remove_StatusEffect(effect); }
        public void Disable_StatusEffects<T>            () where T : StatusEffect { foreach (StatusEffect effect in StatusEffects) if (effect is T) effect.ToggleThisEffect(false); }
        public void Disable_StatusEffect                (StatusEffect effect) { foreach (StatusEffect subEffect in StatusEffects) if (subEffect == effect) effect.ToggleThisEffect(false); }

        private List<Ability> Abilities                 = new List<Ability>();
        public Ability[] Get_Abilities                  () => Abilities.ToArray();
        public string[] Get_Ability_Names               () { string[] abilityNames = new string[Abilities.Count]; for (int i=0;i<Abilities.Count;i++) { abilityNames[i] = Abilities[i].Ability_Name; } return abilityNames; }
        public T Get_Ability<T>                         () where T : Ability { foreach (T ability in Abilities) return ability; return null; }
        public Ability Get_Ability                      (string abilityName) { foreach (Ability ability in Abilities) { if (ability.Ability_Name == abilityName) return ability;  } return null; }
        public void Add_Ability                         (Ability ability) { Abilities.Add(ability); ability.Attach_ToEntity(this); }
        public bool Use_Ability                         (CombatAction combatAction)
        {
            Ability a = Get_Ability(combatAction.CombatAction_Ability_Name);
            EntityResource r = Get_Resource(a.Resource_Name);
            bool usageCheck =
                TryPay_Ability_PointPool(a.Cost_Ability_Points, true)
                &&
                (r?.TryPay(a.Cost, true) ?? false)
                ;
            if (usageCheck)
            {
                TryPay_Ability_PointPool(a.Cost_Ability_Points);
                r.TryPay(a.Cost);
                a.EntityComponent_Use_Ability(combatAction);
            }
            return usageCheck;
        }

        private EntityResource Ability_PointPool        = new EntityResource("Ability_PointPool", 2, 2, 0, 2, 0);
        public int Ability_PointPool_Count              => (int)Ability_PointPool.Resource_Value;
        internal bool TryPay_Ability_PointPool          (int amount, bool peek = false) => Ability_PointPool.TryPay(amount, peek);

        public EntityComponent(
            string race,
            string name,
            string typeName,
            int level,
            int unique_ID,
            List<EntityStat> stats,
            List<EntityResource> resources,
            List<Ability> abilities,
            List<Resistance> resistances,
            EntityController controller
            )
        {
            Race = race;
            Name = name;
            TypeName = typeName;
            Unique_ID = unique_ID;
            foreach (EntityStat stat in stats)
                Add_Stat(stat);
            foreach (EntityResource resource in resources)
                Add_Resource(resource);
            foreach (Ability ability in abilities)
                Add_Ability(ability);
            foreach (Resistance resistance in resistances)
                Resistances.Add(resistance);
            Set_ActingEntity(controller);
            Init(level);
        }

        public EntityComponent(string race = null)
        {
            Race = race ?? RACE_NAME_PLAYER;
            Add_Resource(new Health(1, 1, 0, 0.1f, 1));
            Add_Resource(new Stamina(1, 1, 0.1f, 1));
            Add_Resource(new Mana(1, 1, 0.1f, 1));
            Set_ActingEntity(new ActingEntity_AI());
            Init();
        }

        private void Init(int level = 1)
        {
            Ability_PointPool.Attach_ToEntity(this);
            SetLevel(level);
        }
        
        internal void SetLevel(int level)
        {
            if (this.level == null)
            {
                this.level = new Level(level, 100, 0, 0, 0);
                this.level.Attach_ToEntity(this);
            }
            foreach (EntityResource resource in Resources)
                resource.PerformLevelChange();
            Ability_PointPool.PerformLevelChange();
        }

        internal float DealDamage_ToThis(Damage damage)
        {
            float? difference = Get_ResourceByType<Health>()?.Offset(-damage.Amount * Get_Resistance_Value(damage.DamageType));
            bool state = difference != null;
            float ret = (difference != null) ? (float)difference : float.NaN;
            if (state)
                Handle_DamageDealt_ToThis(damage.DamageType, ret);
            return ret;
        }

        internal float Recover_This<T>(float amount) where T : EntityResource
        {
            float diff = 0;
            foreach (T resource in Resources.OfType<T>())
                diff += resource.Offset(amount);
            return diff;
        }

        protected virtual void Handle_DamageDealt_ToThis(DamageType type, float amount)
        {

        }

        internal void Combat_BeginTurn(Combat_GameState combat)
        {
            HandleCombat_BeginTurn_PreUpkeep(combat);
            Ability_PointPool.Combat_BeginTurn_ReplenishResource(combat);

            foreach (StatusEffect effect in StatusEffects)
                effect.Combat_BeginTurn_StatusEffect(combat);
            
            //skip as a result of death,stun,petrification, etc.
            if (combat.CombatState == CombatState.FinishCurrentTurn)
                return;

            foreach (EntityResource resource in Resources)
                resource.Combat_BeginTurn_ReplenishResource(combat);

            if (CheckIf_IsTurnUnplayable(combat))
                return;

            HandleCombat_BeginTurn_PostUpkeep(combat);
        }

        internal void Combat_EndTurn(Combat_GameState combat)
        {
            HandleCombat_EndTurn_Cleanup(combat);
        }

        internal bool CheckIf_IsTurnUnplayable(Combat_GameState combat)
        {
            bool ret = !Has_PlayableMoves(combat);
            if (ret)
                combat.Request_EndOfTurn();
            return ret;
        }

        internal bool Has_PlayableMoves(Combat_GameState combat)
        {
            bool ret = combat.CombatState == CombatState.BeginNextTurn || combat.CombatState == CombatState.PlayCurrentTurn;

            if(ret)
            {
                ret =
                    !Ability_PointPool.IsDepleted
                    &&
                    CanUseAbilities();
                    ;
            }

            return ret;
        }

        private bool CanUseAbilities()
        {
            bool ret = false;
            bool? check;

            foreach(Ability ability in Abilities)
            {
                check = Get_ResourceByType<EntityResource>(ability.Resource_Name)?.TryPay(ability.Cost, true);
                //TODO: implement debugging
                if (check == null)
                    Console.WriteLine("[Warning EntityComponent.cs] Ability bound to entity without usable resource.");
                ret = ret || (check ?? false);
            }

            return ret;
        }

        protected virtual void HandleCombat_BeginTurn_PreUpkeep(Combat_GameState combat) { }
        protected virtual void HandleCombat_BeginTurn_PostUpkeep(Combat_GameState combat) { }
        protected virtual void HandleCombat_EndTurn_Cleanup(Combat_GameState combat) { }

        public override GameComponent Clone()
        {
            List<EntityStat> clonedStats = new List<EntityStat>();
            List<EntityResource> clonedResources = new List<EntityResource>();
            List<Ability> clonedAbilities = new List<Ability>();
            List<Resistance> clonedResistances = new List<Resistance>();

            foreach (EntityStat stat in Stats)
                clonedStats.Add(stat.Clone());
            foreach (EntityResource resource in Resources)
                clonedResources.Add(resource.Clone());
            foreach (Ability ability in Abilities)
                clonedAbilities.Add(ability.Clone());
            foreach (Resistance resistance in Resistances)
                clonedResistances.Add(resistance.Clone());

            return new EntityComponent(Race, Name, TypeName, Level, Unique_ID, Stats, Resources, Abilities, Resistances, EntityController);
        }

        public override string ToString()
        {
            string r = "";
            foreach (EntityResource re in Resources)
                r += string.Format("[{0}:{1}]", re.Resource_Name, re.Resource_Value);
            string ec_s = string.Format(
                "Name: {0} \tResources: <{1}>",
                Name,
                r
                );

            return ec_s;
        }
    }
}
