using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;

namespace MonkeyDungeon_Core.GameFeatures.EntityResourceManagement
{
    public class GameEntity_Resource_Manager
    {
        private readonly GameEntity Entity;

        public event Action<GameEntity_Resource> Resources_Updated;

        private readonly List<GameEntity_Resource> Resources    = new List<GameEntity_Resource>();
        public GameEntity_Resource[] Get_Resources              () => Resources.ToArray();
        public T Get_ResourceByType<T>                          (string resourceName = null) where T : GameEntity_Resource { foreach (T resource in Resources.OfType<T>()) if (resource.IsEnabled && (resourceName == null || resource.Resource_Name == resourceName)) return resource; return null; }
        public GameEntity_Resource Get_Resource                 (string resourceName) => Get_ResourceByType<GameEntity_Resource>(resourceName);
        public double Get_Resource_Percentage                   (string resourceName) { GameEntity_Resource r = Get_Resource(resourceName); return r.Resource_StrictValue / r.Max_Value; }
        public void Add_Resource<T>                             (T resource) where T : GameEntity_Resource
        {
            resource.ValueChanged += 
                (e) => Resources_Updated?.Invoke(e);
            Resources.Add(resource);
            resource.Attach_ToEntity(Entity);
        }
        public void Remove_Resources<T>                         () where T : GameEntity_Resource { foreach (T resource in Resources.ToArray()) Resources.Remove(resource); }
        public void Replace_Resource<T>                         (T resource) where T : GameEntity_Resource { Remove_Resources<T>(); Add_Resource(resource); }

        public GameEntity_Resource_Manager(GameEntity managedEntity, List<GameEntity_Resource> resources = null)
        {
            Entity = managedEntity;

            if (resources != null)
                foreach (GameEntity_Resource resource in resources)
                    resource.Add_To_Entity(this);
        }

        internal double Recover<T>(double amount) where T : GameEntity_Resource
        {
            double diff = 0;
            foreach (T resource in Resources.OfType<T>())
                diff += resource.Offset(amount);
            return diff;
        }

        internal void Combat_BeginTurn(Combat_GameState combat)
        {
            foreach (GameEntity_Resource resource in Resources)
                resource.Combat_BeginTurn_ReplenishResource(combat);
        }
    }
}
