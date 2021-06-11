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
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_ServerSide : GameEntity
    {
        public void Set_Ready_State(bool state)         => IsReady = state;
        public void Set_Incapacitated_State(bool state) => IsIncapacitated = state;
        
        public string Name                              { get; set;             }
        private int unique_ID                           = 0;
        public int Unique_ID                            { get => unique_ID; internal set => unique_ID = (value >= 0) ? value : 0; }

        public GameEntity_Controller EntityController   { get; internal set;    }
        public void Set_ActingEntity                    (GameEntity_Controller newEntity) { EntityController?.Lose_Control(); newEntity?.Gain_Control(this); }

        private Level level;
        public int Level                                { get => (level != null) ? (int)level.Value : 0; set => Set_Level(value); }
        
        public GameState_Machine Game                   { get; internal set;    }

        public readonly GameEntity_Stat_Manager         Stat_Manager;
        public readonly GameEntity_Resource_Manager     Resource_Manager;
        //public readonly GameEntity_Resistance_Manager   Resistance_Manager;
        public readonly GameEntity_Ability_Manager      Ability_Manager;
        public readonly GameEntity_StatusEffect_Manager StatusEffect_Manager;

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

            Stat_Manager            = new GameEntity_Stat_Manager           (this, stats        );
            Resource_Manager        = new GameEntity_Resource_Manager       (this, resources    );
            //Resistance_Manager      = new GameEntity_Resistance_Manager     (this, resistances  );
            Ability_Manager         = new GameEntity_Ability_Manager        (this, abilities    );
            StatusEffect_Manager    = new GameEntity_StatusEffect_Manager   (this               );
            
            Set_ActingEntity        (controller);
            Set_Level               (level);
        }

        public GameEntity_ServerSide(GameEntity_Attribute_Name_Race race = null)
        {
            GameEntity_Race         = race ?? MD_VANILLA_RACE_NAMES.RACE_MONKEY;
            
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

        internal void Combat_BeginTurn(GameEntity_ServerSide_Roster gameField)
        {
            Handle_Combat_BeginTurn_PreUpkeep(gameField);

            StatusEffect_Manager.Combat_BeginTurn(gameField);

            //TODO: improve on this.
            Ability_Manager.Ability_Point_Pool.Set_Value(2);
            
            Handle_Combat_BeginTurn_PostUpkeep(gameField);
        }

        internal void Combat_EndTurn(GameEntity_ServerSide_Roster gameField)
        {
            Handle_Combat_EndTurn_Cleanup(gameField);
        }

        internal bool Has_PlayableMoves()
        {
            return !Ability_Manager.Ability_Point_Pool.IsDepleted;
        }

        protected virtual void Handle_Combat_BeginTurn_PreUpkeep(GameEntity_ServerSide_Roster gameField) { }
        protected virtual void Handle_Combat_BeginTurn_PostUpkeep(GameEntity_ServerSide_Roster gameField) { }
        protected virtual void Handle_Combat_EndTurn_Cleanup(GameEntity_ServerSide_Roster gameField) { }
        protected virtual void Handle_Incapacitated()
        {
            Game.Relay_Death(this);
        }

        public GameEntity_ServerSide Clone(GameEntity_ID gameEntityId)
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
            EntityController.Clone().Gain_Control(entityServerSide);
            
            return entityServerSide;
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
