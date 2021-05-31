using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Linq;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Roster
    {
        private GameState_Machine Game { get; set; }

        public int EntityCount { get; private set; }
        private GameEntity[] entities;
        public GameEntity[] Entities => entities.ToArray();
        private bool[] readyEntities;
        public bool CheckIf_Team_Is_Ready()
        {
            bool ret = true;
            for (int i = 0; i < readyEntities.Length && ret; i++)
                ret = readyEntities[i];
            return ret;
        }

        public GameEntity_Roster(GameState_Machine game, GameEntity[] entities)
        {
            Game = game;
            this.entities = new GameEntity[entities.Length];
            foreach (GameEntity entity in entities)
                Set_Entity(entity);
            readyEntities = new bool[entities.Length];
            EntityCount = entities.Length-1;
        }

        public void ToggleAllEntities(bool state)
        {
            while (ToggleEntity(state) > 0) ;
        }

        /// <summary>
        /// State 0: ready | !0: not ready.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="state"></param>
        internal void Set_Ready_To_Start(int entityId, bool state = true)
        {
            readyEntities[entityId] = state;
        }

        /// <summary>
        /// Returns index of toggled entity.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public int ToggleEntity(bool state)
        {
            int ret;
            if ((state && EntityCount >= entities.Length)
                ||
                (!state && EntityCount <= 0))
                return -1;

            ret = EntityCount;
            //throw new NotImplementedException();
            //entities[EntityCount].ParentObject?.SpriteComponent.Toggle(state);
            EntityCount += (state) ? 1 : -1;
            return ret;
        }

        public GameEntity Set_Entity(GameEntity gameEntity)
        {
            if (gameEntity == null)
                return null;

            entities[gameEntity.Scene_GameObject_ID % entities.Length] = gameEntity;
            gameEntity.Game = Game;
            gameEntity.Resource_Manager.Resources_Updated += (e) => Game.Relay_Entity_Resource(e);
            throw new NotImplementedException(); //TODO: ability point linkage?
            return gameEntity;
        }
        
        public GameEntity[] Set_Entities(GameEntity[] gameEntities)
        {
            foreach (GameEntity gameEntity in gameEntities)
                Set_Entity(gameEntity);
            return gameEntities;
        }

        internal GameEntity_Attribute_Name[] Get_Races()
        {
            GameEntity_Attribute_Name[] races = new GameEntity_Attribute_Name[entities.Length];
            for (int i = 0; i < entities.Length; i++)
                races[i] = entities[i].Race;
            return races;
        }
    }
}
