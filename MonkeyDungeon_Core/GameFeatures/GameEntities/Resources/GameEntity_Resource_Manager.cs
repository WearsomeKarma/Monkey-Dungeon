using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Resources
{
    public class GameEntity_Resource_Manager
    {
        private readonly GameEntity MANAGED_ENTITY;

        public event Action<GameEntity_Resource> Resources_Updated;

        private readonly List<GameEntity_Resource> RESOURCES    = new List<GameEntity_Resource>();
        public GameEntity_Resource[] Get_Resources              () => RESOURCES.ToArray();
        public GameEntity_Attribute_Name[] Get_Resource_Names                      ()
        {
            GameEntity_Attribute_Name[] ret = new GameEntity_Attribute_Name[RESOURCES.Count];
            for (int i = 0; i < RESOURCES.Count; i++)
                ret[i] = RESOURCES[i].ATTRIBUTE_NAME;
            return ret;
        }
        public T Get_ResourceByType<T>                          (GameEntity_Attribute_Name resourceName = null) where T : GameEntity_Resource { foreach (T resource in RESOURCES.OfType<T>()) if (resource.ATTRIBUTE_NAME == resourceName) return resource; return null; }
        public GameEntity_Resource Get_Resource                 (GameEntity_Attribute_Name resourceName) => Get_ResourceByType<GameEntity_Resource>(resourceName);
        public double Get_Resource_Percentage                   (GameEntity_Attribute_Name resourceName) { GameEntity_Resource r = Get_Resource(resourceName); return r.Value / r.Max_Quantity; }
        public void Add_Resource<T>                             (T resource) where T : GameEntity_Resource
        {
            resource.Quantity_Changed += 
                (e) => Resources_Updated?.Invoke(e as GameEntity_Resource);
            RESOURCES.Add(resource);
            resource.Attach_To_Entity(MANAGED_ENTITY);
        }
        public void Remove_Resources<T>                         () where T : GameEntity_Resource { foreach (T resource in RESOURCES.ToArray()) RESOURCES.Remove(resource); }
        public void Replace_Resource<T>                         (T resource) where T : GameEntity_Resource { Remove_Resources<T>(); Add_Resource(resource); }

        public GameEntity_Resource_Manager(GameEntity managedEntity, List<GameEntity_Resource> resources = null)
        {
            MANAGED_ENTITY = managedEntity;

            if (resources != null)
            {
                foreach (GameEntity_Resource resource in resources)
                {
                    RESOURCES.Add(resource);
                    resource.Attach_To_Entity(MANAGED_ENTITY);
                }
            }
        }
    }
}
