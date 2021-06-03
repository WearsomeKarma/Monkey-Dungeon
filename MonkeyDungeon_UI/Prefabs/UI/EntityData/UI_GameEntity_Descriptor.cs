using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_UI.Prefabs.UI.EntityData
{
    public class UI_GameEntity_Descriptor
    {
        public readonly string RACE;

        private bool isDead = false;
        public bool IsDead { get => isDead; internal set => Set_Death_State(value); }
        private void Set_Death_State(bool state) { isDead = state; Entity_Died?.Invoke(this); }

        private bool isDismissed = false;
        public bool IsDismissed { get => isDismissed; internal set => Set_Dismissed_State(value); }
        private void Set_Dismissed_State(bool state) { isDismissed = state; Entity_Dismissal_State_Changed?.Invoke(this); }

        public uint UNIQUE_IDENTIFIER { get; internal set; }
        public readonly GameEntity_Attribute_Name[] ABILITY_NAMES = new GameEntity_Attribute_Name[MD_PARTY.MAX_ABILITY_COUNT];

        public readonly List<UI_GameEntity_Resource> RESOURCES = new List<UI_GameEntity_Resource>();

        public UI_GameEntity_Resource Level { get; private set; }
        public UI_GameEntity_Resource Ability_Points { get; private set; }

        public readonly int SCENE_ID;
        public readonly int INITATIVE_ORDER;

        public event Action<UI_GameEntity_Resource> Resource_Added;
        public event Action<UI_GameEntity_Descriptor> Entity_Died;
        public event Action<UI_GameEntity_Descriptor> Entity_Dismissal_State_Changed;


        public UI_GameEntity_Descriptor(string race, bool isDismissed = false)
        {
            Level = new UI_GameEntity_Resource(MD_VANILLA_RESOURCES.RESOURCE_LEVEL);
            Ability_Points = new UI_GameEntity_Resource(MD_VANILLA_RESOURCES.RESOURCE_ABILITYPOINTS, 2);

            this.isDismissed = isDismissed;

            RACE = race;
            UNIQUE_IDENTIFIER = 0;
        }
        
        internal void Set_Ability(GameEntity_Ability_Index abilityIndex, GameEntity_Attribute_Name abilityName)
        {
            ABILITY_NAMES[abilityIndex] = abilityName;
        }

        internal void Add_Resource(string resourceName, float initalPercentage = 1)
        {
            UI_GameEntity_Resource r;
            RESOURCES.Add(r = new UI_GameEntity_Resource(resourceName, initalPercentage));
            Resource_Added?.Invoke(r);
        }

        internal void Set_Resource_Percentage(string resourceName, float percentage)
        {
            foreach (UI_GameEntity_Resource resource in RESOURCES)
                if (resource.Resource_Name == resourceName)
                    resource.Resource_Percentage = percentage;
        }

        internal UI_GameEntity_Resource Get_Resource(string resourceName)
        {
            foreach (UI_GameEntity_Resource resource in RESOURCES)
                if (resource.Resource_Name == resourceName)
                    return resource;
            return null;
        }

        internal void Dispose_Resources()
        {
            foreach (UI_GameEntity_Resource resource in RESOURCES)
                resource.Remove_Resource();
            RESOURCES.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceChangedHandler"></param>
        /// <param name="resourceName">Null if subscribing to everyting.</param>
        internal void Subscribe_To_Resource_Changes(ResourceBar[] resourceBars)
        {
            foreach (ResourceBar resourceBar in resourceBars)
                foreach (UI_GameEntity_Resource resource in RESOURCES)
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
                foreach (UI_GameEntity_Resource resource in RESOURCES)
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
