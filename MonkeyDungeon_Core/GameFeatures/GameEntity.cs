using MonkeyDungeon_Core.GameFeatures;
using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityControllers;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity
    {
        public static readonly string RACE_NAME_PLAYER  = "Monkey";

        public string Name                              { get; set; }
        public string Race                              { get; internal set; }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }
        
        public int Scene_GameObject_ID                  { get; internal set; }
        public int Initative_Position                   { get; internal set; }

        public GameEntity_Controller EntityController   { get; internal set; }
        public void Set_ActingEntity                    (GameEntity_Controller newEntity) { EntityController?.LoseControl(); newEntity?.GainControl(this); }
        private bool incapacitated                      = false;
        public bool IsIncapacitated                     { get => incapacitated; internal set => Set_IncapacitatedState(value); }
        internal void Set_IncapacitatedState            (bool value = true) { incapacitated = value; if (value) Handle_Incapacitated(); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Get_BaseValue() : 0; set => Set_Level(value); }

        public readonly GameEntity_Stat_Manager         Stat_Manager;
        public readonly GameEntity_Resource_Manager     Resource_Manager;
        public readonly GameEntity_Resistance_Manager   Resistance_Manager;
        public readonly GameEntity_Ability_Manager      Ability_Manager;
        public readonly GameEntity_StatusEffect_Manager StatusEffect_Manager;

        public bool Use_Ability                         (Combat_Action combatAction)
        {
            GameEntity_Ability a = Ability_Manager.Get_Ability(combatAction.CombatAction_Ability_Name);
            GameEntity_Resource r = Resource_Manager.Get_Resource(a.Resource_Name);
            bool usageCheck =
                Ability_Manager.TryPay_Ability_PointPool(a.Cost_Ability_Points, true)
                &&
                (r?.TryPay(a.Cost, true) ?? false)
                ;
            if (usageCheck)
            {
                Ability_Manager.TryPay_Ability_PointPool(a.Cost_Ability_Points);
                r.TryPay(a.Cost);
                a.EntityComponent_Use_Ability(combatAction);
            }
            return usageCheck;
        }

        public GameEntity(
            string race,
            string name,
            int level,
            int unique_ID,
            List<GameEntity_Stat> stats,
            List<GameEntity_Resource> resources,
            List<GameEntity_Ability> abilities,
            List<GameEntity_Resistance> resistances,
            GameEntity_Controller controller
            )
        {
            Race                    = race;
            Name                    = name;
            Unique_ID               = unique_ID;

            Stat_Manager            = new GameEntity_Stat_Manager           (this, stats);
            Resource_Manager        = new GameEntity_Resource_Manager       (this, resources);
            Resistance_Manager      = new GameEntity_Resistance_Manager     (this, resistances);
            Ability_Manager         = new GameEntity_Ability_Manager        (this, abilities);
            StatusEffect_Manager    = new GameEntity_StatusEffect_Manager   (this);
            
            Set_ActingEntity(controller);
            Set_Level(level);
        }

        public GameEntity(string race = null)
        {
            Race = race ?? RACE_NAME_PLAYER;
            
            Stat_Manager            = new GameEntity_Stat_Manager           (this);
            Resource_Manager        = new GameEntity_Resource_Manager       (this);
            Resistance_Manager      = new GameEntity_Resistance_Manager     (this);
            Ability_Manager         = new GameEntity_Ability_Manager        (this);
            StatusEffect_Manager    = new GameEntity_StatusEffect_Manager   (this);

            Set_ActingEntity(new GameEntity_Controller_AI());
            Set_Level();
        }

        internal void Set_Level(int level=1)
        {
            if (this.level == null)
            {
                this.level = new Level(level, 100, 0, 0, 0);
                this.level.Attach_ToEntity(this);
            }
            this.level.Offset(level - Level);
        }

        internal float Damage_This(Combat_Damage damage)
        {
            double? difference = 
                Resource_Manager
                .Get_ResourceByType<Health>()?
                .Offset(-damage.Amount * Resistance_Manager.Get_Resistance_Value(damage.DamageType));
            bool state = difference != null;
            float ret = (difference != null) ? (float)difference : float.NaN;
            if (state)
                Handle_DamageDealt_ToThis(damage.DamageType, ret);
            return ret;
        }

        internal double Recover_This<T>(double amount) where T : GameEntity_Resource
        {
            return Resource_Manager.Recover<T>(amount);
        }

        protected virtual void Handle_DamageDealt_ToThis(DamageType type, float amount)
        {

        }

        internal void Combat_BeginTurn(Combat_GameState combat)
        {
            Handle_Combat_BeginTurn_PreUpkeep(combat);
            Ability_Manager.Combat_BeginTurn(combat);

            StatusEffect_Manager.Combat_BeginTurn(combat);
            
            //skip as a result of death,stun,petrification, etc.
            if (combat.CombatState == CombatState.FinishCurrentTurn)
                return;

            Resource_Manager.Combat_BeginTurn(combat);

            if (CheckIf_IsTurnUnplayable(combat))
                return;

            Handle_Combat_BeginTurn_PostUpkeep(combat);
        }

        internal void Combat_EndTurn(Combat_GameState combat)
        {
            Handle_Combat_EndTurn_Cleanup(combat);
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
                    !Ability_Manager.Ability_PointPool_IsDepleted
                    &&
                    CheckIf_CanUseAbilities();
                    ;
            }

            return ret;
        }

        private bool CheckIf_CanUseAbilities()
        {
            return Ability_Manager.CheckIf_CanUseAbilities();
        }

        protected virtual void Handle_Combat_BeginTurn_PreUpkeep(Combat_GameState combat) { }
        protected virtual void Handle_Combat_BeginTurn_PostUpkeep(Combat_GameState combat) { }
        protected virtual void Handle_Combat_EndTurn_Cleanup(Combat_GameState combat) { }
        protected virtual void Handle_Incapacitated() { }

        public GameEntity Clone()
        {
            List<GameEntity_Stat> clonedStats = new List<GameEntity_Stat>();
            List<GameEntity_Resource> clonedResources = new List<GameEntity_Resource>();
            List<GameEntity_Ability> clonedAbilities = new List<GameEntity_Ability>();
            List<GameEntity_Resistance> clonedResistances = new List<GameEntity_Resistance>();

            foreach (GameEntity_Stat stat in Stat_Manager.Get_Stats())
                clonedStats.Add(stat.Clone());
            foreach (GameEntity_Resource resource in Resource_Manager.Get_Resources())
                clonedResources.Add(resource.Clone());
            foreach (GameEntity_Ability ability in Ability_Manager.Get_Abilities())
                clonedAbilities.Add(ability.Clone());
            foreach (GameEntity_Resistance resistance in Resistance_Manager.Get_Resistances())
                clonedResistances.Add(resistance.Clone());

            return new GameEntity
                (Race,
                Name,
                Level,
                Unique_ID,
                clonedStats,
                clonedResources,
                clonedAbilities,
                clonedResistances,
                EntityController.Clone()
                );
        }

        public override string ToString()
        {
            string r = "";
            foreach (GameEntity_Resource re in Resource_Manager.Get_Resources())
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
