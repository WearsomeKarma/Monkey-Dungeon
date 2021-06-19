using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Prefabs.UI;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_UI.Prefabs
{
    public sealed class GameEntity_ClientSide : GameEntity
    {
        public void Set_Incapacitated_Status(bool status)
        {
            IsIncapacitated = status;
            if(status)
                UI_EntityObject.Entity_Died();
        }

        public void Set_Dismissed_Status(bool status)
        {
            IsDismissed = status;
            UI_EntityObject.Entity_Dismissal_State_Changed(status);
        }

        public void Set_Cosmetic_Id(uint id)
        {
            GameEntity_Cosmetic_ID = id;
            UI_EntityObject?.Set_Unique_ID(id);
        }

        public readonly GameEntity_ClientSide_Ability[] ABILITIES = new GameEntity_ClientSide_Ability[MD_PARTY.MAX_ABILITY_COUNT];

        public readonly List<GameEntity_ClientSide_Resource> RESOURCES = new List<GameEntity_ClientSide_Resource>();
        private void Resource_Updated(GameEntity_ClientSide_Resource resource)
        {
            UI_EntityObject?.Resource_Updated(resource);
        }

        public GameEntity_ClientSide_Resource Level { get; private set; }
        public GameEntity_ClientSide_Resource Ability_Points { get; private set; }

        public readonly int SCENE_ID;

        private UI_EntityObject UI_EntityObject { get; set; }
        public void Bind_To__UI_EntityObject(UI_EntityObject uiEntityObject)
        {
            UI_EntityObject = uiEntityObject;
            UI_EntityObject.Set_Unique_ID(GameEntity_Cosmetic_ID);
            UI_EntityObject.Set_Race(GameEntity_Race, GameEntity_Cosmetic_ID);
        }

        public GameEntity_ClientSide(GameEntity_Attribute_Name_Race race, GameEntity_Position position, GameEntity_ID id = null, bool isDismissed = false)
        {
            Level = new GameEntity_ClientSide_Resource(MD_VANILLA_RESOURCE_NAMES.RESOURCE_LEVEL);
            Ability_Points = new GameEntity_ClientSide_Resource(MD_VANILLA_RESOURCE_NAMES.RESOURCE_ABILITYPOINTS, 2);

            IsDismissed = isDismissed;

            GameEntity_Position = position;
            GameEntity_Race = race;
            GameEntity_Cosmetic_ID = 0;

            GameEntity_ID = id ?? GameEntity_ID.ID_NULL;
        }
        
        internal void Set_Ability(GameEntity_Ability_Index abilityIndex, GameEntity_Attribute_Name_Ability abilityName)
        {
            ABILITIES[abilityIndex] = new GameEntity_ClientSide_Ability(abilityName);
            //TODO: make it take target type from server.
            ABILITIES[abilityIndex].SurveyTarget.Bind_To_Action(GameEntity_Position, GameEntity_Team_ID, Combat_Target_Type.One_Enemy, true);
        }

        internal void Set_Ability_Target_Type(GameEntity_Attribute_Name_Ability abilityName,
            Combat_Target_Type targetType)
        {
            foreach (GameEntity_ClientSide_Ability ability in ABILITIES)
            {
                if (ability.Ability_Name == abilityName)
                {
                    ability.Set_Target_Type(targetType);
                    return;
                }
            }
        }

        internal void Add_Resource(GameEntity_Attribute_Name_Resource resourceName, float initalPercentage = 1)
        {
            GameEntity_ClientSide_Resource r;
            RESOURCES.Add(r = new GameEntity_ClientSide_Resource(resourceName, initalPercentage));

            r.Resource_Updated += Resource_Updated;
        }

        internal void Set_Resource_Percentage(GameEntity_Attribute_Name resourceName, float percentage)
        {
            foreach (GameEntity_ClientSide_Resource resource in RESOURCES)
                if (resource.Resource_Name == resourceName)
                    resource.Resource_Percentage = percentage;
        }

        internal GameEntity_ClientSide_Resource Get_Resource(string resourceName)
        {
            foreach (GameEntity_ClientSide_Resource resource in RESOURCES)
                if (resource.Resource_Name == resourceName)
                    return resource;
            return null;
        }

        internal void Dispose_Resources()
        {
            foreach (GameEntity_ClientSide_Resource resource in RESOURCES)
                resource.Remove_Resource();
            RESOURCES.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceChangedHandler"></param>
        /// <param name="resourceName">Null if subscribing to everyting.</param>
        internal void Subscribe_To_Resource_Changes(UI_ResourceBar[] resourceBars)
        {
            foreach (UI_ResourceBar resourceBar in resourceBars)
                foreach (GameEntity_ClientSide_Resource resource in RESOURCES)
                    if (resource.Resource_Name == resourceBar.Resource_Name)
                        resourceBar.Attach_To_Resource(resource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceChangedHandler"></param>
        /// <param name="resourceName">Null if unsubscribing from everything.</param>
        internal void Unsubscribe_To_Resource_Changes(UI_ResourceBar[] resourceBars)
        {
            foreach (UI_ResourceBar resourceBar in resourceBars)
                foreach (GameEntity_ClientSide_Resource resource in RESOURCES)
                    if (resource.Resource_Name == resourceBar.Resource_Name)
                        resourceBar.Attach_To_Resource(null);
        }

        private bool IsMatch_With_Nullable (GameEntity_ClientSide_Resource clientSideResource, string resourceName)
        {
            bool? match = clientSideResource.Resource_Name == resourceName;
            return match ?? true;
        }
    }
}
