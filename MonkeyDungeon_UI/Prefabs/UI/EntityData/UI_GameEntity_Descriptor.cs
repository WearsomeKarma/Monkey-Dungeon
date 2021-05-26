using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI.EntityData
{
    public class UI_GameEntity_Descriptor
    {
        public readonly string RACE;
        public uint UNIQUE_IDENTIFIER { get; internal set; }
        public string[] Ability_Names { get; private set; }

        public List<UI_GameEntity_Resource> Resources { get; set; }

        public UI_GameEntity_Resource Level { get; private set; }
        public UI_GameEntity_Resource Ability_Points { get; private set; }

        public readonly int SCENE_ID;
        public readonly int INITATIVE_ORDER;

        public event Action<UI_GameEntity_Resource> Resource_Added;

        public UI_GameEntity_Descriptor(string race, uint uid = 0, string[] abilityNames = null, string[] resourceNames = null)
        {
            Level = new UI_GameEntity_Resource(MD_VANILLA_RESOURCES.RESOURCE_LEVEL);
            Ability_Points = new UI_GameEntity_Resource(MD_VANILLA_RESOURCES.RESOURCE_ABILITYPOINTS, 2);

            RACE = race;
            UNIQUE_IDENTIFIER = uid;
            Set_Abilities(abilityNames);
            Resources = new List<UI_GameEntity_Resource>();
            Set_Resources(resourceNames);
        }
        
        internal void Set_Abilities(string[] abilityNames)
        {
            Ability_Names = abilityNames ?? new string[] { };
        }

        internal void Set_Resources(string[] resourceNames)
        {
            Dispose_Resources();
            if (resourceNames == null)
                return;

            for (int i = 0; i < resourceNames.Length; i++)
            {
                Add_Resource(resourceNames[i]);
            }
        }

        internal void Add_Resource(string resourceName, float initalPercentage = 1)
        {
            UI_GameEntity_Resource r;
            Resources.Add(r = new UI_GameEntity_Resource(resourceName, initalPercentage));
            Resource_Added?.Invoke(r);
        }

        internal void Set_Resource_Percentage(string resourceName, float percentage)
        {
            foreach (UI_GameEntity_Resource resource in Resources)
                if (resource.Resource_Name == resourceName)
                    resource.Resource_Percentage = percentage;
        }

        internal UI_GameEntity_Resource Get_Resource(string resourceName)
        {
            foreach (UI_GameEntity_Resource resource in Resources)
                if (resource.Resource_Name == resourceName)
                    return resource;
            return null;
        }

        internal void Dispose_Resources()
        {
            foreach (UI_GameEntity_Resource resource in Resources)
                resource.Remove_Resource();
            Resources.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceChangedHandler"></param>
        /// <param name="resourceName">Null if subscribing to everyting.</param>
        internal void Subscribe_To_Resource_Changes(ResourceBar[] resourceBars)
        {
            foreach (ResourceBar resourceBar in resourceBars)
                foreach (UI_GameEntity_Resource resource in Resources)
                    if (resource.Resource_Name == resourceBar.Resource_Name)
                        resourceBar.Attach_To_Resource(resource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceChangedHandler"></param>
        /// <param name="resourceName">Null if unsubscribing from everything.</param>
        internal void Unsubscribe_To_Resource_Changes(ResourceBar[] resourceBars)
        {
            foreach (ResourceBar resourceBar in resourceBars)
                foreach (UI_GameEntity_Resource resource in Resources)
                    if (resource.Resource_Name == resourceBar.Resource_Name)
                        resourceBar.Attach_To_Resource(null);
        }

        private bool IsMatch_With_Nullable (UI_GameEntity_Resource resource, string resourceName)
        {
            bool? match = resource.Resource_Name == resourceName;
            return match ?? true;
        }
    }
}
