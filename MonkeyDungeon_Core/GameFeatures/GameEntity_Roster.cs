using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures
{
    public class GameEntity_Roster
    {
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

        public GameEntity_Roster(GameEntity[] entities)
        {
            this.entities = entities.ToArray();
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
        internal void Set_Ready_To_Start(int entityId, int state=0)
        {
            readyEntities[entityId] = state == 0;
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
            entities[gameEntity.Scene_GameObject_ID % entities.Length] = gameEntity;
            return gameEntity;
        }

        internal string[] Get_Races()
        {
            string[] races = new string[entities.Length];
            for (int i = 0; i < entities.Length; i++)
                races[i] = entities[i].Race;
            return races;
        }
    }
}
