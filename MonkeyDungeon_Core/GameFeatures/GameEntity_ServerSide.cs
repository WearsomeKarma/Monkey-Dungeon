using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Controllers;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Stats;
using MonkeyDungeon_Core.GameFeatures.GameEntities.StatusEffects;
using MonkeyDungeon_Core.GameFeatures.GameStates;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    //TODO: seal this.
    public class GameEntity_ServerSide : GameEntity
    {
        public void Set_Ready_State(bool state)         => IsReady = state;
        public void Set_Incapacitated_State(bool state) => IsIncapacitated = state;

        public void Set_Position(GameEntity_Position position)
            => GameEntity_Position = position;
        
        public string Name                              { get; set;             }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }

        public GameEntity_Controller EntityController   { get; internal set;    }
        public void Attach__Controller                  (GameEntity_Controller newEntity) { EntityController?.Controller_Detach_From_Entity(); newEntity?.Controller_Attack_To_Entity(this); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Value : 0; set => Set__GameEntity_Level(value); }
        
        public GameState_Machine Game                   { get; internal set;    }
        public GameEntity_ServerSide_Roster Entity_Field => Game?.GameField;

        private readonly GameEntity_Stat_Manager         STAT_MANAGER;
        public T     Get__Stat__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_Stat
            => STAT_MANAGER.Get__Stat<T>(name);
        
        private readonly GameEntity_Resource_Manager     RESOURCE_MANAGER;
        public T     Get__Resource__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_Resource
            => RESOURCE_MANAGER.Get__Resource<T>(name);
        public void Offset__Resource__GameEntity<T>(GameEntity_Attribute_Name name, double value)
            where T : GameEntity_Resource
            => Get__Resource__GameEntity<T>(name).Offset_Value(value);
        public GameEntity_Attribute_Name[] Get__Resource_Names__GameEntity()
            => RESOURCE_MANAGER.Get__Resource_Names();
        
        //public readonly GameEntity_Resistance_Manager   Resistance_Manager;
        private readonly GameEntity_Ability_Manager      ABILITY_MANAGER;
        public T     Get__Ability__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_Ability
            => ABILITY_MANAGER.Get__Ability<T>(name);
        public GameEntity_Ability[] Get__Abilities__GameEntity()
            => ABILITY_MANAGER.Get__Abilities();
        
        private readonly GameEntity_StatusEffect_Manager STATUSEFFECT_MANAGER;
        public T     Get__StatusEffect__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_StatusEffect
            => STATUSEFFECT_MANAGER.Get__StatusEffect__GameEntity<T>(name);
        public void  Add__GameEntity_StatusEffect<T>(T statusEffect) 
            where T : GameEntity_StatusEffect
            => STATUSEFFECT_MANAGER.Add__StatusEffect__GameEntity(statusEffect);
        public void  Remove__GameEntity_StatusEffect<T>(T statusEffect) 
            where T : GameEntity_StatusEffect
            => STATUSEFFECT_MANAGER.Remove__StatusEffect__GameEntity(statusEffect);
        public void  Remove_All__StatusEffects__GameEntity()
            => STATUSEFFECT_MANAGER.Remove_All__StatusEffects__GameEntity();
        public void  Remove_All__StatusEffects_Except__GameEntity<T>() 
            where T : GameEntity_StatusEffect
            => STATUSEFFECT_MANAGER.Remove_All__Except<T>();
        public void React_To__Cast__GameEntity(Combat_Action action)
            => STATUSEFFECT_MANAGER.React_To_Cast(action);
        public Combat_Redirection_Chance[] React_To__Redirect_Chance__GameEntity
            (
            Combat_Action action,
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => STATUSEFFECT_MANAGER.React_To_Redirect_Chance
                (
                action,
                assaultType,
                assaulterPositionType,
                targetPositionType,
                baseChance
                );
        public void React_To__Pre_Resource_Offset__GameEntity
            (
            GameEntity_Attribute_Name resourceName,
            double finalizedOffset
            )
            => STATUSEFFECT_MANAGER.React_To_Pre_Resource_Offset
                (
                resourceName,
                finalizedOffset
                );
        public void React_To__Post_Resource_Offset__GameEntity
            (
            GameEntity_Attribute_Name resourceName,
            double finalizedOffset
            )
            => STATUSEFFECT_MANAGER.React_To_Post_Resource_Offset
                (
                resourceName,
                finalizedOffset
                );
        public double Get_Dodge_Bonuses__GameEntity(Combat_Action action)
            => STATUSEFFECT_MANAGER.Get_Dodge_Bonuses__StatusEffect_Manager(action);
        public double Get_Hit_Bonuses__GameEntity(Combat_Action action)
            => STATUSEFFECT_MANAGER.Get_Hit_Bonuses__StatusEffect_Manager(action);
        
        public GameEntity_ServerSide(

            GameEntity_Attribute_Name_Race race,
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
            GameEntity_Race         = race;
            Name                    = name;
            Unique_ID               = unique_ID;

            STAT_MANAGER            = new GameEntity_Stat_Manager           (this, stats        );
            RESOURCE_MANAGER        = new GameEntity_Resource_Manager       (this, resources    );
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this, resistances  );
            ABILITY_MANAGER         = new GameEntity_Ability_Manager        (this, abilities    );
            STATUSEFFECT_MANAGER    = new GameEntity_StatusEffect_Manager   (this               );
            
            Attach__Controller        (controller);
            Set__GameEntity_Level               (level);
        }

        public GameEntity_ServerSide(GameEntity_Attribute_Name_Race race = null)
        {
            GameEntity_Race         = race ?? MD_VANILLA_RACE_NAMES.RACE_MONKEY;
            
            STAT_MANAGER            = new GameEntity_Stat_Manager           (this);
            RESOURCE_MANAGER        = new GameEntity_Resource_Manager       (this);
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this);
            ABILITY_MANAGER         = new GameEntity_Ability_Manager        (this);
            STATUSEFFECT_MANAGER    = new GameEntity_StatusEffect_Manager   (this);

            Attach__Controller        (new GameEntity_Controller_AI());
            Set__GameEntity_Level               ();
        }

        internal void Set__GameEntity_Level(int level=1)
        {
            if (this.level == null)
            {
                this.level = new Level(100, level);
                this.level.Attach_To_Entity(this);
            }
            this.level.Force_Offset__Resource(level - Level);
        }

        internal void Combat_Begin__GameEntity()
        {
            EntityController.Combat_Begin();
        }
        
        internal void Combat_Begin_Turn__GameEntity()
        {
            STATUSEFFECT_MANAGER.Combat_BeginTurn__StatusEffect_Manager();

            EntityController.Combat_Begin_Turn();
            //TODO: improve on this.
            ABILITY_MANAGER.Ability_Point_Pool.Set_Value(2);
        }

        internal void Combat_End_Turn__GameEntity()
        {
            EntityController.Combat_End_Turn();
        }

        internal void Combat_End__GameEntity()
        {
            EntityController.Combat_End();
        }

        internal bool Has_PlayableMoves__GameEntity()
        {
            return !ABILITY_MANAGER.Ability_Point_Pool.IsDepleted;
        }
        
        protected virtual void Handle_Incapacitated__GameEntity()
        {
            Game.Relay_Death(this);
        }

        public GameEntity_ServerSide Clone__GameEntity(GameEntity_ID gameEntityId)
        {
            List<GameEntity_Stat> clonedStats = new List<GameEntity_Stat>();
            List<GameEntity_Resource> clonedResources = new List<GameEntity_Resource>();
            List<GameEntity_Ability> clonedAbilities = new List<GameEntity_Ability>();
            //List<GameEntity_Resistance> clonedResistances = new List<GameEntity_Resistance>();

            foreach (GameEntity_Stat stat in STAT_MANAGER.Get__Stats())
                clonedStats.Add(stat.Clone__Stat());
            foreach (GameEntity_Resource resource in RESOURCE_MANAGER.Get__Resources())
                clonedResources.Add(resource.Clone__Resource());
            foreach (GameEntity_Ability ability in ABILITY_MANAGER.Get__Abilities())
                clonedAbilities.Add(ability.Clone__Ability());
            //foreach (GameEntity_Resistance resistance in Resistance_Manager.Get_Resistances())
            //    clonedResistances.Add(resistance.Clone());

            GameEntity_ServerSide entityServerSide = new GameEntity_ServerSide
                (
                GameEntity_Race,
                Name,
                Level,
                Unique_ID,
                clonedStats,
                clonedResources,
                clonedAbilities,
                //clonedResistances,
                null
                ) 
                { GameEntity_ID = gameEntityId};

            //TODO: fix
            EntityController.Clone__Controller().Controller_Attack_To_Entity(entityServerSide);
            
            return entityServerSide;
        }

        public override string ToString()
        {
            string r = "";
            foreach (GameEntity_Resource re in RESOURCE_MANAGER.Get__Resources())
                r += string.Format("[{0}:{1}]", re.Attribute_Name, re.Value);
            string ec_s = string.Format(
                "Name: {0} \tResources: <{1}>",
                Name,
                r
                );

            return ec_s;
        }
    }
}
