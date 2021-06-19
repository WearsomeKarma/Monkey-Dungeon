using MonkeyDungeon_Vanilla_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameComponents.EntityQuantities.Resources
{
    public sealed class GameEntity_Resource_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        public event Action<GameEntity_ServerSide_Resource> Event__Resource_Updated;

        private readonly List<GameEntity_ServerSide_Resource> RESOURCES    = new List<GameEntity_ServerSide_Resource>();
        public GameEntity_ServerSide_Resource[] Get__Resources              () => RESOURCES.ToArray();
        public GameEntity_Attribute_Name[] Get__Resource_Names                      ()
        {
            GameEntity_Attribute_Name[] ret = new GameEntity_Attribute_Name[RESOURCES.Count];
            for (int i = 0; i < RESOURCES.Count; i++)
                ret[i] = RESOURCES[i].Attribute_Name;
            return ret;
        }
        public T Get__Resource<T>                                (GameEntity_Attribute_Name resourceName) where T : GameEntity_ServerSide_Resource { foreach (T resource in RESOURCES.OfType<T>()) if (resource.Attribute_Name == resourceName) return resource; return null; }
        public void Add__Resource<T>                             (T resource) where T : GameEntity_ServerSide_Resource
        {
            resource.Event__Quantity_Changed__Quantity += Relay__Resource;
            RESOURCES.Add(resource);
            resource.Attach_To__Entity__ServerSide_Resource(ATTACHED_ENTITY);
        }
        public void Remove__Resource<T>                          (T resource) where T : GameEntity_ServerSide_Resource
        {
            resource.Event__Quantity_Changed__Quantity -= Relay__Resource;
            RESOURCES.Remove(resource);
            resource.Detach_From__Entity__ServerSide_Resource();
        }
        public void Remove__Resources<T>                         () where T : GameEntity_ServerSide_Resource { foreach (T resource in RESOURCES.ToArray()) Remove__Resource(resource);}
        public void Replace__Resource<T>                         (T resource) where T : GameEntity_ServerSide_Resource { Remove__Resources<T>(); Add__Resource(resource); }

        internal GameEntity_Resource_Manager(GameEntity_ServerSide attachedEntity, List<GameEntity_ServerSide_Resource> resources = null)
        {
            ATTACHED_ENTITY = attachedEntity;

            if (resources != null)
            {
                foreach (GameEntity_ServerSide_Resource resource in resources)
                {
                    Add__Resource(resource);
                }
            }
        }

        private void Relay__Resource(GameEntity_Quantity<GameEntity_ServerSide> quantityAsResource)
        {
            Console.WriteLine("[ge_res_manager.cs:55] relay? " + quantityAsResource);
            Event__Resource_Updated?.Invoke(quantityAsResource as GameEntity_ServerSide_Resource);
        }
    }
}
