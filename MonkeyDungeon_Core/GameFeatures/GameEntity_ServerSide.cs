using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameComponents.Controllers;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources.Implemented;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Stats;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.StatusEffects;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;
using System.Collections.Generic;
using System;

namespace MonkeyDungeon_Core.GameFeatures
{
    //TODO: seal this.
    public class GameEntity_ServerSide : GameEntity
    {
        public void Set_Ready_State(bool state)         => IsReady = state;
        public void Set_Incapacitated_State(bool state) { IsIncapacitated = state; Handle_Incapacitated__GameEntity(); }

        public void Set_Position(GameEntity_Position position)
            => GameEntity_Position = position;
        
        public string Name                              { get; set;             }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }

        public GameEntity_ServerSide_Controller EntityServerSideController   { get; internal set;    }
        public void Attach__Controller                  (GameEntity_ServerSide_Controller newEntityServerSide) { EntityServerSideController?.Controller_Detach_From_Entity(); newEntityServerSide?.Controller_Attack_To_Entity(this); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Value : 0; set => Set__GameEntity_Level(value); }
        
        public Game_StateMachine Game                   { get; internal set;    }
        public GameEntity_ServerSide_Roster Entity_Field => Game?.GameField;

        private readonly GameEntity_Stat_Manager         STAT_MANAGER;
        public T     Get__Stat__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_ServerSide_Stat
            => STAT_MANAGER.Get__Stat<T>(name);
        
        private readonly GameEntity_Resource_Manager     RESOURCE_MANAGER;
        private void Relay__Resource_Updated(GameEntity_ServerSide_Resource resource)
            => Event__Resource_Updated__GameEntity?.Invoke(resource);
        public event Action<GameEntity_ServerSide_Resource> Event__Resource_Updated__GameEntity;
        public T     Get__Resource__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_ServerSide_Resource
            => RESOURCE_MANAGER.Get__Resource<T>(name);
        public void Offset__Resource__GameEntity<T>(GameEntity_Attribute_Name name, double value)
            where T : GameEntity_ServerSide_Resource
            => Get__Resource__GameEntity<T>(name).Offset__Value__Quantity(value);
        public GameEntity_Attribute_Name[] Get__Resource_Names__GameEntity()
            => RESOURCE_MANAGER.Get__Resource_Names();
        
        //public readonly GameEntity_Resistance_Manager   Resistance_Manager;
        private readonly GameEntity_Ability_Manager      ABILITY_MANAGER;
        private void Relay__Ability_Points_Updated<T>(GameEntity_Quantity<T> points) where T : GameEntity =>
            Event__Ability_Points_Updated__GameEntity?.Invoke(points as GameEntity_ServerSide_Resource);
        public event Action<GameEntity_ServerSide_Resource> Event__Ability_Points_Updated__GameEntity;
        public T     Get__Ability__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_ServerSide_Ability
            => ABILITY_MANAGER.Get__Ability<T>(name);
        public GameEntity_ServerSide_Ability[] Get__Abilities__GameEntity()
            => ABILITY_MANAGER.Get__Abilities();
        public bool Try_Offset__Ability_Point__GameEntity(int offset, bool peeking = false)
            => ABILITY_MANAGER.Ability_Point_Pool.Try_Offset__Resource(offset, peeking);
        public int Get__Ability_Point_Count__GameEntity()
            => (int)ABILITY_MANAGER.Ability_Point_Pool.Value;
        
        private readonly GameEntity_StatusEffect_Manager STATUSEFFECT_MANAGER;
        public T     Get__StatusEffect__GameEntity<T>(GameEntity_Attribute_Name name) 
            where T : GameEntity_ServerSide_StatusEffect
            => STATUSEFFECT_MANAGER.Get__StatusEffect__GameEntity<T>(name);
        public void  Add__GameEntity_StatusEffect<T>(T statusEffect) 
            where T : GameEntity_ServerSide_StatusEffect
            => STATUSEFFECT_MANAGER.Add__StatusEffect__GameEntity(statusEffect);
        public void  Remove__GameEntity_StatusEffect<T>(T statusEffect) 
            where T : GameEntity_ServerSide_StatusEffect
            => STATUSEFFECT_MANAGER.Remove__StatusEffect__GameEntity(statusEffect);
        public void  Remove_All__StatusEffects__GameEntity()
            => STATUSEFFECT_MANAGER.Remove_All__StatusEffects__GameEntity();
        public void  Remove_All__StatusEffects_Except__GameEntity<T>() 
            where T : GameEntity_ServerSide_StatusEffect
            => STATUSEFFECT_MANAGER.Remove_All__Except<T>();
        public void React_To__Cast__GameEntity()
            => STATUSEFFECT_MANAGER.React_To_Cast();
        public Combat_Redirection_Chance[] React_To__Redirect_Chance__GameEntity
            (
            Combat_Assault_Type assaultType,
            GameEntity_Position_Type assaulterPositionType,
            GameEntity_Position_Type targetPositionType,
            Combat_Redirection_Chance baseChance
            )
            => STATUSEFFECT_MANAGER.React_To_Redirect_Chance
                (
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
        public double Get_Dodge_Bonuses__GameEntity()
            => STATUSEFFECT_MANAGER.Get_Dodge_Bonuses__StatusEffect_Manager();
        public double Get_Hit_Bonuses__GameEntity()
            => STATUSEFFECT_MANAGER.Get_Hit_Bonuses__StatusEffect_Manager();
        
        public GameEntity_ServerSide(

            GameEntity_Attribute_Name_Race race,
            string  name,
            int     level,
            int     unique_ID,

            List<GameEntity_ServerSide_Stat>       stats,
            List<GameEntity_ServerSide_Resource>   resources,
            List<GameEntity_ServerSide_Ability>    abilities,
            //List<GameEntity_Resistance> resistances,
            GameEntity_ServerSide_Controller       serverSideController

            )
        {
            GameEntity_Race         = race;
            Name                    = name;
            Unique_ID               = unique_ID;

            STAT_MANAGER            = new GameEntity_Stat_Manager           (this, stats        );
            RESOURCE_MANAGER        = new GameEntity_Resource_Manager       (this, resources    );
            RESOURCE_MANAGER.Event__Resource_Updated += Relay__Resource_Updated;
            
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this, resistances  );
            ABILITY_MANAGER         = new GameEntity_Ability_Manager        (this, abilities    );
            ABILITY_MANAGER.Ability_Point_Pool.Event__Quantity_Changed__Quantity += Relay__Ability_Points_Updated;
            
            STATUSEFFECT_MANAGER    = new GameEntity_StatusEffect_Manager   (this               );
            
            Attach__Controller        (serverSideController);
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

            Attach__Controller        (new GameEntityServerSideControllerAi());
            Set__GameEntity_Level               ();
        }

        internal void Set__GameEntity_Level(int level=1)
        {
            if (this.level == null)
            {
                this.level = new Level(100, level);
                this.level.Attach_To__Entity__ServerSide_Resource(this);
            }
            this.level.Force_Offset__Resource(level - Level);
        }

        internal void Combat_Begin__GameEntity()
        {
            EntityServerSideController.Combat_Begin();
        }
        
        internal void Combat_Begin_Turn__GameEntity()
        {
            STATUSEFFECT_MANAGER.Combat_BeginTurn__StatusEffect_Manager();

            EntityServerSideController.Combat_Begin_Turn();
            //TODO: improve on this.
            ABILITY_MANAGER.Ability_Point_Pool.Set__Value__Quantity(2);
        }

        internal void Combat_End_Turn__GameEntity()
        {
            EntityServerSideController.Combat_End_Turn();
        }

        internal void Combat_End__GameEntity()
        {
            EntityServerSideController.Combat_End();
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
            List<GameEntity_ServerSide_Stat> clonedStats = new List<GameEntity_ServerSide_Stat>();
            List<GameEntity_ServerSide_Resource> clonedResources = new List<GameEntity_ServerSide_Resource>();
            List<GameEntity_ServerSide_Ability> clonedAbilities = new List<GameEntity_ServerSide_Ability>();
            //List<GameEntity_Resistance> clonedResistances = new List<GameEntity_Resistance>();

            foreach (GameEntity_ServerSide_Stat stat in STAT_MANAGER.Get__Stats())
                clonedStats.Add(stat.Clone__Stat() as GameEntity_ServerSide_Stat);
            foreach (GameEntity_ServerSide_Resource resource in RESOURCE_MANAGER.Get__Resources())
                clonedResources.Add(resource.Clone__Resource() as GameEntity_ServerSide_Resource);
            foreach (GameEntity_ServerSide_Ability ability in ABILITY_MANAGER.Get__Abilities())
                clonedAbilities.Add(ability.Clone__ServerSide_Ability());
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
            EntityServerSideController.Clone__Controller().Controller_Attack_To_Entity(entityServerSide);
            
            return entityServerSide;
        }

        public override string ToString()
        {
            string r = "";
            foreach (GameEntity_ServerSide_Resource re in RESOURCE_MANAGER.Get__Resources())
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
