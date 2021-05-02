using MonkeyDungeon.Prefabs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class GameEntity_Roster
    {
        public int EntityCount { get; private set; }
        private GameEntity[] entities;
        public GameEntity[] Entities => entities.ToArray();

        public GameEntity_Roster(GameEntity[] entities)
        {
            this.entities = entities.ToArray();
            EntityCount = entities.Length-1;
        }

        public void ToggleAllEntities(bool state)
        {
            while (ToggleEntity(state) > 0) ;
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
            throw new NotImplementedException();
            //entities[EntityCount].ParentObject?.SpriteComponent.Toggle(state);
            EntityCount += (state) ? 1 : -1;
            return ret;
        }

        public int Mutate_EntityComponent(int index, GameEntity value)
        {
            if (index >= EntityCount)
                index = EntityCount;
            else if (index < 0)
                index = 0;

            entities[index] = value;

            return index;
        }
    }
}
