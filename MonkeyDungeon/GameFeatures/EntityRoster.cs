using MonkeyDungeon.Components;
using MonkeyDungeon.Prefabs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class EntityRoster
    {
        public int EntityCount { get; private set; }
        private EntityComponent[] entities;
        public EntityComponent[] Entities => entities.ToArray();

        public EntityRoster(EntityComponent[] entities)
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
            entities[EntityCount].ParentObject?.SpriteComponent.Toggle(state);
            EntityCount += (state) ? 1 : -1;
            return ret;
        }

        public int Mutate_EntityComponent(int index, EntityComponent value)
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
