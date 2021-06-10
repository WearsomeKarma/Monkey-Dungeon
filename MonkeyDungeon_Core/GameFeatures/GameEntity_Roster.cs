using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain;
using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Roster
    {
        internal static int ROSTER_COUNT { get; private set; }
        private GameState_Machine Game { get; set; }

        private readonly GameEntity_RosterEntry[] ROSTER_ENTRIES;

        internal GameEntity_RosterEntry[] Get_Roster_Entries()
        {
            GameEntity_RosterEntry[] entities = new GameEntity_RosterEntry[ROSTER_ENTRIES.Length];
            for (int i = 0; i < ROSTER_ENTRIES.Length; i++)
                entities[i] = ROSTER_ENTRIES[i];
            return entities;
        }

        internal GameEntity_ID[] Get_IDs()
        {
            GameEntity_ID[] ids = new GameEntity_ID[ROSTER_ENTRIES.Length];
            for (int i = 0; i < ROSTER_ENTRIES.Length; i++)
                ids[i] = ROSTER_ENTRIES[i].GameEntity_ID;
            return ids;
        }
        
        internal GameEntity Get_Entity(GameEntity_ID id)
        {
            foreach(GameEntity_RosterEntry entry in ROSTER_ENTRIES)
                if (entry.GameEntity_ID == id)
                    return entry.Entity;
            return null;
        }

        internal GameEntity Get_Entity(GameEntity_Position_Type positionType)
        {
            foreach (GameEntity_RosterEntry entry in ROSTER_ENTRIES)
                if (positionType == (GameEntity_Position_Type) entry.World_Position)
                    return entry.Entity;
            return null;
        }
        
        internal readonly GameEntity_Roster_Id ROSTER_ID;

        public bool Is_On_Team(GameEntity_ID id)
            => id.Roster_ID == ROSTER_ID;
        
        internal GameEntity_Roster(GameState_Machine game, GameEntity[] entities)
        {
            //TODO: constraint this index overflow.
            ROSTER_ID = GameEntity_Roster_Id.ROSTER_IDS[ROSTER_COUNT];
            ROSTER_COUNT++;
            Game = game;
            ROSTER_ENTRIES = new GameEntity_RosterEntry[entities.Length];

            for (int i = 0; i < ROSTER_ENTRIES.Length; i++)
                ROSTER_ENTRIES[i] = new GameEntity_RosterEntry(entities[i], ROSTER_ID);
            
            Set_Entities(entities);
        }

        internal void Set_Ready_To_Start(GameEntity_ID entityId, bool state = true)
        {
            ROSTER_ENTRIES[entityId % MD_PARTY.MAX_PARTY_SIZE].Is_Ready = true;
        }

        public GameEntity Set_Entity(GameEntity gameEntity)
        {
            if (gameEntity == null)
                return null;

            ROSTER_ENTRIES[gameEntity.GameEntity_ID % MD_PARTY.MAX_PARTY_SIZE].Entity = gameEntity;
            gameEntity.Game = Game;
            gameEntity.Resource_Manager.Resources_Updated += (e) => Game.Relay_Entity_Resource(e);

            //TODO: make this better.
            gameEntity.Ability_Manager.Ability_Point_Pool.Quantity_Changed +=
                (e) => Game.Relay_Entity_Static_Resource(e as GameEntity_Resource);
            return gameEntity;
        }
        
        internal GameEntity[] Set_Entities(GameEntity[] gameEntities)
        {
            foreach (GameEntity gameEntity in gameEntities)
                Set_Entity(gameEntity);
            return gameEntities;
        }

        internal GameEntity_Attribute_Name[] Get_Races()
        {
            GameEntity_Attribute_Name[] races = new GameEntity_Attribute_Name[ROSTER_ENTRIES.Length];
            for (int i = 0; i < ROSTER_ENTRIES.Length; i++)
                races[i] = ROSTER_ENTRIES[i].Entity.Race;
            return races;
        }

        internal bool CheckIf_Team_Is_Ready()
        {
            foreach (GameEntity_RosterEntry entry in ROSTER_ENTRIES)
                if (!(entry?.Is_Ready) ?? true)
                    return false;
            return true;
        }
    }
}
