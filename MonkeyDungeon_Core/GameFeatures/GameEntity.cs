using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.EntityControllers;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity
    {
        public string Name                              { get; set;             }
        public GameEntity_Attribute_Name Race                  { get; internal set;    }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }
        
        public int Relay_ID_Of_Owner                    { get; internal set;    }
        public GameEntity_ID Scene_GameObject_ID                  { get; internal set;    }
        public int Initative_Position                   { get; internal set;    }

        public GameEntity_Controller EntityController   { get; internal set;    }
        public void Set_ActingEntity                    (GameEntity_Controller newEntity) { EntityController?.LoseControl(); newEntity?.GainControl(this); }
        private bool incapacitated                      = false;
        public bool IsIncapacitated                     { get => incapacitated; internal set => Set_IncapacitatedState(value); }
        internal void Set_IncapacitatedState            (bool value = true) { incapacitated = value; if (value) Handle_Incapacitated(); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Value : 0; set => Set_Level(value); }
        
        public GameState_Machine Game                   { get; internal set;    }

        public readonly GameEntity_Stat_Manager         Stat_Manager;
        public readonly GameEntity_Resource_Manager     Resource_Manager;
        //public readonly GameEntity_Resistance_Manager   Resistance_Manager;
        public readonly GameEntity_Ability_Manager      Ability_Manager;
        public readonly GameEntity_StatusEffect_Manager StatusEffect_Manager;

        public GameEntity(

            GameEntity_Attribute_Name race,
            string  name,
            int     level,
            int     unique_ID,

            List<GameEntity_Stat>       stats,
            List<GameEntity_Resource>   resources,
            List<GameEntity_Ability>    abilities,
            //List<GameEntity_Resistance> resistances,
            GameEntity_Controller       controller

            )
        {
            Race                    = race;
            Name                    = name;
            Unique_ID               = unique_ID;

            Stat_Manager            = new GameEntity_Stat_Manager           (this, stats        );
            Resource_Manager        = new GameEntity_Resource_Manager       (this, resources    );
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this, resistances  );
            Ability_Manager         = new GameEntity_Ability_Manager        (this, abilities    );
            StatusEffect_Manager    = new GameEntity_StatusEffect_Manager   (this               );
            
            Set_ActingEntity        (controller);
            Set_Level               (level);
        }

        public GameEntity(GameEntity_Attribute_Name race = null)
        {
            Race                    = race ?? MD_VANILLA_RACES.RACE_MONKEY;
            
            Stat_Manager            = new GameEntity_Stat_Manager           (this);
            Resource_Manager        = new GameEntity_Resource_Manager       (this);
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this);
            Ability_Manager         = new GameEntity_Ability_Manager        (this);
            StatusEffect_Manager    = new GameEntity_StatusEffect_Manager   (this);

            Set_ActingEntity        (new GameEntity_Controller_AI());
            Set_Level               ();
        }

        internal void Set_Level(int level=1)
        {
            if (this.level == null)
            {
                this.level = new Level(100, level);
                this.level.Attach_To_Entity(this);
            }
            this.level.Force_Offset(level - Level);
        }

        internal void Combat_BeginTurn(Combat_GameState combat)
        {
            Handle_Combat_BeginTurn_PreUpkeep(combat);
            Ability_Manager.Combat_BeginTurn();

            StatusEffect_Manager.Combat_BeginTurn(combat);
            
            //skip as a result of death,stun,petrification, etc.
            if (combat.CombatState == CombatState.FinishCurrentTurn)
                return;

            throw new NotImplementedException();
            //Resource_Manager.Combat_BeginTurn(combat);

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
        protected virtual void Handle_Incapacitated()
        {
            Game.Relay_Death(this);
        }

        public GameEntity Clone()
        {
            List<GameEntity_Stat> clonedStats = new List<GameEntity_Stat>();
            List<GameEntity_Resource> clonedResources = new List<GameEntity_Resource>();
            List<GameEntity_Ability> clonedAbilities = new List<GameEntity_Ability>();
            //List<GameEntity_Resistance> clonedResistances = new List<GameEntity_Resistance>();

            foreach (GameEntity_Stat stat in Stat_Manager.Get_Stats())
                clonedStats.Add(stat.Clone());
            foreach (GameEntity_Resource resource in Resource_Manager.Get_Resources())
                clonedResources.Add(resource.Clone());
            foreach (GameEntity_Ability ability in Ability_Manager.Get_Abilities())
                clonedAbilities.Add(ability.Clone());
            //foreach (GameEntity_Resistance resistance in Resistance_Manager.Get_Resistances())
            //    clonedResistances.Add(resistance.Clone());

            return new GameEntity
                (Race,
                Name,
                Level,
                Unique_ID,
                clonedStats,
                clonedResources,
                clonedAbilities,
                //clonedResistances,
                EntityController.Clone()
                );
        }

        public override string ToString()
        {
            string r = "";
            foreach (GameEntity_Resource re in Resource_Manager.Get_Resources())
                r += string.Format("[{0}:{1}]", re.ATTRIBUTE_NAME, re.Value);
            string ec_s = string.Format(
                "Name: {0} \tResources: <{1}>",
                Name,
                r
                );

            return ec_s;
        }
    }
}
