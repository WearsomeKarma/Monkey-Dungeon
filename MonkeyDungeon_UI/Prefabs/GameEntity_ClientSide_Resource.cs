using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;

namespace MonkeyDungeon_UI.Prefabs
{
    public class GameEntity_ClientSide_Resource
    {
        public event Action<GameEntity_ClientSide_Resource> Resource_Removed;

        public event Action<float> Resource_Updated;
        private float resource_Percentage;
        public float Resource_Percentage { get => resource_Percentage;
            set
            {
                resource_Percentage = value;
                Resource_Updated?.Invoke(resource_Percentage);
            }
        }

        public GameEntity_Attribute_Name_Resource Resource_Name { get; private set; }

        public GameEntity_ClientSide_Resource(GameEntity_Attribute_Name_Resource name, float initalPercentage=1)
        {
            Resource_Percentage = initalPercentage;
            Resource_Name = name;
        }

        internal void Remove_Resource()
        {
            Resource_Removed?.Invoke(this);
        }
    }
}
