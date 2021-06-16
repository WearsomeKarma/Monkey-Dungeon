using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources
{
    public sealed class GameEntity_Resource_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        public event Action<GameEntity_Resource> Resources_Updated;

        private readonly List<GameEntity_Resource> RESOURCES    = new List<GameEntity_Resource>();
        public GameEntity_Resource[] Get__Resources              () => RESOURCES.ToArray();
        public GameEntity_Attribute_Name[] Get__Resource_Names                      ()
        {
            GameEntity_Attribute_Name[] ret = new GameEntity_Attribute_Name[RESOURCES.Count];
            for (int i = 0; i < RESOURCES.Count; i++)
                ret[i] = RESOURCES[i].Attribute_Name;
            return ret;
        }
        public T Get__Resource<T>                                (GameEntity_Attribute_Name resourceName) where T : GameEntity_Resource { foreach (T resource in RESOURCES.OfType<T>()) if (resource.Attribute_Name == resourceName) return resource; return null; }
        public void Add__Resource<T>                             (T resource) where T : GameEntity_Resource
        {
            resource.Quantity_Changed += 
                (e) => Resources_Updated?.Invoke(e as GameEntity_Resource);
            RESOURCES.Add(resource);
            resource.Attach_To_Entity(ATTACHED_ENTITY);
        }
        public void Remove__Resources<T>                         () where T : GameEntity_Resource { foreach (T resource in RESOURCES.ToArray()) RESOURCES.Remove(resource); }
        public void Replace__Resource<T>                         (T resource) where T : GameEntity_Resource { Remove__Resources<T>(); Add__Resource(resource); }

        internal GameEntity_Resource_Manager(GameEntity_ServerSide attachedEntity, List<GameEntity_Resource> resources = null)
        {
            ATTACHED_ENTITY = attachedEntity;

            if (resources != null)
            {
                foreach (GameEntity_Resource resource in resources)
                {
                    RESOURCES.Add(resource);
                    resource.Attach_To_Entity(ATTACHED_ENTITY);
                }
            }
        }
    }
}
